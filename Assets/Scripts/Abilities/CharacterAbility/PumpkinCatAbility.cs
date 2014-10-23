using UnityEngine;
using System.Collections;

/// <summary>
/// Pumpkin cat ability.
/// 
/// This class is subclass of CatStickAbility
/// The ability of this character is as same as CatStickAbility
/// </summary>
public class PumpkinCatAbility : CatStickAbility 
{
	/// <summary>
	/// The ability clip.
	/// </summary>
	public AudioClip abilityClip;

	public override void ActiveAbility(GameObject owner)
	{
		if(abilityClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.sfxClip = abilityClip;
				soundPlayer.PlaySound();
			}
		}

		base.ActiveAbility (owner);
	}

	protected override void RemoveAbility()
	{
		if(abilityClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}
		
		base.RemoveAbility ();
		
	}
	
	public override void RemoveAbilityImmediately ()
	{
		if(abilityClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}
		
		base.RemoveAbilityImmediately ();
	}
}
