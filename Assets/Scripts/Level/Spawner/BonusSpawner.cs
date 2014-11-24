using UnityEngine;
using System.Collections;

/// <summary>
/// Bonus spawner.
/// 
/// This class is subclass of LevelSpawner.
/// 
/// This class is designed to spawn Bonus such Cat Coine, Fish Bone and Cat Cookie
/// </summary>
public class BonusSpawner : LevelSpawner 
{

	/// <summary>
	/// Spawns the bonus.
	/// Spawn one bonus.
	/// </summary>
	public override GameObject SpawnObject(GameObject objectPrefab)
	{
		
		//bonus prefab
		GameObject prefab = objectPrefab;
		
		//spawn bonus
		//GameObject bonus = GameController.sharedGameController.objectPool.GetObjectFromPool (prefab, transform.position, Quaternion.identity);
		GameObject bonus = TrashMan.spawn (prefab, transform.position, Quaternion.identity);

		//fix poisition on y
		float newY = bonus.transform.position.y;

		if(bonus.transform.position.y <= Camera.main.GetBottomBorderWorldSpace(bonus.transform.position.z))
		{
			newY = Camera.main.GetBottomBorderWorldSpace(bonus.transform.position.z) - bonus.GetComponent<AssistantItemHolder>().GetHalfHeight();
		}

		bonus.transform.position = new Vector3 (bonus.transform.position.x, newY, bonus.transform.position.z);
		
		return bonus;
	}
}
