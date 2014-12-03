using UnityEngine;
using System.Collections;

public class LoginControl : MonoBehaviour 
{
	public string errorKey = "Error";
	public string enterAccountKey = "EnterAccount";
	public string enterPasswordKey = "EnterPassword";
	public string loggingKey = "Logging";
	public string loginSuccessKey = "LoginSuccess";
	public string loginFailKey = "LoginFail";
	public string creatingAccountKey = "CreatingAccount";
	public string createAccountSuccessKey = "CreateAccountSuccess";
	public string createAccountFailKey = "CreateAccountFail";
	public string syncDataKey = "SyncData";
	public string syncDataSuccessKey = "SyncDataSuccess";
	public string syncDataFailKey = "SyncDataFail";
	public string noInternet = "NoInternet";

	/// <summary>
	/// The account input.
	/// </summary>
	public UIInput accountInput;

	/// <summary>
	/// The password inpu.
	/// </summary>
	public UIInput passwordInput;

	/// <summary>
	/// The login button.
	/// </summary>
	public UIButton loginButton;

	/// <summary>
	/// The status label.
	/// </summary>
	public UILabel statusLabel;

	/// <summary>
	/// The create account button.
	/// </summary>
	public UIButton createAccountButton;

	/// <summary>
	/// The alert control.
	/// </summary>
	public UIAlertControl alertControl;

	// Use this for initialization
	void Start () 
	{
		alertControl.CloseAlertWindow ();

		statusLabel.text = "";

		UserProfile up = UserProfile.Load ();

		if (!string.IsNullOrEmpty (up.userName))
		{
			accountInput.value = up.userName;
		}
		else
		{
			accountInput.value = "";
		}

		if(!string.IsNullOrEmpty(up.password))
		{
			passwordInput.value = up.password;
		}
		else
		{
			passwordInput.value = "";
		}

	}

	void OnEnable()
	{
		alertControl.Evt_CloseAlertWindow += OnAlertWindowClose;

		ServerSync.SharedInstance.Evt_OnInternetNotAvailable += OnInternetNotAvailable;
		ServerSync.SharedInstance.Evt_OnCreateAccountSuccess += OnCreateAccountSuccess;
		ServerSync.SharedInstance.Evt_OnCreateAccountFail += OnCreateAccountFail;
		ServerSync.SharedInstance.Evt_OnLoginSuccess += OnLoginSuccess;
		ServerSync.SharedInstance.Evt_OnLoginFail += OnLoginFail;
	}

	void OnDisable()
	{
		if(alertControl)
			alertControl.Evt_CloseAlertWindow -= OnAlertWindowClose;

		if(ServerSync.SharedInstance)
		{
			ServerSync.SharedInstance.Evt_OnInternetNotAvailable -= OnInternetNotAvailable;
			ServerSync.SharedInstance.Evt_OnCreateAccountSuccess -= OnCreateAccountSuccess;
			ServerSync.SharedInstance.Evt_OnCreateAccountFail -= OnCreateAccountFail;
			ServerSync.SharedInstance.Evt_OnLoginSuccess -= OnLoginSuccess;
			ServerSync.SharedInstance.Evt_OnLoginFail -= OnLoginFail;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Login()
	{
		if (string.IsNullOrEmpty (accountInput.value)) 
		{
			alertControl.ShowAlertWindow(errorKey, enterAccountKey);

			return;
		}

		if(string.IsNullOrEmpty(passwordInput.value))
		{
			alertControl.ShowAlertWindow(errorKey, enterPasswordKey);

			return;
		}

		LockUI ();

		statusLabel.text = Localization.Get (loggingKey);

		ServerSync.SharedInstance.LoginServer (accountInput.value, passwordInput.value);
	}

	public void CreateAccount()
	{
		statusLabel.text = Localization.Get (creatingAccountKey);

		LockUI ();

		ServerSync.SharedInstance.CreateAccount ();
	}

	private void SyncData()
	{
		statusLabel.text = Localization.Get (syncDataKey);

		GoToMainMenu ();
	}

	private void LockUI()
	{
		accountInput.GetComponent<BoxCollider> ().enabled = false;
		passwordInput.GetComponent<BoxCollider> ().enabled = false;
		loginButton.isEnabled = false;
		createAccountButton.isEnabled = false;
	}

	private void UnlockUI()
	{
		accountInput.GetComponent<BoxCollider> ().enabled = true;
		passwordInput.GetComponent<BoxCollider> ().enabled = true;
		loginButton.isEnabled = true;
		createAccountButton.isEnabled = true;
	}

	private void GoToMainMenu()
	{
		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel ("MainMenu");
	}

	#region UIAlertWindow callback
	void OnAlertWindowClose(UIAlertControl control)
	{
		statusLabel.text = "";

		UnlockUI ();
	}
	#endregion UIAlertWindow callback

	#region ServerSync callback
	void OnInternetNotAvailable(ServerSync syncControl)
	{
		statusLabel.text = "";

		alertControl.ShowAlertWindow (errorKey, noInternet);
	}

	void OnCreateAccountSuccess(ServerSync syncControl, string username, string password)
	{
		//save account password
		UserProfile up = UserProfile.Load ();

		up.userName = username;
		up.password = password;

		UserProfile.Save (up);

		//set input value
		accountInput.value = username;
		passwordInput.value = password;

		statusLabel.text = Localization.Get (createAccountSuccessKey);

		//logging server
		Invoke ("Login", 1f);
	}

	void OnCreateAccountFail(ServerSync syncControl, int errorCode)
	{
		alertControl.ShowAlertWindow (errorKey, createAccountFailKey);
	}

	void OnLoginSuccess(ServerSync syncControl, string uid)
	{
		//save uid
		UserProfile up = UserProfile.Load ();

		up.uid = uid;

		UserProfile.Save (up);

		UnlockUI ();

		statusLabel.text = Localization.Get (loginSuccessKey);

		//sync data
		Invoke ("SyncData", 1f);
	}

	void OnLoginFail(ServerSync syncControl, int errorCode)
	{
		alertControl.ShowAlertWindow (errorKey, loginFailKey);
	}
	#endregion ServerSync callback
}
