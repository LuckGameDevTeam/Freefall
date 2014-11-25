using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

/// <summary>
/// Save load manager.
/// 
/// This class provide persistant data saving and loading.
/// 
/// Manager require your save data class to derive from PersistantMetaData class 
/// otherwise manager will not save your data
/// 
/// Notice: It is not recommend to manual give file name and file extension when saving and loading, 
/// using class type is highly recommend.
/// </summary>
public class SaveLoadManager 
{
	/// <summary>
	/// Singletone instant of manager
	/// </summary>
	private static SaveLoadManager sharedMgr;

	/// <summary>
	/// BinaryFormatter
	/// </summary>
	private BinaryFormatter bf;

	/// <summary>
	/// File save directory
	/// </summary>
	private string fileDirectory = Application.persistentDataPath+"/SLM_SaveData";

	/// <summary>
	/// file extension
	/// </summary>
	private const string fileEx = "data";

	public SaveLoadManager() : base()
	{
		//create BinaryFormatter
		bf = new BinaryFormatter ();

		//check if save data directory exist
		if(!Directory.Exists(fileDirectory))
		{
			//create one if it is not exist
			Directory.CreateDirectory(fileDirectory);

			Debug.Log("Create directory SaveData at: "+fileDirectory);
		}

	}

	~SaveLoadManager()
	{
		if(sharedMgr != null)
		{
			sharedMgr = null;
		}
	}

	/// <summary>
	/// Sets the environment variables.
	/// 
	/// Due to IOS throw an exception while in serialization process
	/// ExecutionEngineException: Attempting to JIT compile method 'List 1__TypeMetadata:.ctor ()' while running with --aot-only
	/// 
	/// IOS plateform do not allow any code to be generated at runtime, therefore, JIT(Just-in-time compiler) is not compatible
	/// 
	/// This use different code path instead of runtime
	/// 
	/// Environment variable must be set for IOS before any serialization.
	/// </summary>
	private void SetEnvironmentVariables() 
	{
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}

	/////////////////////////////////////////////////////Persistant data saving/////////////////////////////////////////////////////

	/// <summary>
	/// Save persistant data
	/// 
	/// return true successful or false fail
	/// </summary>
	/// <param name="dataToSave">Data to save.</param>
	/// <param name="fileName">if null then saved file name will use class's name.</param>
	/// <param name="fileExtension">File extension.</param>
	/// <typeparam name="T">The type of save data.</typeparam>
	public bool Save<T>(T dataToSave, string fileName = null, string fileExtension = fileEx)
	{
		//check if save data class is derive from PersistantMetaData... no return
		if(!(dataToSave is PersistantMetaData))
		{
			Debug.LogError("Save data fail, class:"+dataToSave.GetType().ToString()+". The class of data you pass in is not derived from PersistantMetaData class");

			return false;
		}

		//check if class is serializable or not
		if((dataToSave.GetType().Attributes & TypeAttributes.Serializable) == 0)
		{
			Debug.LogError("Save data fail, class:"+dataToSave.GetType().ToString()+". You must put [Serializable] before class declaration");

			return false;
		}

		//IOS plateform require set environment before serialization
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			SetEnvironmentVariables();
		}


		//if fileName is null...use class's name
		if(fileName == null)
		{
			//use class's name
			fileName = dataToSave.GetType().ToString();
		}

		//open and create file
		FileStream file = File.Open (fileDirectory + "/" + fileName + "." + fileExtension, FileMode.Create);

		//serialize data and save
		bf.Serialize (file, dataToSave);

		//close file
		file.Close ();

		return true;

	}

	/////////////////////////////////////////////////////Persistant data saving/////////////////////////////////////////////////////


	/////////////////////////////////////////////////////Persistant data loading/////////////////////////////////////////////////////

	/// <summary>
	/// Load the specific persistant file
	/// 
	/// return instance of class or null if no file exist
	/// </summary>
	/// <param name="fileName">File name.</param>
	/// <param name="fileExtension">File extension.</param>
	/// <typeparam name="T">The st type parameter.</typeparam>
	public T Load<T>(string fileName, string fileExtension = fileEx)
	{
		//if file does not exists return null
		if(!File.Exists(fileDirectory + "/" + fileName + "." + fileExtension))
		{
			Debug.LogWarning(fileName+"."+fileExtension+" can not be loaded, file does not exists");
			return default(T);
		}

		//IOS plateform require set environment before serialization
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			SetEnvironmentVariables();
		}

		//open file
		FileStream file = File.Open (fileDirectory + "/" + fileName + "." + fileExtension, FileMode.Open);

		//deserialze data
		T retData = (T)bf.Deserialize (file);

		//close file
		file.Close ();

		return retData;
	}

	/// <summary>
	///  Load the specific persistant file by class Type.
	/// 
	/// return instance of class or null if no file exist
	/// </summary>
	/// <typeparam name="T">The type of save data.</typeparam>
	public T Load<T>()
	{

		return Load<T> ((typeof(T)).ToString(), fileEx);

	}

	/////////////////////////////////////////////////////Persistant data loading/////////////////////////////////////////////////////


	/////////////////////////////////////////////////////Delete persistant data/////////////////////////////////////////////////////

	/// <summary>
	/// Delete all persistant data.
	/// </summary>
	public void DeleteAllSaveData()
	{
		//check if save data directory exist
		if(Directory.Exists(fileDirectory))
		{
			//get directory info
			DirectoryInfo dirInfo = new DirectoryInfo(fileDirectory);

			//get all files inside directory
			FileInfo[] fileInfos = dirInfo.GetFiles();

			//if has more than one file
			if(fileInfos.Length > 0)
			{
				//delete all file
				for(int i=0; i<fileInfos.Length; i++)
				{
					FileInfo fileInfo = fileInfos[i];

					File.Delete(fileDirectory+"/"+fileInfo.Name);

					Debug.Log("Saved file: "+fileInfo.Name+" has been deleted");
				}
			}
			else
			{
				Debug.LogWarning("Directory is empty");
			}
		}
	}

	/// <summary>
	/// Delete single persistant data.
	/// </summary>
	/// <param name="fileName">File name.</param>
	/// <param name="fileExtension">File extension.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void DeleteSaveData<T>(string fileName, string fileExtension = fileEx)
	{
		//if file does not exists return null
		if(!File.Exists(fileDirectory + "/" + fileName + "." + fileExtension))
		{
			Debug.LogWarning(fileName+"."+fileExtension+" can not be deleted, file does not exists");
			return;
		}

		File.Delete (fileDirectory + "/" + fileName + "." + fileExtension);

		Debug.Log("Saved file: "+fileName+"."+fileExtension+" has been deleted");
	}

	/// <summary>
	/// Delete single persistant data by give class.
	/// </summary>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void DeleteSaveData<T>()
	{
		DeleteSaveData<T> (typeof(T).ToString (), fileEx);
	}

	/////////////////////////////////////////////////////Delete persistant data/////////////////////////////////////////////////////

	/////////////////////////////////////////////////////Persistant file checking/////////////////////////////////////////////////////

	/// <summary>
	/// Check file exist or not
	/// 
	/// return true file exist or false file does not exist
	/// </summary>
	/// <param name="fileName">give null then type of class will be used as file name.</param>
	/// <param name="fileExtension">File extension.</param>
	/// <typeparam name="T">The type of save data.</typeparam>
	public bool IsFileExist<T> (string fileName = null, string fileExtension = fileEx)
	{
		if(fileName == null)
		{
			fileName = (typeof(T)).ToString();
		}

		return File.Exists (fileDirectory + "/" + fileName + "." + fileEx);
	}

	/////////////////////////////////////////////////////Persistant file checking/////////////////////////////////////////////////////

	/// <summary>
	/// Gets the singleton of manager.
	/// </summary>
	/// <value>The shared manager.</value>
	public static SaveLoadManager SharedManager
	{
		get
		{
			if(sharedMgr == null)
			{
				sharedMgr = new SaveLoadManager();

			}

			return sharedMgr;
		}
	}

}
