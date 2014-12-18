using UnityEngine;
using System.Collections;

/// <summary>
/// Cat crown ability.
/// This class is subclass of Ability.
/// This Ability make character become bigger and invulnerable for 3 seconds
/// </summary>
public class CatCrownAbility : Ability 
{
	/// <summary>
	/// How big is character is going to be
	/// </summary>
	public float characterSize = 2f;

	/// <summary>
	/// The cat crown clip.
	/// </summary>
	public AudioClip catCrownClip;

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		//increase size of character
		character.transform.localScale = new Vector3 (characterSize, characterSize, 1f);

		//make character invulnerable
		character.GetComponent<CharacterHealth> ().invulnerable = true;

		//play cat crown clip
		if(catCrownClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.sfxClip = catCrownClip;
				soundPlayer.loop = true;
				soundPlayer.PlaySound();
			}
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" unable to play cat crown clip, cat crown clip not assigned");
		}
	}

	protected override void RemoveAbility()
	{
		base.RemoveAbility ();

		//set character size to normal
		character.transform.localScale = new Vector3 (1f, 1f, 1f);

		//make character vulnerable again
		character.GetComponent<CharacterHealth> ().invulnerable = false;

		//stop sound
		if(catCrownClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}
	}

	public override void RemoveAbilityImmediately ()
	{
		base.RemoveAbilityImmediately ();

		//set character size to normal
		character.transform.localScale = new Vector3 (1f, 1f, 1f);

		//make character vulnerable again
		character.GetComponent<CharacterHealth> ().invulnerable = false;

		//stop sound
		if(catCrownClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}
	}

	protected override void ProcessAbility()
	{
		base.ProcessAbility ();
	}
}
