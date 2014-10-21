using UnityEngine;
using System.Collections;

/// <summary>
/// Effect once animation.
/// 
/// This is subclass of EffectAnimation
/// 
/// This class is designed for effect animation thatn only
/// play once.
/// </summary>
public class EffectOnceAnimation : EffectAnimation 
{
	/// <summary>
	/// The play count.
	/// </summary>
	protected int playCount = 0;

	protected override void Awake()
	{
		base.Awake ();
	}
	
	protected override void Start ()
	{
		base.Start ();

		//gameObject.SetActive (false);
	}
	
	public override void PlayAnimation()
	{
		playCount++;
		
		gameObject.SetActive (true);
		
		base.PlayAnimation ();
	}
	
	public override void StopAnimation()
	{
		base.StopAnimation ();

		playCount = 0;
		
		gameObject.SetActive (false);
	}

	protected override void AnimationEnd()
	{
		playCount--;
		
		if(playCount <= 0)
		{
			base.AnimationEnd();

			gameObject.SetActive (false);
			
			playCount = 0;
		}
		
	}

	/// <summary>
	/// For animation callback from animation editor
	/// </summary>
	void AnimationFinished()
	{
		AnimationEnd ();
	}
}
