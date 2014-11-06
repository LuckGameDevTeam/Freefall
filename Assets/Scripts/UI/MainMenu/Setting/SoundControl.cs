using UnityEngine;
using System.Collections;

public class SoundControl : MonoBehaviour 
{
	public UIToggle checkBoxOn;
	public UIToggle checkBoxOff;

	bool isSet = false;

	// Use this for initialization
	void Start () 
	{
		AudioSetting audioSetting = AudioSetting.Load ();
		
		if(audioSetting.soundFXMute)
		{
			//NGUI 2.7
			//checkBoxOn.isChecked = false;
			//checkBoxOff.isChecked = true;

			//NGUI 3.x.x
			checkBoxOn.value = false;
			checkBoxOff.value = true;

		}
		else
		{
			//NGUI 2.7
			//checkBoxOn.isChecked = true;
			//checkBoxOff.isChecked = false;

			//NGUI 3.x.x
			checkBoxOn.value = true;
			checkBoxOff.value = false;

		}


	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!isSet)
		{
			//NGUI 2.7
			//checkBoxOn.eventReceiver = gameObject;
			//checkBoxOn.functionName = "TurnOnSound";

			//NGUI 3.x.x
			EventDelegate.Set(checkBoxOn.onChange, TurnOnSound);

			//NGUI 2.7
			//checkBoxOff.eventReceiver = gameObject;
			//checkBoxOff.functionName = "TurnOffSound";

			//NGUI 3.x.x
			EventDelegate.Set(checkBoxOff.onChange, TurnOffSound);
		}
	}

	//NGUI 2.7
	/*
	public void TurnOnSound(bool mChecked)
	{
		(GameObject.FindObjectOfType (typeof(SFXManager)) as SFXManager).Mute = false;

	}
	*/

	public void TurnOnSound()
	{
		(GameObject.FindObjectOfType (typeof(SFXManager)) as SFXManager).Mute = false;
		
	}

	//NGUI 2.7
	/*
	public void TurnOffSound(bool mChecked)
	{
		(GameObject.FindObjectOfType (typeof(SFXManager)) as SFXManager).Mute = true;

	}
	*/

	public void TurnOffSound()
	{
		(GameObject.FindObjectOfType (typeof(SFXManager)) as SFXManager).Mute = true;
		
	}
}
