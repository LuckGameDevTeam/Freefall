using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour 
{
	/// <summary>
	/// The music clip.
	/// </summary>
	public AudioClip musicClip = null;

	/// <summary>
	/// The volume.
	/// </summary>
	public float volume = 0.3f;

	/// <summary>
	/// The priority.
	/// </summary>
	[Range(0, 255)]
	public int priority = 128;

	/// <summary>
	/// The loop.
	/// </summary>
	public bool loop = true;

	/// <summary>
	/// Play music on start
	/// </summary>
	public bool playOnStart = true;

	/// <summary>
	/// The ignore time scale.
	/// 
	/// true audio playback will not depend on time scale, otherwise false
	/// </summary>
	public bool ignoreTimeScale = false;

	/// <summary>
	/// The mute.
	/// </summary>
	private bool mute = false;

	void Awake()
	{
		if(GetComponent<AudioSource>() == null)
		{
			gameObject.AddComponent<AudioSource>();
		}
	}

	void OnDisable()
	{
		if(audio != null)
		{
			if(audio.isPlaying)
			{
				audio.Stop();
			}
		}

	}

	// Use this for initialization
	void Start () 
	{
		AudioSetting audioSetting = AudioSetting.Load ();

		mute = audioSetting.backgroundMusicMute;

		if(playOnStart)
		{
			PlayMusic();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(ignoreTimeScale)
		{
			audio.pitch = 1.0f;
		}
		else
		{
			audio.pitch = Time.timeScale;
		}

	}

	public void PlayMusic()
	{
		if(GetComponent<AudioSource>() == null)
		{
			gameObject.AddComponent<AudioSource>();
		}

		if(musicClip != null)
		{
			if(audio.isPlaying)
			{
				audio.Stop();
			}

			audio.clip = musicClip;
			audio.loop = loop;
			audio.playOnAwake = false;
			audio.volume = volume;
			audio.priority = priority;
			audio.mute = mute;
			audio.Play();
		}
		else
		{
			Debug.LogError("Unable to play music, no audio clip assigned");
		}
	}

	public void StopMusic()
	{
		if(audio.isPlaying)
		{
			audio.Stop();
		}
	}

	public bool Mute
	{
		get
		{
			return mute;
		}

		set
		{
			mute = value;

			audio.mute = mute;

			AudioSetting audioSetting = AudioSetting.Load();

			audioSetting.backgroundMusicMute = mute;

			AudioSetting.Save(audioSetting);
		}
	}
}
