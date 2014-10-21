using UnityEngine;
using System.Collections;

public class SoundControl : MonoBehaviour 
{
	public UICheckbox checkBoxOn;
	public UICheckbox checkBoxOff;

	bool isSet = false;

	// Use this for initialization
	void Start () 
	{
		AudioSetting audioSetting = AudioSetting.Load ();
		
		if(audioSetting.soundFXMute)
		{
			checkBoxOn.isChecked = false;
			checkBoxOff.isChecked = true;

		}
		else
		{
			checkBoxOn.isChecked = true;
			checkBoxOff.isChecked = false;

		}


	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!isSet)
		{
			checkBoxOn.eventReceiver = gameObject;
			checkBoxOn.functionName = "TurnOnSound";
			
			checkBoxOff.eventReceiver = gameObject;
			checkBoxOff.functionName = "TurnOffSound";
		}
	}

	public void TurnOnSound(bool mChecked)
	{
		(GameObject.FindObjectOfType (typeof(SFXManager)) as SFXManager).Mute = false;

	}
	
	public void TurnOffSound(bool mChecked)
	{
		(GameObject.FindObjectOfType (typeof(SFXManager)) as SFXManager).Mute = true;

	}
}
