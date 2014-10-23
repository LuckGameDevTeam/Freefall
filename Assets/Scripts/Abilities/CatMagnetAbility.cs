using UnityEngine;
using System.Collections;

/// <summary>
/// Cat magnet ability.
/// This class is subclass of Ability.
/// This ability make all coin and fish bone magnetable to character
/// for 5 seconds
/// </summary>
public class CatMagnetAbility : Ability 
{
	/// <summary>
	/// The cat magnet clip.
	/// </summary>
	public AudioClip catMagnetClip;

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		//play cat magnet clip
		if(catMagnetClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.sfxClip = catMagnetClip;
				soundPlayer.loop = true;
				soundPlayer.PlaySound();
			}
		}
		else
		{
			Debug.LogError(gameObject.name+" unable to play cat magnet clip, cat magnet clip not assigned");
		}
	}
	
	protected override void RemoveAbility()
	{
		base.RemoveAbility ();

		//stop sound
		if(catMagnetClip != null)
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

		//stop sound
		if(catMagnetClip != null)
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

		//find all assisant item holder from scene
		GameObject[] assistantItemHolders = GameObject.FindGameObjectsWithTag (Tags.assistantItemHolder);

		if(assistantItemHolders.Length > 0)
		{
			for(int i=0; i<assistantItemHolders.Length; i++)
			{
				//tell assistant item holder's child object to become magnetable
				assistantItemHolders[i].GetComponent<AssistantItemHolder>().isMagnet = true;
			}
		}


	}
}
