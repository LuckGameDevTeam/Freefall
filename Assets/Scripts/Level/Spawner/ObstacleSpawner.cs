using UnityEngine;
using System.Collections;

/// <summary>
/// Obstacle spawner.
/// 
/// This class is subclass of LevelSpawner
/// 
/// This class is designed to spawn all obstacles(monsters) in scene 
/// </summary>
public class ObstacleSpawner : LevelSpawner 
{
	/// <summary>
	/// The sibling.
	/// This should be assign with same level of spawner
	/// This is also the distinaion of spawned obstacle
	/// </summary>
	public GameObject sibling;

	protected override void Awake()
	{
		base.Awake ();
	}

	void Update()
	{

	}

	/// <summary>
	/// Spawns the obstacle.
	/// Spawn one obstacle.
	/// </summary>
	public override GameObject SpawnObject(GameObject objectPrefab)
	{

		//obstacle prefab
		GameObject prefab = objectPrefab;

		//spawn obstacle
		//GameObject obstacle = GameController.sharedGameController.objectPool.GetObjectFromPool (prefab, transform.position, Quaternion.identity);
		GameObject obstacle = TrashMan.spawn (prefab, transform.position, Quaternion.identity);

		//get obstacle script
		Obstacle o = obstacle.GetComponent<Obstacle> ();

		if(o.monsterType != MonsterTypes.Boss)
		{
			//fix spawn position
			//Vector2 sPos = FixObstacleSpawnPosition (Camera.main, o.renderer, transform.ConvertPositionToVector2 ());
			Vector2 sPos = PositionEx.FixSpawnPosition (Camera.main, o.renderer, transform.ConvertPositionToVector2 ());
			obstacle.transform.position = new Vector3 (sPos.x, sPos.y, transform.position.z);
			
			//set destination
			//o.Destination = FixObstacleDestination(Camera.main, o.renderer, sibling.transform.ConvertPositionToVector2(), 2f);
			o.Destination = PositionEx.FixDestination(Camera.main, o.renderer, sibling.transform.ConvertPositionToVector2(), 2f);
		}

		//init obstacle
		o.InitObstacle ();


		return obstacle;
	}

	/// <summary>
	/// Fixs the obstacle destination.
	/// Take destination and sprite to calculate proper destinaiton
	/// This allow you to place spawner just out side of camera and don't
	/// have to place percisely.
	/// </summary>
	/// <returns>The fixed obstacle destination.</returns>
	/// <param name="cam">Cam.</param>
	/// <param name="spriteRender">Sprite render.</param>
	/// <param name="target">Destination.</param>
	/// <param name="offset">the offset that is out of screen</param>
	Vector2 FixObstacleDestination(Camera cam, Renderer spriteRender, Vector2 destination, float offset = 0f)
	{
		//calculate destination for obstacle
		//destination must out of screen involve obstacle bound

		Vector2 retDest = destination;

		//find camera bound
		float camLeftBound = cam.GetLeftBorderWorldSpace (transform.position.z);
		float camRightBound = cam.GetRightBorderWorldSpace (transform.position.z);
		float camTopBound = cam.GetTopBorderWorldSpace (transform.position.z);
		float camBottomBound = cam.GetBottomBorderWorldSpace (transform.position.z);

		//final position
		float fx, fy;
		
		if((destination.x < camLeftBound) || (sibling.transform.position.x == camLeftBound))
		{
			// camera's left
			fx = camLeftBound - spriteRender.bounds.extents.x - offset;
		}
		else if(destination.x > camRightBound || (sibling.transform.position.x == camRightBound))
		{
			//camera's right
			fx = camRightBound + spriteRender.bounds.extents.x + offset;
		}
		else
		{
			//unknow
			fx = retDest.x;
		}
		
		if((destination.y < camBottomBound) || (sibling.transform.position.y == camBottomBound))
		{
			//camera's bottom
			fy = camBottomBound - spriteRender.bounds.extents.y - offset;
		}
		else if((destination.y > camTopBound) || (sibling.transform.position.y == camTopBound))
		{
			//camera's top
			fy = camTopBound + spriteRender.bounds.extents.y + offset;
		}
		else
		{
			//unknow
			fy = retDest.y;
		}

		//set final destination
		retDest = new Vector2 (fx, fy);

		return retDest;
	}

	/// <summary>
	/// Fixs the obstacle spawn position.
	/// Take position and sprite to calculate proper destinaiton
	/// This allow you to place spawner just slightly out side of camera and don't
	/// have to place percisely.
	/// </summary>
	/// <returns>The fixed obstacle position.</returns>
	/// <param name="cam">Cam.</param>
	/// <param name="spriteRender">Sprite render.</param>
	/// <param name="target">Sapwn Pos.</param>
	Vector2 FixObstacleSpawnPosition(Camera cam, Renderer spriteRender, Vector2 spawnPos)
	{
		return FixObstacleDestination (cam, spriteRender, spawnPos);
	}
	
}
