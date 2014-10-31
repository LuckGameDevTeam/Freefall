using UnityEngine;
using System.Collections;

/// <summary>
/// Bonus event.
/// 
/// This class is subclass of LevelEvent
/// 
/// This class is designed for event that spawn Bonus.
/// 
/// This event require you to give Bonus prefab gameobjects that
/// might be spawned, spawn points, how many time to spawn and delay
/// which duration between each spawn.
/// </summary>
public class BonusEvent : LevelEvent 
{

	/// <summary>
	/// Bunch of prefab that might be spawn
	/// Only one will be select
	/// </summary>
	public GameObject[] availablePrefabs;

	/// <summary>
	/// Contain spawn points in scene
	/// </summary>
	public GameObject[] spawnPoints;

	/// <summary>
	/// How many time to spawn
	/// </summary>
	public int totalSpawn = 1;
	
	/// <summary>
	/// The delay.
	/// How much delay until next process
	/// </summary>
	public float delay = 0f;

	/// <summary>
	/// next time to spawn
	/// </summary>
	private float nextSpawnTime = 0f;
	
	/// <summary>
	/// total spawn count 
	/// </summary>
	private int totalObjectToSpawn = 0;

	protected override void Awake()
	{
		base.Awake ();
	}

	void Update()
	{
		//if delay was not set...otherwise process delay spawn
		if(delay == 0)
		{
			while(totalObjectToSpawn > 0)
			{
				//spawn
				Spawn(false);
				
				//decrease the spawn count
				totalObjectToSpawn--;
			}
			
			//stop this event
			StopEvent();
		}
		else
		{
			if(Time.time >= nextSpawnTime)
			{
				//if there still are spawn count left
				if(totalObjectToSpawn > 0)
				{
					//spawn
					Spawn(true);
					
					//decrease the spawn count
					totalObjectToSpawn--;
					
					//if spawn count reach or below 0
					if(totalObjectToSpawn <= 0)
					{
						//stop this event
						StopEvent();
					}
					else
					{
						//find out next spawn time
						nextSpawnTime = Time.time + delay;
					}
				}
			}
		}
	}

	public override void TriggerEvent()
	{
		base.TriggerEvent ();

		totalObjectToSpawn = totalSpawn;
	}

	public override void StopEvent()
	{
		base.StopEvent ();
	}



	void Spawn(bool delayEnabled)
	{
		if ((spawnPoints.Length <= 0) || (availablePrefabs.Length <= 0))
		{
			return;
		}

		//pick spawn points
		int selectedIndex = Random.Range (0, spawnPoints.Length);
		GameObject selectSpawnPoint = spawnPoints[selectedIndex];
		
		
		//pick bonus to spawn
		selectedIndex = Random.Range(0, availablePrefabs.Length);
		GameObject bonusPrefab = availablePrefabs[selectedIndex];

		//prefabe is null
		if(bonusPrefab == null)
		{
			Debug.LogError(gameObject.name+" can not spawn object, prefab missing at element: "+selectedIndex);
			
			return;
		}
		
		//tell spawn point to spawn obstacle
		LevelSpawner spawner = selectSpawnPoint.GetComponent<LevelSpawner>();
		spawner.SpawnObject(bonusPrefab);
	}
}
