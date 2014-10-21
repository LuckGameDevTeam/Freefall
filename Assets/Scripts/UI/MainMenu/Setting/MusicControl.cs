using UnityEngine;
using System.Collections;

public class MusicControl : MonoBehaviour 
{
	public UICheckbox checkBoxOn;
	public UICheckbox checkBoxOff;

	bool isSet = false;

	// Use this for initialization
	void Start () 
	{


		AudioSetting audioSetting = AudioSetting.Load ();

		if(audioSetting.backgroundMusicMute)
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
			checkBoxOn.functionName = "TurnOnMusic";
			
			checkBoxOff.eventReceiver = gameObject;
			checkBoxOff.functionName = "TurnOffMusic";
		}
	}

	public void TurnOnMusic()
	{
		(GameObject.FindObjectOfType (typeof(MusicManager)) as MusicManager).Mute = false;
	}

	public void TurnOffMusic()
	{
		(GameObject.FindObjectOfType (typeof(MusicManager)) as MusicManager).Mute = true;
	}
}
