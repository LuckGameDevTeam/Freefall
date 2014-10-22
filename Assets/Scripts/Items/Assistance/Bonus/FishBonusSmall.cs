using UnityEngine;
using System.Collections;

/// <summary>
/// Fish bonus small.
/// 
/// This class is for Small Fish Bone
/// </summary>
public class FishBonusSmall : Bonus 
{
	public AudioClip eatFishBoneClip;

	protected override void Awake()
	{
		base.Awake ();
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Tags.player)
		{
			GameController.sharedGameController.StarCount += bonus;

			//tell character play fish bone eaten effect
			GameController.sharedGameController.character.GetComponent<CharacterEffect>().PlayFishBoneEatenEffect();

			//play eat fish bone sound 
			if(eatFishBoneClip != null)
			{
				AudioSource.PlayClipAtPoint(eatFishBoneClip, transform.position);
			}
			else
			{
				Debug.LogError(gameObject.name+" unable to play eat fish bone clip, eat fish bone clip not assign");
			}

			base.OnTriggerEnter2D(other);
			
		}
	}
}
