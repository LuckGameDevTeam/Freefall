using UnityEngine;
using System.Collections;

/// <summary>
/// Effect loop animation.
/// 
/// This is subclass of EffectAnimation
/// 
/// This class is designed for effect animation that will keep looping once
/// been played.
/// </summary>
public class EffectLoopAnimation : EffectAnimation 
{

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
		gameObject.SetActive (true);

		base.PlayAnimation ();
	}
	
	public override void StopAnimation()
	{
		base.StopAnimation ();

		gameObject.SetActive (false);
	}

}
