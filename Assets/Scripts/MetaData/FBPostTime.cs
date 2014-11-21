using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FBPostTime : PersistantMetaData 
{
	//time of fb post to wall
	public string postTime;

	/// <summary>
	/// Load this instance.
	/// </summary>
	public static FBPostTime Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<FBPostTime>())
		{
			//create new one
			FBPostTime newFile = new FBPostTime();
			
			newFile.postTime = "";
			
			SaveLoadManager.SharedManager.Save(newFile);
			
			return newFile;
		}
		
		return SaveLoadManager.SharedManager.Load<FBPostTime> ();
	}
	
	/// <summary>
	/// Save data.
	/// </summary>
	/// <param name="data">Data.</param>
	public static bool Save(FBPostTime data)
	{
		return SaveLoadManager.SharedManager.Save (data);
		
		
	}
}
