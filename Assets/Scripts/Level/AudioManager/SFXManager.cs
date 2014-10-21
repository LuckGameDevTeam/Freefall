using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour 
{
	/// <summary>
	/// The mute.
	/// </summary>
	private bool mute = false;

	void Awake()
	{

	}

	// Use this for initialization
	void Start () 
	{
		AudioSetting audioSetting = AudioSetting.Load ();
		
		mute = audioSetting.soundFXMute;


	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public bool Mute
	{
		get
		{
			return mute;
		}

		set
		{
			Debug.Log("sfx save");

			Debug.Log(UnityEngine.StackTraceUtility.ExtractStackTrace ());
			mute = value;
			
			AudioSetting audioSetting = AudioSetting.Load();
			
			audioSetting.soundFXMute = mute;
			
			AudioSetting.Save(audioSetting);
		}
	}
}
