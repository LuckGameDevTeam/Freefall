using UnityEngine;
using System.Collections;

/// <summary>
/// Random ability control.
/// 
/// This class is subclass of CharacterAbilityControl.
/// 
/// This class calculate the chance that might give character ability
/// 
/// </summary>
public class RandomAbilityControl : CharacterAbilityControl 
{
	/// <summary>
	/// The ability happen rate.
	/// 
	/// The chace ability control will give ability to character
	/// </summary>
	[Range(1, 100)]
	public int abilityGivenRate = 50;

	/// <summary>
	/// The duration between last chace generated and 
	/// now 
	/// </summary>
	public float randGenDuration = 1f;

	/// <summary>
	/// Next time rand chance generator start
	/// </summary>
	private float nextGenTime = 0f;

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
			nextGenTime = Time.time + randGenDuration;

			return;
		}

		if(usingAbility == false)
		{
			if(Time.time >= nextGenTime)
			{
				int rand = Random.Range(1, 101);
				
				if(rand <= abilityGivenRate)
				{
					GiveAbility();
				}

				//set next generate time
				nextGenTime = Time.time + randGenDuration;
			}

		}
		else
		{
			//set next generate time
			nextGenTime = Time.time + randGenDuration;
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
