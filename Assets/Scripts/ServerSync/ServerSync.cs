using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using System.Text;

public class ServerSync :MonoBehaviour
{
	//server api automatically create account
	const string addrCreateAccount = "http://ccat.eznewlife.com/user/fast_register";

	//server api login server
	const string addrLoginServer = "http://ccat.eznewlife.com/user/login";

	//server api download/upload data
	const string addrDownloadUpload = "http://ccat.eznewlife.com/user/data";

	//server api change password
	const string addrChangePassword = "http://ccat.eznewlife.com/user/password";

	/// <summary>
	/// The internal error.
	/// </summary>
	const int internalError = -1;

	const int serverFatalError = -2;

	const string authName = "test01";
	const string authPass = "test01";

	public delegate void OnInternetNotAvailable(ServerSync syncControl);
	/// <summary>
	/// Event when internet not available.
	/// </summary>
	public OnInternetNotAvailable Evt_OnInternetNotAvailable;

	public delegate void OnAuthrizeSuccess(ServerSync syncControl);
	/// <summary>
	///  Event when authrize success.
	/// </summary>
	public OnAuthrizeSuccess Evt_OnAuthrizeSuccess;

	public delegate void OnAuthrizeFail(ServerSync syncControl, int errorCode);
	/// <summary>
	/// Event when authrize fail.
	/// </summary>
	public OnAuthrizeFail Evt_OnAuthrizeFail;

	public delegate void OnCreateAccountSuccess(ServerSync syncControl, string username, string password);
	/// <summary>
	/// Event when account create success.
	/// </summary>
	public OnCreateAccountSuccess Evt_OnCreateAccountSuccess;

	public delegate void OnCreateAccountFail(ServerSync syncControl, int errorCode);
	/// <summary>
	/// Event when account create fail.
	/// </summary>
	public OnCreateAccountFail Evt_OnCreateAccountFail;

	public delegate void OnLoginSuccess(ServerSync syncControl, string uid);
	/// <summary>
	/// Event when login success.
	/// </summary>
	public OnLoginSuccess Evt_OnLoginSuccess;

	public delegate void OnLoginFail(ServerSync syncControl, int errorCode);
	/// <summary>
	/// Event when login fail.
	/// </summary>
	public OnLoginFail Evt_OnLoginFail;

	public delegate void OnGetDataSuccess(ServerSync syncControl, string data);
	/// <summary>
	/// Event when get data success.
	/// </summary>
	public OnGetDataSuccess Evt_OnGetDataSuccess;

	public delegate void OnGetDataFail(ServerSync syncControl, int errorCode);
	/// <summary>
	/// Event when get data fail.
	/// </summary>
	public OnGetDataFail Evt_OnGetDataFail;

	public delegate void OnUploadDataSuccess(ServerSync synControl);
	/// <summary>
	/// Event when upload data success.
	/// </summary>
	public OnUploadDataSuccess Evt_OnUploadDataSuccess;

	public delegate void OnUploadDataFail(ServerSync syncControl, int errorCode);
	/// <summary>
	/// Event when upload data fail.
	/// </summary>
	public OnUploadDataFail Evt_OnUploadDataFail;

	public delegate void OnOtherDeviceLogin(ServerSync syncControl, int errorCode);
	/// <summary>
	/// Event when other device login.
	/// </summary>
	public OnOtherDeviceLogin Evt_OnOtherDeviceLogin;

	public delegate void OnChangePasswordSuccess(ServerSync syncControl, string newPassword);
	/// <summary>
	/// Event when change password success.
	/// </summary>
	public OnChangePasswordSuccess Evt_OnChangePasswordSuccess;

	public delegate void OnChangePasswordFail(ServerSync syncControl, int errorCode);
	/// <summary>
	/// Event when change password fail.
	/// </summary>
	public OnChangePasswordFail Evt_OnChangePasswordFail;

	/// <summary>
	/// The instance.
	/// </summary>
	private static ServerSync instance;

	public static ServerSync SharedInstance
	{
		get
		{


			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<ServerSync>();

				if(instance != null)
				{
					return instance; 
				}

				GameObject go = new GameObject("ServerSync");

				instance = go.AddComponent<ServerSync>();
			}
			else
			{
				//gameobject already inscene;
				return instance;
			}

			return instance;
		}
	}

	/// <summary>
	/// Is login to server or not.
	/// </summary>
	private bool isLogin = false;

	public bool IsLogedin{ get{ return isLogin; }}

	/// <summary>
	/// Is creating account or not.
	/// </summary>
	private bool isCreatingAccount = false;

	/// <summary>
	/// Is logging into server or not.
	/// </summary>
	private bool isLoggingServer = false;

	/// <summary>
	/// Is authrizing or not.
	/// </summary>
	private bool isAuthrizing = false;

	/// <summary>
	/// Is getting data.
	/// </summary>
	private bool isGettingData = false;

	/// <summary>
	/// Is uploading data.
	/// </summary>
	private bool isUploadingData = false;

	/// <summary>
	/// Is changing password.
	/// </summary>
	private bool isChangingPassword = false;

	/// <summary>
	/// The authrized.
	/// </summary>
	private bool authrized = false;

	public bool IsAuthrized{get{return authrized;}}

	/// <summary>
	/// The username that used to login server.
	/// </summary>
	private string username;

	public string UserName{ get{return username;}}

	/// <summary>
	/// The password that used to login server.
	/// </summary>
	private string password;

	public string Password{ get{return password;}}

	/// <summary>
	/// temp username to login server .
	/// </summary>
	private string tmpLoginUserName;

	/// <summary>
	/// temp password to login server
	/// </summary>
	private string tmpLoginPassword;

	/// <summary>
	/// The uid.
	/// uid will be used to veriefy with server
	/// </summary>
	private string uid;

	void Start()
	{
		DontDestroyOnLoad (gameObject);
	}

	void Update()
	{
#if UNITY_EDITOR
		if((Application.isEditor)&&(!Application.isPlaying))
		{
			ServerSync sync = GameObject.FindObjectOfType<ServerSync> ();
			
			if(sync)
				GameObject.DestroyImmediate (sync.gameObject);

		}
		
#endif
	}

	/// <summary>
	/// Determines whether is internet available or not.
	/// </summary>
	/// <returns><c>true</c> if this instance is internet available; otherwise, <c>false</c>.</returns>
	private bool IsInternetAvailable()
	{
		if(Application.internetReachability == NetworkReachability.NotReachable)
		{
			if(Evt_OnInternetNotAvailable != null)
			{
				Evt_OnInternetNotAvailable(this);
			}

			return false;
		}

		return true;
	}

	/// <summary>
	/// Gets the SHA512 encryption from string.
	/// </summary>
	/// <returns>The SH a512.</returns>
	/// <param name="str">String.</param>
	private string GetSHA512(string str)
	{
		byte[] hashByte;
		string retStr = "";

		System.Security.Cryptography.SHA512 sha512 = System.Security.Cryptography.SHA512.Create();

		hashByte = sha512.ComputeHash (Encoding.ASCII.GetBytes (str));

		//retStr = Encoding.ASCII.GetString (hashByte);

		foreach (byte x in hashByte) 
		{
			retStr += string.Format ("{0:x2}", x);
		}
		
		return retStr;
	}

	private string TrimStringForData(string dataStr)
	{
		string pattern = "/*****/";
		string retStr = dataStr;

		int firstTrimStartIndex = 0;
		int firstTrimLength = retStr.IndexOf (pattern)+pattern.Length;
		retStr = retStr.Remove (firstTrimStartIndex, firstTrimLength);

		int lastTrimStartIndex = retStr.LastIndexOf (pattern);
		int lastTrimLength = retStr.Length - (lastTrimStartIndex+1)+1;
		retStr = retStr.Remove (lastTrimStartIndex, lastTrimLength);

		return retStr;
	}

	#region Authrization
	public void Auth()
	{
		if(isAuthrizing)
		{
			return;
		}

		StartCoroutine ("DoAuth");
	}

	IEnumerator DoAuth()
	{
		isAuthrizing = true;

		WWWForm postData = new WWWForm ();
		postData.AddField ("name", authName);
		postData.AddField ("password", GetSHA512(authPass));
		postData.AddField ("device_id", SystemInfo.deviceUniqueIdentifier);


		WWW wGo = new WWW (addrLoginServer, postData);

		yield return wGo;

		isAuthrizing = false;

		//if there is an error while download data
		if(!string.IsNullOrEmpty(wGo.error))
		{
			Debug.LogError(gameObject.name+" "+wGo.error);
			
			if(Evt_OnAuthrizeFail != null)
			{
				Evt_OnAuthrizeFail(this, internalError);
			}
		}
		else
		{
			if(string.IsNullOrEmpty(wGo.text))
			{
				if(Evt_OnAuthrizeFail != null)
				{
					Evt_OnAuthrizeFail(this, serverFatalError);
				}
			}
			else
			{

				//parse json data
				JSONNode data = JSON.Parse(TrimStringForData(wGo.text));
				
				
				if((data == null) || (data == ""))
				{
					if(Evt_OnAuthrizeFail != null)
					{
						Evt_OnAuthrizeFail(this, serverFatalError);
					}

				}
				else
				{
					int error = int.Parse(data["error"].Value);

					if(error == 15)
					{
						if(Evt_OnOtherDeviceLogin != null)
						{
							Evt_OnOtherDeviceLogin(this, error);
						}
					}
					else if(error != 0)//if data has an error
					{
						Debug.LogError(gameObject.name+" return data has error:"+error);
						if(Evt_OnAuthrizeFail != null)
						{
							Evt_OnAuthrizeFail(this, error);
						}
					}
					else
					{
						authrized = true;

						uid = data["uid"];

						
						if(Evt_OnAuthrizeSuccess != null)
						{
							Evt_OnAuthrizeSuccess(this);
						}
					}
				}
			}
		}
	}
	#endregion Authrization
	
	#region CreateAccount
	/// <summary>
	/// Creates a new account from server automatically.
	/// </summary>
	public void CreateAccount()
	{

		if(!IsInternetAvailable())
		{
			return;
		}

		if(isCreatingAccount)
		{
			return;
		}
		else
		{
			StartCoroutine("DoCreateAccount");
		}
	}

	/// <summary>
	/// Do create account.
	/// Create account process is here
	/// </summary>
	/// <returns>The create account.</returns>
	IEnumerator DoCreateAccount()
	{
		isCreatingAccount = true;

		WWWForm postData = new WWWForm ();
		postData.AddField ("device_id", SystemInfo.deviceUniqueIdentifier);
		postData.AddField ("uid", uid);

		WWW wGo = new WWW (addrCreateAccount, postData);

		yield return wGo;

		isCreatingAccount = false;

		//if there is an error while download data
		if(!string.IsNullOrEmpty(wGo.error))
		{
			Debug.LogError(gameObject.name+" "+wGo.error);

			if(Evt_OnCreateAccountFail != null)
			{
				Evt_OnCreateAccountFail(this, internalError);
			}
		}
		else
		{
			if(string.IsNullOrEmpty(wGo.text))
			{
				if(Evt_OnCreateAccountFail != null)
				{
					Evt_OnCreateAccountFail(this, serverFatalError);
				}
			}
			else
			{
				//parse json data
				JSONNode data = JSON.Parse(TrimStringForData(wGo.text));
				
				
				if((data == null) || (data == ""))
				{
					if(Evt_OnCreateAccountFail != null)
					{
						Evt_OnCreateAccountFail(this, serverFatalError);
					}
					
				}
				else
				{
					int error = int.Parse(data["error"].Value);

					if(error == 15)
					{
						if(Evt_OnOtherDeviceLogin != null)
						{
							Evt_OnOtherDeviceLogin(this, error);
						}
					}
					else if(error != 0)//if data has an error
					{
						Debug.LogError(gameObject.name+" return data has error:"+error);
						if(Evt_OnCreateAccountFail != null)
						{
							Evt_OnCreateAccountFail(this, error);
						}
					}
					else
					{
						//store username and password
						username = data["name"].Value;
						password = data["password"].Value;
						
						Debug.Log("Server return username:"+username+" password:"+password);
						
						if(Evt_OnCreateAccountSuccess != null)
						{
							Evt_OnCreateAccountSuccess(this, username, password);
						}
					}
				}
			}


		}
	}
	#endregion CreateAccount

	#region Loggin server
	/// <summary>
	/// Log into server.
	/// </summary>
	/// <param name="username">Username.</param>
	/// <param name="password">Password.</param>
	public void LoginServer(string username, string password)
	{
		if(!IsInternetAvailable())
		{
			return;
		}

		if(isLoggingServer)
		{
			return;
		}
		else
		{
			tmpLoginUserName = username;
			tmpLoginPassword = password;

			StartCoroutine("DoLoginServer");
		}
	}

	/// <summary>
	/// Do login server.
	/// </summary>
	/// <returns>The login server.</returns>
	IEnumerator DoLoginServer()
	{
		isLoggingServer = true;

		WWWForm postData = new WWWForm ();
		postData.AddField ("name", tmpLoginUserName);
		postData.AddField ("password", GetSHA512 (tmpLoginPassword));
		postData.AddField ("device_id", SystemInfo.deviceUniqueIdentifier);


		WWW wGo = new WWW (addrLoginServer, postData);

		yield return wGo;

		isLoggingServer = false;

		//if there is an error while download data
		if(!string.IsNullOrEmpty(wGo.error))
		{
			Debug.LogError(gameObject.name+" "+wGo.error);
			
			if(Evt_OnLoginFail != null)
			{
				Evt_OnLoginFail(this, internalError);
			}
		}
		else
		{
			if(string.IsNullOrEmpty(wGo.text))
			{
				if(Evt_OnLoginFail != null)
				{
					Evt_OnLoginFail(this, serverFatalError);
				}
			}
			else
			{
				//parse json data
				JSONNode data = JSON.Parse(TrimStringForData(wGo.text));
				
				if((data == null) || (data == ""))
				{
					if(Evt_OnLoginFail != null)
					{
						Evt_OnLoginFail(this, serverFatalError);
					}
				}
				else
				{
					int error = int.Parse(data["error"].Value);

					if(error == 15)
					{
						if(Evt_OnOtherDeviceLogin != null)
						{
							Evt_OnOtherDeviceLogin(this, error);
						}
					}
					else if(error != 0)//if data has an error
					{
						Debug.LogError(gameObject.name+" return data has error:"+error);
						if(Evt_OnLoginFail != null)
						{
							Evt_OnLoginFail(this, error);
						}
					}
					else
					{
						isLogin = true;

						//store uid
						uid = data["uid"].Value;
						
						username = tmpLoginUserName;
						password = tmpLoginPassword;
						
						if(Evt_OnLoginSuccess != null)
						{
							Evt_OnLoginSuccess(this, uid);
						}
					}
				}
			}

		}
	}
	#endregion Loggin server

	/*
	#region Logout server
	public void Logout()
	{
		isLogin = false;
	}
	#endregion Logout server
	*/

	#region Change password
	public void ChangePassword(string newPassword)
	{
		if(!IsInternetAvailable())
		{
			return;
		}

		if(!isLogin)
		{
			return;
		}
		else if(isChangingPassword)
		{
			return;
		}
		else
		{
			StartCoroutine("DoChangePassword", newPassword);
		}
	}

	IEnumerator DoChangePassword(string newPassword)
	{
		isChangingPassword = true;

		WWWForm postData = new WWWForm ();
		postData.AddField ("uid", uid);
		postData.AddField ("password", GetSHA512 (newPassword));
		postData.AddField ("device_id", SystemInfo.deviceUniqueIdentifier);

		WWW wGo = new WWW (addrChangePassword, postData);
		
		yield return wGo;
		
		isChangingPassword = false;

		//if there is an error while download data
		if(!string.IsNullOrEmpty(wGo.error))
		{
			Debug.LogError(gameObject.name+" "+wGo.error);
			
			if(Evt_OnChangePasswordFail != null)
			{
				Evt_OnChangePasswordFail(this, internalError);
			}
		}
		else
		{
			if(string.IsNullOrEmpty(wGo.text))
			{
				if(Evt_OnChangePasswordFail != null)
				{
					Evt_OnChangePasswordFail(this, serverFatalError);
				}
			}
			else
			{
				//parse json data
				JSONNode data = JSON.Parse(TrimStringForData(wGo.text));
				
				if((data == null) || (data == ""))
				{
					if(Evt_OnChangePasswordFail != null)
					{
						Evt_OnChangePasswordFail(this, serverFatalError);
					}
				}
				else
				{
					int error = int.Parse(data["error"].Value);
					
					if(error == 15)
					{
						if(Evt_OnOtherDeviceLogin != null)
						{
							Evt_OnOtherDeviceLogin(this, error);
						}
					}
					else if(error != 0)//if data has an error
					{
						Debug.LogError(gameObject.name+" return data has error:"+error);
						if(Evt_OnChangePasswordFail != null)
						{
							Evt_OnChangePasswordFail(this, error);
						}
					}
					else
					{
						password = newPassword;
						
						if(Evt_OnChangePasswordSuccess != null)
						{
							Evt_OnChangePasswordSuccess(this, newPassword);
						}
					}
				}
			}
			
		}
	}
	#endregion Change password

	#region Get data from server
	public void GetServerData()
	{
		if(!IsInternetAvailable())
		{
			return;
		}

		if(isGettingData)
		{
			return;
		}
		else
		{
			StartCoroutine("DoGetServerData");
		}
	}

	IEnumerator DoGetServerData()
	{
		isGettingData = true;

		//modify address for Get method
		string newAddr = addrDownloadUpload+"?"+"uid="+uid+"&device_id="+SystemInfo.deviceUniqueIdentifier;

		WWW wGo = new WWW (newAddr);
		
		yield return wGo;

		isGettingData = false;

		//if there is an error while download data
		if(!string.IsNullOrEmpty(wGo.error))
		{
			Debug.LogError(gameObject.name+" "+wGo.error);
			
			if(Evt_OnGetDataFail != null)
			{
				Evt_OnGetDataFail(this, internalError);
			}
		}
		else
		{
			if(string.IsNullOrEmpty(wGo.text))
			{
				if(Evt_OnGetDataFail != null)
				{
					Evt_OnGetDataFail(this, serverFatalError);
				}
			}
			else
			{
				//parse json data
				JSONNode data = JSON.Parse(TrimStringForData(wGo.text));
				
				if((data == null) || (data == ""))
				{
					if(Evt_OnGetDataFail != null)
					{
						Evt_OnGetDataFail(this, serverFatalError);
					}
				}
				else
				{
					int error = int.Parse(data["error"].Value);

					if(error == 15)
					{
						if(Evt_OnOtherDeviceLogin != null)
						{
							Evt_OnOtherDeviceLogin(this, error);
						}
					}
					else if(error != 0)//if data has an error
					{
						Debug.LogError(gameObject.name+" return data has error:"+error);
						if(Evt_OnGetDataFail != null)
						{
							Evt_OnGetDataFail(this, error);
						}
					}
					else
					{
						string dataStr = data["response"].Value;
						
						if(Evt_OnGetDataSuccess != null)
						{
							Evt_OnGetDataSuccess(this, dataStr);
						}
					}
				}
			}
			
		}
	}
	#endregion Get data from server

	#region upload data to server
	public void UploadData(string dataToUpload)
	{
		if(!IsInternetAvailable())
		{
			return;
		}

		if(isUploadingData)
		{
			return;
		}
		else
		{
			StartCoroutine("DoUploadData", dataToUpload);
		}
	}

	IEnumerator DoUploadData(string uData)
	{
		isUploadingData = true;

		WWWForm postData = new WWWForm ();
		postData.AddField ("uid", uid);
		postData.AddField ("device_id", SystemInfo.deviceUniqueIdentifier);
		postData.AddField ("string", uData);
		
		WWW wGo = new WWW (addrDownloadUpload, postData);
		
		yield return wGo;

		isUploadingData = false;

		//if there is an error while download data
		if(!string.IsNullOrEmpty(wGo.error))
		{
			Debug.LogError(gameObject.name+" "+wGo.error);
			
			if(Evt_OnUploadDataFail != null)
			{
				Evt_OnUploadDataFail(this, internalError);
			}
		}
		else
		{
			if(string.IsNullOrEmpty(wGo.text))
			{
				if(Evt_OnUploadDataFail != null)
				{
					Evt_OnUploadDataFail(this, serverFatalError);
				}
			}
			else
			{
				//parse json data
				JSONNode data = JSON.Parse(TrimStringForData(wGo.text));
				
				if((data == null) || (data == ""))
				{
					if(Evt_OnUploadDataFail != null)
					{
						Evt_OnUploadDataFail(this, serverFatalError);
					}
				}
				else
				{
					int error = int.Parse(data["error"].Value);

					if(error == 15)
					{
						if(Evt_OnOtherDeviceLogin != null)
						{
							Evt_OnOtherDeviceLogin(this, error);
						}
					}
					else if(error != 0)//if data has an error
					{
						Debug.LogError(gameObject.name+" return data has error:"+error);
						if(Evt_OnUploadDataFail != null)
						{
							Evt_OnUploadDataFail(this, error);
						}
					}
					else
					{
						
						if(Evt_OnUploadDataSuccess != null)
						{
							Evt_OnUploadDataSuccess(this);
						}
					}
				}
			}
			
		}
	}
	#endregion upload data to server
}
