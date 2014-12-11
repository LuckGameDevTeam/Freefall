using UnityEngine;
using System.Collections;

public class UIPlayerProfile : MonoBehaviour 
{
	public string errorKey = "Error";
	public string enterNewPasswordKey = "EnterNewPassword";
	public string changePasswordFailKey = "ChangePasswordFail";

	public delegate void OnPlayerProfileClose(UIPlayerProfile profiel);
	/// <summary>
	/// Event when player profile close.
	/// </summary>
	public OnPlayerProfileClose Evt_OnPlayerProfileClose;

	public UIInput accountInput;

	public UIInput passwordInput;

	/// <summary>
	/// The new password input.
	/// </summary>
	public UIInput newPasswordInput;

	public UIButton changePassBtn;

	/// <summary>
	/// The alert control.
	/// </summary>
	public UIAlertControl alertControl;


	// Use this for initialization
	void Start () 
	{
	
	}

	void OnEnable()
	{
		ServerSync.SharedInstance.Evt_OnChangePasswordSuccess += OnPasswordChangeSuccess;
		ServerSync.SharedInstance.Evt_OnChangePasswordFail += OnPasswordChangeFail;
		ServerSync.SharedInstance.Evt_OnOtherDeviceLogin += OnLogginFromOtherDevice;
	}

	void OnDisable()
	{
		ServerSync.SharedInstance.Evt_OnChangePasswordSuccess -= OnPasswordChangeSuccess;
		ServerSync.SharedInstance.Evt_OnChangePasswordFail -= OnPasswordChangeFail;
		ServerSync.SharedInstance.Evt_OnOtherDeviceLogin -= OnLogginFromOtherDevice;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowPlayerProfile()
	{
		gameObject.SetActive (true);

		UserProfile up = UserProfile.Load ();

		accountInput.value = up.userName;
		passwordInput.value = up.password;
	}

	public void ClosePlayerProfile()
	{
		if(Evt_OnPlayerProfileClose != null)
		{
			Evt_OnPlayerProfileClose(this);
		}

		newPasswordInput.value = "";

		gameObject.SetActive (false);
	}

	public void ChangePassword()
	{
		if(!string.IsNullOrEmpty(newPasswordInput.value))
		{
			changePassBtn.isEnabled = false;

			//change password
			ServerSync.SharedInstance.ChangePassword(newPasswordInput.value);
		}
		else
		{
			alertControl.ShowAlertWindow(errorKey, enterNewPasswordKey);
		}
	}

	#region ServerSync callback
	void OnPasswordChangeSuccess(ServerSync syncControl, string newPassword)
	{
		changePassBtn.isEnabled = true;

		UserProfile up = UserProfile.Load ();

		up.password = newPassword;

		UserProfile.Save (up);

		ClosePlayerProfile ();
	}

	void OnPasswordChangeFail(ServerSync syncControl, int errorCode)
	{
		changePassBtn.isEnabled = true;

		alertControl.ShowAlertWindow (errorKey, changePasswordFailKey);
	}

	void OnLogginFromOtherDevice(ServerSync syncControl, int errorCode)
	{
		//go back to login scene
		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel ("LoginScene");
	}
	#endregion ServerSync callback
}
