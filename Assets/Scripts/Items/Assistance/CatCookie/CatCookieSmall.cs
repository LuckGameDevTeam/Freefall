using UnityEngine;
using System.Collections;

/// <summary>
/// Cat cookie small.
/// 
/// This class is for Small Cat Cookie
/// </summary>
public class CatCookieSmall : CatCookie {

	protected override void Awake()
	{
		base.Awake ();
	}
	
	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Tags.player)
		{
			//add hp to character 
			GameController.sharedGameController.character.GetComponent<CharacterHealth>().AddHP(increaseHP);

			//tell character play cat cookie eaten effect
			GameController.sharedGameController.character.GetComponent<CharacterEffect>().PlayCatCookieEatenEffect();
			
			base.OnTriggerEnter2D(other);
			
		}
	}
}
