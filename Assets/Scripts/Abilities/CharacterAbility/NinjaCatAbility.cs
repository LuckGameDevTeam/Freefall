using UnityEngine;
using System.Collections;

public class NinjaCatAbility : Ability 
{
	/// <summary>
	/// The ninja cat clip.
	/// </summary>
	public AudioClip ninjaCatClip;

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		//disable character's collision component
		character.GetComponent<CharacterControl> ().CollisionSetting (false);

		//do not render character
		character.renderer.enabled = false;

		//play ninja cat clip
		/*
		if(ninjaCatClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.sfxClip = ninjaCatClip;
				soundPlayer.PlaySound();
			}
		}
		else
		{
			Debug.LogError(gameObject.name+" unable to play ninaj cat clip, ninja cat clip not assigned");
		}
		*/
		if(ninjaCatClip != null)
		{
			AudioSource.PlayClipAtPoint(ninjaCatClip, transform.position);
		}
		else
		{
			Debug.LogError(gameObject.name+" unable to play ninaj cat clip, ninja cat clip not assigned");
		}
	}

	protected override void RemoveAbility()
	{
		//enable character's collision component
		character.GetComponent<CharacterControl> ().CollisionSetting (true);
		
		//render character
		character.renderer.enabled = true;

		/*
		//stop sound
		if(ninjaCatClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}
		*/

		base.RemoveAbility ();
		
	}
	
	public override void RemoveAbilityImmediately ()
	{
		//enable character's collision component
		character.GetComponent<CharacterControl> ().CollisionSetting (true);
		
		//render character
		character.renderer.enabled = true;

		/*
		//stop sound
		if(ninjaCatClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}
		*/

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
