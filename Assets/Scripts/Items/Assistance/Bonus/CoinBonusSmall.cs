using UnityEngine;
using System.Collections;

/// <summary>
/// Coin bonus small.
/// 
/// This class is for Small Cat Coine
/// </summary>
public class CoinBonusSmall : Bonus 
{
	public AudioClip eatCoinClip;

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

			//play eat coin sound 
			if(eatCoinClip != null)
			{
				AudioSource.PlayClipAtPoint(eatCoinClip, transform.position);
			}
			else
			{
				DebugEx.DebugError(gameObject.name+" unable to play eat coin clip, eat coin clip not assign");
			}
			
			base.OnTriggerEnter2D(other);
			
		}
	}

}
