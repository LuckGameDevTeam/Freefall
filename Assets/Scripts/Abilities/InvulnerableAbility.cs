using UnityEngine;
using System.Collections;

/// <summary>
/// Invulnerable ability.
/// 
/// This class is subclass of Ability.
/// This ability allow character to become invulnerable for few seconds
/// </summary>
public class InvulnerableAbility : Ability 
{
	/// <summary>
	/// The invulnerable clip.
	/// </summary>
	public AudioClip invulnerableClip;

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		//set character invulnerable
		character.GetComponent<CharacterHealth> ().invulnerable = true;

		//play invulnerable clip
		if(invulnerableClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.sfxClip = invulnerableClip;
				soundPlayer.PlaySound();
			}
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" unable to play invulnerable clip, invulnerable clip not assigned");
		}
	}
	
	protected override void RemoveAbility()
	{
		//set character vulnerable
		character.GetComponent<CharacterHealth> ().invulnerable = false;

		//stop sound
		if(invulnerableClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}

		base.RemoveAbility ();
	}

	public override void RemoveAbilityImmediately()
	{
		//set character vulnerable
		character.GetComponent<CharacterHealth> ().invulnerable = false;

		//stop sound
		if(invulnerableClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}

		base.RemoveAbilityImmediately ();
	}
	
	protected override void ProcessAbility()
	{
		base.ProcessAbility ();
	}
}
