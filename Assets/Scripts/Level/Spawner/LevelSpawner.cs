using UnityEngine;
using System.Collections;

/// <summary>
/// Level spawner.
/// 
/// This is generic class for all spawner.
/// </summary>
public abstract class LevelSpawner : MonoBehaviour 
{

	/// <summary>
	/// reference to GameController
	/// </summary>
	//protected GameController gameController;

	protected virtual void Awake()
	{
		//find GameController
		//gameController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<GameController>();
	}

	/// <summary>
	/// Spawns object.
	///	
	/// <param name="prefab">The prefab of object to spawn</param>
	/// </summary>
	public abstract GameObject SpawnObject(GameObject objectPrefab);
}
