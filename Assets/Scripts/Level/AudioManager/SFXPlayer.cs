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
	/// Reference to SFXManager
	/// </summary>
	private SFXManager sfxMgr;

	void Awake()
	{
		if(GetComponent<AudioSource>() == null)
		{
			gameObject.AddComponent<AudioSource>();
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
		audio.pitch = Time.timeScale;
	}

	/// <summary>
	/// Plaies the sound.
	/// </summary>
	public void PlaySound()
	{
		if(GetComponent<AudioSource>() == null)
		{
			gameObject.AddComponent<AudioSource>();
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
			Debug.LogError("Unable to play sound, no sound audio clip assigned");
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
}
