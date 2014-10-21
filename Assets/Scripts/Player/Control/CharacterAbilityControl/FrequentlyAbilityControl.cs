using UnityEngine;
using System.Collections;

/// <summary>
/// Frequently ability control.
/// 
/// This class is subclass of CharacterAbilityControl.
/// 
/// This class calculate or decide the time and when to give character assigend ability
/// </summary>
public class FrequentlyAbilityControl : CharacterAbilityControl
{
	/// <summary>
	/// The duration between last ability that was given and 
	/// the ability that will give to character now
	/// </summary>
	public float abilityGivenDuration = 7f;

	/// <summary>
	/// The time reached and give ability to character
	/// </summary>
	private float nextTimeToGive = 0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(running == false)
		{
			//set next generate time
			nextTimeToGive = Time.time + abilityGivenDuration;

			return;
		}

		//if ability is not using
		if(usingAbility == false)
		{
			//check if it is the time to give ability
			if(Time.time >= nextTimeToGive)
			{
				GiveAbility();

				//set next given time
				nextTimeToGive = Time.time + abilityGivenDuration;
			}
		}
		else
		{
			//set next generate time
			nextTimeToGive = Time.time + abilityGivenDuration;
		}
	}

	protected override void CharacterAbilityStart(Ability ability)
	{
		base.CharacterAbilityStart (ability);
	}

	protected override void CharacterAbilityEnd(Ability ability)
	{
		base.CharacterAbilityEnd (ability);
	}
}
