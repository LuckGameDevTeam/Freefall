using UnityEngine;
using System.Collections;

/// <summary>
/// Cat cookie big ability.
/// 
/// This class is subclass of Ability
/// </summary>
public class CatCookieBigAbility : Ability 
{
	/// <summary>
	/// How many health will add to ability owner.
	/// </summary>
	public float addHealth = 3f;

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		//add health to owner 
		character.GetComponent<CharacterHealth> ().AddHP(addHealth);

		RemoveAbility ();
	}

}
