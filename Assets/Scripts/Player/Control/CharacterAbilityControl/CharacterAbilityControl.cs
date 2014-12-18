using UnityEngine;
using System.Collections;

/// <summary>
/// Character ability control.
/// 
/// This is generic class that responsible to give player assigned ability
/// and ability event registeration/unregistration 
/// </summary>
public abstract class CharacterAbilityControl : MonoBehaviour 
{
	/// <summary>
	/// The ability prefab.
	/// 
	/// The ability that will give to player
	/// </summary>
	public GameObject abilityPrefab;

	/// <summary>
	/// Enable ability control.
	/// Make it running.
	/// </summary>
	[System.NonSerialized]
	public bool running = false;

	/// <summary>
	/// Indicate the ability that give to character is in use or not
	/// </summary>
	protected bool usingAbility = false;

	/// <summary>
	/// Reference to CharacterControl
	/// </summary>
	private CharacterControl characterControl;

	void Awake()
	{
		//find CharacterControl
		characterControl = GetComponent<CharacterControl> ();
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Give character ability
	/// </summary>
	protected virtual void GiveAbility()
	{
		if(abilityPrefab != null)
		{
			//retrieve ability from object pool
			//GameObject abilityObject = GameController.sharedGameController.objectPool.GetObjectFromPool(abilityPrefab, transform.position, Quaternion.identity);
			GameObject abilityObject = TrashMan.spawn(abilityPrefab, transform.position, Quaternion.identity);

			Ability ability = abilityObject.GetComponent<Ability>();

			//register event
			ability.Evt_AbilityStart += CharacterAbilityStart;
			ability.Evt_AbilityRemoved += CharacterAbilityEnd;

			//give ability to character
			characterControl.AddAbility(abilityObject);
		}
		else
		{
			DebugEx.DebugWarning("No ability prefab was assigned, character won't have ability");
		}
	}

	/// <summary>
	/// Call when ability that attached to character is started
	/// </summary>
	protected virtual void CharacterAbilityStart(Ability ability)
	{
		usingAbility = true;
	}

	/// <summary>
	/// Call when ability that attached to chartacter is end
	/// </summary>
	protected virtual void CharacterAbilityEnd(Ability ability)
	{
		usingAbility = false;

		//unregister event
		ability.Evt_AbilityStart -= CharacterAbilityStart;
		ability.Evt_AbilityRemoved -= CharacterAbilityEnd;
	}
}
