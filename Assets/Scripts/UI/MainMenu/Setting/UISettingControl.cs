using UnityEngine;
using System.Collections;

public class UISettingControl : MonoBehaviour 
{
	public delegate void EventOnSettingClose(UISettingControl control);
	public EventOnSettingClose Evt_OnSettingClose;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowSetting()
	{
		gameObject.SetActive (true);
	}

	public void CloseSetting()
	{
		gameObject.SetActive (false);

		if(Evt_OnSettingClose != null)
		{
			Evt_OnSettingClose(this);
		}
	}
}
