using UnityEngine;
using System.Collections;

/// <summary>
/// Invulnerable ability.
/// 
/// This class is subclass of Ability.
/// This ability allow character to become invulnerable for few seconds
/// </summary>
public class InvulnerableAbility : Ability 
{

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		//set character invulnerable
		character.GetComponent<CharacterHealth> ().invulnerable = true;
	}
	
	protected override void RemoveAbility()
	{
		//set character vulnerable
		character.GetComponent<CharacterHealth> ().invulnerable = false;

		base.RemoveAbility ();
	}

	public override void RemoveAbilityImmediately()
	{
		//set character vulnerable
		character.GetComponent<CharacterHealth> ().invulnerable = false;
		
		base.RemoveAbilityImmediately ();
	}
	
	protected override void ProcessAbility()
	{
		base.ProcessAbility ();
	}
}
