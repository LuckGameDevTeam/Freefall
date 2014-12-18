using UnityEngine;
using System.Collections;

/// <summary>
/// Candy cat's ability.
/// 
/// This class is subclass of Ability
/// </summary>
public class CandyCatAbility : Ability 
{
	/// <summary>
	/// Prefab of cookie that will turn obstacle into cookie
	/// </summary>
	public GameObject cookiePrefab;

	/// <summary>
	/// The ability clip.
	/// </summary>
	public AudioClip abilityClip;

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);
		
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (Tags.obstacle);
		
		for(int i=0; i<obstacles.Length; i++)
		{
			Obstacle o = obstacles[i].GetComponent<Obstacle>();

			//if obstacle is in screen
			if(o.IsVisibleFromCamera(Camera.main))
			{
				//if it is not a type of boss
				if(o.monsterType != MonsterTypes.Boss)
				{
					//get obstacle position
					Vector3 oPos = obstacles[i].transform.position;

					//get CatCookieMovable gameobject
					//GameObject cookie = GameController.sharedGameController.objectPool.GetObjectFromPool(cookiePrefab, oPos, Quaternion.identity);
					//GameObject cookie = TrashMan.spawn(cookiePrefab, oPos, Quaternion.identity);

					//check if prefab is in the TrashMan's bin
					GameObject cookie = TrashMan.spawn(cookiePrefab.name, oPos, Quaternion.identity);

					//add bin to TrashMan and spawn object
					if(cookie == null)
					{
						TrashManRecycleBin newBin = new TrashManRecycleBin();
						newBin.prefab = cookiePrefab;
						newBin.instancesToAllocateIfEmpty = 2;
						newBin.instancesToPreallocate = 2;
						newBin.cullExcessPrefabs = false;
						newBin.imposeHardLimit = false;

						TrashMan.manageRecycleBin(newBin);

						cookie = TrashMan.spawn(cookiePrefab, oPos, Quaternion.identity);
					}

					CatCookieMovable cc = cookie.GetComponent<CatCookieMovable>();

					//set cookie's dest as same as obstacle's dest
					cc.dest = o.Destination;

					//set cookie move speed as same as obstacle's speed
					cc.moveSpeed = o.speed;

					//recycle obstacle
					//GameController.sharedGameController.objectPool.RecycleObject(obstacles[i]);
					TrashMan.despawn(obstacles[i]);
				}
			}
			
		}

		//play ability clip
		if(abilityClip != null)
		{
			AudioSource.PlayClipAtPoint(abilityClip, transform.position);
		}

		//Remove ability
		RemoveAbility ();
	}
	
	protected override void RemoveAbility()
	{
		base.RemoveAbility ();

	}
	
	public override void RemoveAbilityImmediately ()
	{
		base.RemoveAbilityImmediately ();

	}
	
	protected override void ProcessAbility()
	{
		base.ProcessAbility ();
	}

	protected override void EffectFinished()
	{
		base.EffectFinished ();
		
		RemoveAbility ();
	}
}
