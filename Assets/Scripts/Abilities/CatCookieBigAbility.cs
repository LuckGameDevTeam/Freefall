using UnityEngine;
using System.Collections;

/// <summary>
/// Cat cookie big ability.
/// 
/// This class is subclass of Ability
/// </summary>
public class CatCookieBigAbility : Ability 
{
	/// <summary>
	/// How many health will add to ability owner.
	/// </summary>
	public float addHealth = 3f;

	/// <summary>
	/// The cat cookie clip.
	/// </summary>
	public AudioClip catCookieClip;

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		//add health to owner 
		character.GetComponent<CharacterHealth> ().AddHP(addHealth);

		//play cat cookie clip
		if(catCookieClip != null)
		{
			AudioSource.PlayClipAtPoint(catCookieClip, transform.position);
		}
		else
		{
			DebugEx.DebugError(gameObject.name+"unable to play cat cookie clip, cat cookie clip not assigned");
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
