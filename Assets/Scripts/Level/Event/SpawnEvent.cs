using UnityEngine;
using System.Collections;

/// <summary>
/// Spawn event.
/// 
/// This class is subclass of LevelEvent
/// 
/// This class is designed for event that spawn all obstacles(monster).
/// 
/// Ths spawn process will require to be implementd by subclass.
/// 
/// It required you to give the obstacles that will be spawn, as well as how many time
/// obstacle will spawn, spawn points, delay which is duration between each spawn.
/// </summary>
public class SpawnEvent : LevelEvent 
{
	/// <summary>
	/// Class of spawned obect meta data
	/// it descript how many time certain object
	/// is going to spawn. As well as, the prefab of 
	/// object
	/// </summary>
	[System.Serializable]
	public class SpawnedObjectMetaData
	{
		/// <summary>
		/// Prefab that will be spawn
		/// </summary>
		public GameObject prefabToSpawn;

		/// <summary>
		/// How many time this object will be spawn
		/// </summary>
		public int spawnTimes;
	}

	/// <summary>
	/// The delay.
	/// How much delay until next process
	/// </summary>
	public float delay = 0f;

	/// <summary>
	/// Contain metadata about object that will be spawn
	/// </summary>
	public SpawnedObjectMetaData[] spawnObjects;

	/// <summary>
	/// Contain spawn points in scene
	/// </summary>
	public GameObject[] spawnPoints;
	
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
		//no object to spawn
		if(totalObjectToSpawn <= 0)
		{
			return;
		}

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

		//find out how many objects is going to spawn
		if(spawnObjects.Length > 0)
		{
			foreach(SpawnedObjectMetaData objData in spawnObjects)
			{
				totalObjectToSpawn += objData.spawnTimes;
			}
		}

		//prevent double spawn at same point
		//no delay and spawn point count less than total obstacle spawn count
		if(delay == 0)
		{
			if(spawnPoints.Length < totalObjectToSpawn)
			{
				Debug.LogError(gameObject.name+" have number of spawn objects: "+totalObjectToSpawn+" and have number of spawn points: "+spawnPoints.Length+" your delay was set to 0");
				Debug.LogError(gameObject.name+" it is recommended to set delay to non 0 value, but system do it for you now, your delay will be set to 1 to prevent logical error");

				delay = 1f;
			}
		}
	}
	
	public override void StopEvent()
	{
		base.StopEvent ();
	}

	/// <summary>
	/// An object will be sapwn
	/// 
	/// Subclass must implement this method
	/// Subclass must spawn a gameobject during this method
	/// 
	/// <param name="delayEnabled">Wether spawn delay is on or off</param>
	/// </summary>
	protected virtual void Spawn(bool delayEnabled)
	{

	}
	
}
