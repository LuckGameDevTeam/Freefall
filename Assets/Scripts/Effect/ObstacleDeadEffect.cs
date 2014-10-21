using UnityEngine;
using System.Collections;

/// <summary>
/// Obstacle dead effect.
/// 
/// This is subclass of EffectOnceAnimation
/// 
/// This class is special design for effect that when obstacle dead.
/// 
/// This is for optmization purpose, so effect can be resued.
/// </summary>
public class ObstacleDeadEffect : EffectOnceAnimation 
{
	//private GameController gameController;

	protected override void Awake()
	{
		base.Awake ();

		//gameController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<GameController> ();
	}

	protected override void AnimationEnd()
	{
		//recycle effect
		GameController.sharedGameController.objectPool.RecycleObject (gameObject);
	}

	/// <summary>
	/// For animation callback from animation editor
	/// </summary>
	void AnimationFinished()
	{
		AnimationEnd ();
	}
}
