using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AudioSetting : PersistantMetaData 
{
	/// <summary>
	/// The background music mute.
	/// </summary>
	public bool backgroundMusicMute = false;

	/// <summary>
	/// The sound FX mute.
	/// </summary>
	public bool soundFXMute = false;

	public static AudioSetting Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<AudioSetting>())
		{
			//create new one
			AudioSetting newFile = new AudioSetting();
			
			SaveLoadManager.SharedManager.Save(newFile);
			
			return newFile;
		}
		
		return SaveLoadManager.SharedManager.Load<AudioSetting> ();
	}
	
	public static bool Save(AudioSetting data)
	{
		return SaveLoadManager.SharedManager.Save (data);
	}
}
