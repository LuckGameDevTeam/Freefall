using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Obstacle event.
/// 
/// This class is subclass of SpawnEvent.
/// 
/// This class is special designed to remember which spawn point
/// has spawned obstacle and not allow it to be used to spawn again when
/// "Delay" is 0 otherwise, spawn points can be reused.
/// 
/// This class also handle spawn process.
/// 
/// </summary>
public class ObstacleEvent : SpawnEvent 
{
	/// <summary>
	/// Key
	/// keep spawn count of each obstacle
	// keep prefab name as key and spawnTimes as value
	// key will be the gameobject prefab, value will be spawnTime
	// it will be modify after an obstacle spawn
	/// </summary>
	private List<GameObject> obstacleSpawnKeys = new List<GameObject>();

	/// <summary>
	/// Value
	/// keep spawn count of each obstacle
	// keep prefab name as key and spawnTimes as value
	// key will be the gameobject prefab, value will be spawnTime
	// it will be modify after an obstacle spawn
	/// </summary>
	private List<int> obstacleSpawnValues = new List<int>();

	/// <summary>
	/// contain the spawn points that has been spawned obstacle
	/// to prevent same spawn point spawn obstacle more than once
	/// </summary>
	private List<GameObject> spawnedPoints = new List<GameObject>();

	protected override void Awake()
	{
		base.Awake ();
	}

	public override void TriggerEvent()
	{
		base.TriggerEvent ();

		//obstacleSpawnKeys = new List<GameObject> ();
		//obstacleSpawnValues = new List<int> ();
		//spawnedPoints = new List<GameObject> ();

		//keep spawn count of each obstacle
		foreach(SpawnedObjectMetaData objData in spawnObjects)
		{
			if(obstacleSpawnKeys.Contains(objData.prefabToSpawn))
			{
				continue;
			}
			else
			{
				//add key,value
				obstacleSpawnKeys.Add(objData.prefabToSpawn);
				obstacleSpawnValues.Add(objData.spawnTimes);

			}
		}

	}

	public override void StopEvent()
	{
		base.StopEvent ();

		if(obstacleSpawnKeys != null)
		{
			obstacleSpawnKeys.Clear ();
		}

		if(obstacleSpawnValues != null)
		{
			obstacleSpawnValues.Clear ();
		}

		if(spawnedPoints != null)
		{
			spawnedPoints.Clear ();
		}

	}

	protected override void Spawn(bool delayEnabled)
	{
		base.Spawn (delayEnabled);

		bool spawnSuccessful = false;

		while(spawnSuccessful != true)
		{
			int selectedIndex = 0;

			//pick spawn points
			selectedIndex = Random.Range(0, spawnPoints.Length);
			GameObject selectSpawnPoint = spawnPoints[selectedIndex];

			//if spawn delay not enabled
			if(!delayEnabled)
			{
				//make sure spawn point not used more than once
				if(spawnedPoints.Contains(selectSpawnPoint))
				{
					continue;
				}

				//add spawn point to list to pevent spawn more than once
				spawnedPoints.Add(selectSpawnPoint);
			}


			if((obstacleSpawnKeys.Count <= 0) && (obstacleSpawnValues.Count <= 0))
			{
				return;
			}


			//pick obstacle to spawn
			selectedIndex = Random.Range(0, obstacleSpawnKeys.Count);
			GameObject obstaclePrefab = obstacleSpawnKeys[selectedIndex];

			//tell spawn point to spawn obstacle
			LevelSpawner spawner = selectSpawnPoint.GetComponent<LevelSpawner>();
			spawner.SpawnObject(obstaclePrefab);

			//decrease spawned obstacle count
			obstacleSpawnValues[selectedIndex] = obstacleSpawnValues[selectedIndex]-1;

			//check if obstacle spawn times reach 0 then remove
			if(obstacleSpawnValues[selectedIndex] <= 0)
			{
				//remove key and value
				obstacleSpawnKeys.RemoveAt(selectedIndex);
				obstacleSpawnValues.RemoveAt(selectedIndex);
			}

			spawnSuccessful = true;

		}
	}
}
