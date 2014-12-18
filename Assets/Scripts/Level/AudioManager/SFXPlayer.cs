using UnityEngine;
using System.Collections;

public class SFXPlayer : MonoBehaviour 
{
	/// <summary>
	/// The sfx clip.
	/// </summary>
	public AudioClip sfxClip;

	/// <summary>
	/// The volume.
	/// </summary>
	public float volume = 1f;
	
	/// <summary>
	/// The priority.
	/// </summary>
	[Range(0, 255)]
	public int priority = 128;

	/// <summary>
	/// The loop.
	/// 
	/// It is not recommand to loop on SFX
	/// </summary>
	public bool loop = false;
	
	/// <summary>
	/// Play music on start
	/// </summary>
	public bool playOnStart = false;

	/// <summary>
	/// The ignore time scale.
	/// 
	/// true audio playback will not depend on time scale, otherwise false
	/// </summary>
	public bool ignoreTimeScale = false;

	/// <summary>
	/// Reference to SFXManager
	/// </summary>
	private SFXManager sfxMgr;

	/// <summary>
	/// The audio source.
	/// </summary>
	private AudioSource audioSource;

	void Awake()
	{
		if(GetComponent<AudioSource>() == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}

		sfxMgr = GameObject.FindObjectOfType (typeof(SFXManager)) as SFXManager;
	}

	// Use this for initialization
	void Start () 
	{
		
		if(playOnStart)
		{
			PlaySound();
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

	/// <summary>
	/// Plaies the sound.
	/// </summary>
	public void PlaySound()
	{
		if(GetComponent<AudioSource>() == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}

		if(sfxClip != null)
		{
			if(audio.isPlaying)
			{
				audio.Stop();
			}
			
			audio.clip = sfxClip;
			audio.loop = loop;
			audio.playOnAwake = false;
			audio.volume = volume;
			audio.priority = priority;
			audio.mute = sfxMgr.Mute;
			audio.Play();
		}
		else
		{
			DebugEx.DebugError("Unable to play sound, no sound audio clip assigned");
		}
	}

	/// <summary>
	/// Stops the sound.
	/// </summary>
	public void StopSound()
	{
		if(audio.isPlaying)
		{
			audio.Stop();
		}
	}

	public bool IsPlaying
	{
		get
		{
			return audioSource.isPlaying;
		}
	}
}
