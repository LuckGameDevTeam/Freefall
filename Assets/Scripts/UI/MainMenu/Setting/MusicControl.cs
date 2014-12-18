using UnityEngine;
using System.Collections;

public class MusicControl : MonoBehaviour 
{
	public UIToggle checkBoxOn;
	public UIToggle checkBoxOff;

	bool isSet = false;

	// Use this for initialization
	void Start () 
	{


		AudioSetting audioSetting = AudioSetting.Load ();

		if(audioSetting.backgroundMusicMute)
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
			//checkBoxOn.functionName = "TurnOnMusic";

			//NGUI 3.x.x
			EventDelegate.Set(checkBoxOn.onChange, TurnOnMusic);

			//NGUI 2.7
			//checkBoxOff.eventReceiver = gameObject;
			//checkBoxOff.functionName = "TurnOffMusic";

			//NGUI 3.x.x
			EventDelegate.Set(checkBoxOff.onChange, TurnOffMusic);


		}
	}

	//NGUI 3.x.x
	public void TurnOnMusic()
	{
		(GameObject.FindObjectOfType (typeof(MusicManager)) as MusicManager).Mute = false;
	}

	//NGUI 3.x.x
	public void TurnOffMusic()
	{
		(GameObject.FindObjectOfType (typeof(MusicManager)) as MusicManager).Mute = true;
	}
}
