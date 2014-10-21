using UnityEngine;
using System.Collections;

/// <summary>
/// Effect animation.
/// 
/// This is generic class ofr all effect animation
/// </summary>
public class EffectAnimation : MonoBehaviour 
{
	/// <summary>
	/// Event for animation end
	/// </summary>
	/// <returns>The animation finished.</returns>
	public delegate void EventAnimationFinished();
	public EventAnimationFinished Evt_AnimationFinished;

	/// <summary>
	/// Reference to Animator
	/// </summary>
	protected Animator animator;

	/// <summary>
	/// Hash id for parameter in animator
	/// Common use for effect.
	/// Each effect animator must have a boolean type of "Play" parameter
	/// </summary>
	protected int play_bool;



    protected virtual void Awake()
	{
		//find animator
		animator = GetComponent<Animator> ();

		//convert animation's parameters to hash id
		play_bool = Animator.StringToHash ("Play");
	}

	protected virtual void Start ()
	{

	}

	public virtual void PlayAnimation()
	{
		animator.SetBool (play_bool, true);
	}

	public virtual void StopAnimation()
	{

		if(gameObject.activeInHierarchy)
		{
			animator.SetBool (play_bool, false);
		}
	}

	/// <summary>
	/// Callback when animation end.
	/// Animation can/can not setup a callback.
	/// This is design for animation that one play once
	/// </summary>
	protected virtual void AnimationEnd()
	{
		if(gameObject.activeInHierarchy)
		{
			animator.SetBool (play_bool, false);
		}


		if(Evt_AnimationFinished != null)
		{
			Evt_AnimationFinished();
		}
	}
	
}
