using UnityEngine;
using System.Collections;

/// <summary>
/// Ability assistant item.
/// 
/// This class is subclass of AssistantItem.
/// 
/// This class is designed for item that have ability
/// and will give to character when character touch it.
/// 
/// Attach this script to item gameobject that have ability
/// and assign ability prefab.
/// </summary>
public class AbilityAssistantItem : AssistantItem 
{

	/// <summary>
	/// The ability that will give to character.
	/// </summary>
	public GameObject abilityPrefab;
	
	protected override void Awake()
	{
		base.Awake ();
	}
	
	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Tags.player)
		{
			//create ability gameobject
			//GameObject ability = Instantiate(abilityPrefab) as GameObject;
			//ability.name = abilityPrefab.name;

			//since ability is recyclable so we get it from pool
			//GameObject ability = GameController.sharedGameController.objectPool.GetObjectFromPool(abilityPrefab, Vector3.zero, Quaternion.identity);
			GameObject ability = TrashMan.spawn(abilityPrefab, Vector3.zero, Quaternion.identity);
			
			//give to player
			GameController.sharedGameController.character.GetComponent<CharacterControl>().AddAbility(ability);
			
			base.OnTriggerEnter2D(other);
		}
	}
}
