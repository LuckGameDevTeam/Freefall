using UnityEngine;
using System.Collections;

public class LoginControl : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		ServerSync.SharedInstance.CreateAccount ();
	}

	void OnEnable()
	{
		ServerSync.SharedInstance.Evt_OnCreateAccountSuccess += OnCreateAccountSuccess;
		ServerSync.SharedInstance.Evt_OnCreateAccountFail += OnCreateAccountFail;
		ServerSync.SharedInstance.Evt_OnLoginSuccess += OnLoginSuccess;
		ServerSync.SharedInstance.Evt_OnLoginFail += OnLoginFail;
	}

	void OnDisable()
	{
		ServerSync.SharedInstance.Evt_OnCreateAccountSuccess -= OnCreateAccountSuccess;
		ServerSync.SharedInstance.Evt_OnCreateAccountFail -= OnCreateAccountFail;
		ServerSync.SharedInstance.Evt_OnLoginSuccess -= OnLoginSuccess;
		ServerSync.SharedInstance.Evt_OnLoginFail -= OnLoginFail;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCreateAccountSuccess(ServerSync syncControl, string username, string password)
	{
		UserProfile up = UserProfile.Load ();

		up.userName = username;
		up.password = password;

		UserProfile.Save (up);

		ServerSync.SharedInstance.LoginServer (username, password);
	}

	void OnCreateAccountFail(ServerSync syncControl, int errorCode)
	{

	}

	void OnLoginSuccess(ServerSync syncControl, string uid)
	{
		UserProfile up = UserProfile.Load ();

		up.uid = uid;

		UserProfile.Save (up);

		Debug.Log ("username:" + up.userName + " password:" + up.password + " uid:" + up.uid);
	}

	void OnLoginFail(ServerSync syncControl, int errorCode)
	{

	}
}
