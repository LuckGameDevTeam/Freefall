using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class ServerSync :MonoBehaviour
{
	//server api automatically create account
	const string addrCreateAccount = "http://ccat.eznewlife.com/user/fast_register";

	//server api login server
	const string addrLoginServer = "http://ccat.eznewlife.com/user/login";

	/// <summary>
	/// The internal error.
	/// </summary>
	const int internalError = -1;

	public delegate void OnInternetNotAvailable(ServerSync syncControl);
	/// <summary>
	/// Event when internet not available.
	/// </summary>
	public OnInternetNotAvailable Evt_OnInternetNotAvailable;

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

	/// <summary>
	/// Is creating account or not.
	/// </summary>
	private bool isCreatingAccount = false;

	/// <summary>
	/// Is logging into server or not.
	/// </summary>
	private bool isLoggingServer = false;

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

		WWW wGo = new WWW (addrCreateAccount);

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
			//parse json data
			JSONNode data = JSON.Parse(wGo.text);

			int error = int.Parse(data["error"].Value);

			//if data has an error
			if(error != 0)
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

				if(Evt_OnCreateAccountSuccess != null)
				{
					Evt_OnCreateAccountSuccess(this, username, password);
				}
			}
		}
	}
	#endregion CreateAccount

	#region Loggin server
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
		postData.AddField ("password", tmpLoginPassword);

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
			//parse json data
			JSONNode data = JSON.Parse(wGo.text);
			
			int error = int.Parse(data["error"].Value);

			//if data has an error
			if(error != 0)
			{
				Debug.LogError(gameObject.name+" return data has error:"+error);
				if(Evt_OnLoginFail != null)
				{
					Evt_OnLoginFail(this, error);
				}
			}
			else
			{
				//store uid
				uid = data["uid"].Value;
				
				if(Evt_OnLoginSuccess != null)
				{
					Evt_OnLoginSuccess(this, uid);
				}
			}
		}
	}
	#endregion Loggin server


}
