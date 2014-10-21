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

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		//increase size of character
		character.transform.localScale = new Vector3 (characterSize, characterSize, 1f);

		//make character invulnerable
		character.GetComponent<CharacterHealth> ().invulnerable = true;
	}

	protected override void RemoveAbility()
	{
		base.RemoveAbility ();

		//set character size to normal
		character.transform.localScale = new Vector3 (1f, 1f, 1f);

		//make character vulnerable again
		character.GetComponent<CharacterHealth> ().invulnerable = false;
	}

	public override void RemoveAbilityImmediately ()
	{
		base.RemoveAbilityImmediately ();

		//set character size to normal
		character.transform.localScale = new Vector3 (1f, 1f, 1f);

		//make character vulnerable again
		character.GetComponent<CharacterHealth> ().invulnerable = false;
	}

	protected override void ProcessAbility()
	{
		base.ProcessAbility ();
	}
}
