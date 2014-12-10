using UnityEngine;
using System.Collections;

public class UISettingControl : MonoBehaviour 
{
	public delegate void EventOnSettingClose(UISettingControl control);
	/// <summary>
	/// The evt_ on setting close.
	/// </summary>
	public EventOnSettingClose Evt_OnSettingClose;

	public GameObject settingMenu;

	public UIPlayerProfile playerProfile;

	// Use this for initialization
	void Start () 
	{
		settingMenu.SetActive (true);
		playerProfile.gameObject.SetActive (false);
	}

	void OnEnable()
	{
		playerProfile.Evt_OnPlayerProfileClose += OnPlayerProfileClose;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowSetting()
	{
		gameObject.SetActive (true);

		settingMenu.SetActive (true);
		playerProfile.gameObject.SetActive (false);
	}

	public void CloseSetting()
	{
		playerProfile.gameObject.SetActive (false);

		gameObject.SetActive (false);

		if(Evt_OnSettingClose != null)
		{
			Evt_OnSettingClose(this);
		}
	}

	public void ShowPlayerProfile()
	{
		settingMenu.SetActive (false);

		playerProfile.GetComponent<UIPlayerProfile> ().ShowPlayerProfile ();
	}

	void OnPlayerProfileClose(UIPlayerProfile profile)
	{
		settingMenu.SetActive (true);
	}
}
