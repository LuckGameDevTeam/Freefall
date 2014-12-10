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

	/// <summary>
	/// The alert control.
	/// </summary>
	public UIAlertControl alertControl;


	// Use this for initialization
	void Start () 
	{
	
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

		gameObject.SetActive (false);
	}

	public void ChangePassword()
	{
		if(!string.IsNullOrEmpty(newPasswordInput.value))
		{
			//todo change password
		}
		else
		{
			alertControl.ShowAlertWindow(errorKey, enterNewPasswordKey);
		}
	}
}
