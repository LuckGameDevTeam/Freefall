using UnityEngine;
using System.Collections;

/// <summary>
/// Coin bonus small.
/// 
/// This class is for Small Cat Coine
/// </summary>
public class CoinBonusSmall : Bonus 
{


	protected override void Awake()
	{
		base.Awake ();
	}
	
	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Tags.player)
		{

			GameController.sharedGameController.CoinCount += bonus;

			//tell character play coin eaten effect
			GameController.sharedGameController.character.GetComponent<CharacterEffect>().PlayCoinEatenEffect();
			
			base.OnTriggerEnter2D(other);
			
		}
	}

}
