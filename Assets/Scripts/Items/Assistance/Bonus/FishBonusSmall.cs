using UnityEngine;
using System.Collections;

/// <summary>
/// Fish bonus small.
/// 
/// This class is for Small Fish Bone
/// </summary>
public class FishBonusSmall : Bonus 
{
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

			base.OnTriggerEnter2D(other);
			
		}
	}
}
