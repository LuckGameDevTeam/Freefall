using UnityEngine;
using System.Collections;

/// <summary>
/// Cat sword ability.
/// This class is subclass of Ability.
/// This ability destroy all obstacles in scene except boss
/// </summary>
public class CatSwordAbility : Ability 
{
	/// <summary>
	/// The cat sword clip.
	/// </summary>
	public AudioClip catSwordClip;

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (Tags.obstacle);

		for(int i=0; i<obstacles.Length; i++)
		{
			Obstacle o = obstacles[i].GetComponent<Obstacle>();

			//if monster object is in camera view
			if(o.IsVisibleFromCamera(Camera.main))
			{
				//if monster is not boss make it dead
				if(o.monsterType != MonsterTypes.Boss)
				{
					o.isDead = true;
				}
			}

		}

		//play cat sword clip
		if(catSwordClip != null)
		{
			AudioSource.PlayClipAtPoint(catSwordClip, transform.position);
		}
		else
		{
			Debug.LogError(gameObject.name+"unable to play cat sword clip, cat sword clip not assigned");
		}
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

		//since ability effect play once but still have time
		//so we listen to effect finished event then remove ability
		//result in effect is still visible without destroy immediately
		RemoveAbility ();
	}
}
