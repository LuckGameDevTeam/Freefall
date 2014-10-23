using UnityEngine;
using System.Collections;

/// <summary>
/// Tarzan cat ability.
/// 
/// This class is subclass of Ability
/// </summary>
public class TarzanCatAbility : Ability 
{
	/// <summary>
	/// The ability clip.
	/// </summary>
	public AudioClip abilityClip;

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		//find all obstacle from scene
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (Tags.obstacle);
		
		for(int i=0; i<obstacles.Length; i++)
		{
			Obstacle o = obstacles[i].GetComponent<Obstacle>();
			
			//if obstacle is in screen
			if(o.IsVisibleFromCamera(Camera.main))
			{
				//if it is type of monster medium...make it become small obstacle
				if(o.monsterType == MonsterTypes.MonsterMedium)
				{
					o.BecomeSmallObstacle();
				}
			}
			
		}

		//play ability clip
		if(abilityClip != null)
		{
			AudioSource.PlayClipAtPoint(abilityClip, transform.position);
		}
		
		//remove ability
		RemoveAbility ();
	}
	
	protected override void RemoveAbility()
	{
		base.RemoveAbility ();
		
	}
	
	public override void RemoveAbilityImmediately ()
	{
		base.RemoveAbilityImmediately ();
		
	}
	
	protected override void ProcessAbility()
	{
		base.ProcessAbility ();
	}
	
	protected override void EffectFinished()
	{
		base.EffectFinished ();
		
		RemoveAbility ();
	}
}
