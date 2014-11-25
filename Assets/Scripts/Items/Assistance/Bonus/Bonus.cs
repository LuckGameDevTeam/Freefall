using UnityEngine;
using System.Collections;

/// <summary>
/// Bonus.
/// 
/// This class is subclass of AssistantItem.
/// 
/// This is generic class for such Cat Coin and Fish Bone.
/// 
/// This class also handle Magnet functionality when Magnet method
/// is called and about to become magnetable object. It simply get the 
/// same gameobject from ObjectPool then make it become magnet follow by
/// disable itself.
/// 
/// </summary>
public class Bonus : AssistantItem 
{


	/// <summary>
	/// How much bonus this Bonus object will give
	/// </summary>
	public int bonus = 1;

	
	protected override void Awake()
	{
		base.Awake ();
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D (other);
	}

	public override void Magnet(GameObject target)
	{
		base.Magnet (target);

		//get same gameobject from pool
		//GameObject magnetObject = GameController.sharedGameController.objectPool.GetObjectFromPool (gameObject.name, transform.position, Quaternion.identity);
		GameObject magnetObject = TrashMan.spawn (gameObject.name, transform.position, Quaternion.identity);

		//make it become magnetable to target
		magnetObject.GetComponent<AssistantItem> ().BecomeMagnet (target);

		//tell holder this item was eaten
		if(Evt_BonusEaten != null)
		{
			Evt_BonusEaten();
		}

		//disble this item
		gameObject.SetActive(false);
	}


}
