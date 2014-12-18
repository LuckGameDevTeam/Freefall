using UnityEngine;
using System.Collections;

/// <summary>
/// Obstacle big.
/// 
/// This class is subclass of Obstacle.
/// 
/// This class is designed for obstacle that is big such as Boss monster.
/// 
/// Boss has it's own moving path
/// </summary>
public class ObstacleBig : Obstacle 
{
	/// <summary>
	/// The path prefix.
	/// 
	/// This is same as PathPoint in path
	/// Each path has it's own path prefix
	/// </summary>
	public string pathPrefix;

	/// <summary>
	/// The destination migrate.
	/// 
	/// If distance to target is less or equal to 
	/// this value then it mean arrive target
	/// </summary>
	public float destMigrate = 0.5f;

	/// <summary>
	/// The boss clip.
	/// </summary>
	public AudioClip bossClip;

	/// <summary>
	/// The current path point.
	/// </summary>
	private PathPoint currentPathPoint;

	/// <summary>
	/// The begin wait time.
	/// </summary>
	private float beginWaitTime = 0f;

	/// <summary>
	/// Reference to PathManager
	/// </summary>
	private PathManager  pathMgr;

	protected override void Awake()
	{
		base.Awake ();


	}

	protected override void Start()
	{
		base.Start ();


	}

	protected override void OnEnable()
	{
		base.OnEnable ();

		if((bossClip != null) && (soundPlayer != null))
		{
			soundPlayer.sfxClip = bossClip;
			soundPlayer.loop = true;
			soundPlayer.PlaySound();
		}

	}
	
	protected override void OnDisable()
	{
		base.OnDisable ();

		currentPathPoint = null;

		if(soundPlayer != null)
		{
			if(soundPlayer.IsPlaying)
			{
				soundPlayer.StopSound();
			}
		}
	}

	/// <summary>
	/// Override parent update logic.
	/// </summary>
	protected override void Update () 
	{
		//if paths has assigned
		if(currentPathPoint != null)
		{
			//if reach destination
			if(Vector2.Distance(Destination, transform.ConvertPositionToVector2()) <= destMigrate)
			{
				//check if has more path or not...no path recycle gameobject...otherwise move to next path
				if(currentPathPoint == pathMgr.GetLastPathPoint(pathPrefix))
				{
					//recycle obstacle
					//GameController.sharedGameController.objectPool.RecycleObject(gameObject);
					TrashMan.despawn(gameObject);
				}
				else
				{
					//prepare next path point
					PrepareNextPathPoint(false);
				}
			}

			//On current path info, if begin wait time is over
			if(Time.time >= beginWaitTime)
			{
				//move boss
				MoveObstacle();

				//calcualte moving velocity
				CalculateMovingVelocity();
			}
		}
		else
		{
			DebugEx.DebugError(gameObject.name + " no path point");

			//recycle obstacle
			//GameController.sharedGameController.objectPool.RecycleObject(gameObject);
			TrashMan.despawn(gameObject);
		}
	}

	//on collision
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Tags.player)
		{
			//DebugEx.Debug("hit player");

			other.gameObject.SendMessageUpwards("TakeDamage", damage);
		}
	}

	public override void InitObstacle()
	{
		base.InitObstacle ();

		if(pathMgr == null)
		{
			//find path manager
			pathMgr = GameObject.FindGameObjectWithTag (Tags.pathManager).GetComponent<PathManager>();
		}

		//set PathPoint to first 
		PrepareNextPathPoint(true);
	}

	protected override void BounceObstacle (Vector2 bounceDir)
	{
		//boss do not bounce
	}


	/// <summary>
	/// Prepares the next path point.
	/// </summary>
	/// <param name="startOver">If set to true then path will start from beginning...otherwise continue next point.</param>
	void PrepareNextPathPoint(bool startOver)
	{
		if(startOver == true)
		{
			if(pathMgr == null)
			{
				//find path manager from scene
				pathMgr = GameObject.FindGameObjectWithTag(Tags.pathManager).GetComponent<PathManager>();
			}

			//change current path point to first
			currentPathPoint = pathMgr.GetFirstPathPoint(pathPrefix);
		}
		else
		{
			if(currentPathPoint.nextPoint == null)
			{
				DebugEx.DebugError(gameObject.name + "there is no next path point");

				currentPathPoint = null;

				return;
			}

			//change current path point to next
			currentPathPoint = currentPathPoint.nextPoint;
		}

		
		//set begin wait time
		beginWaitTime = Time.time + currentPathPoint.beginWait;
		
		//set begin position
		Vector2 beginPos = currentPathPoint.GetPosition2D();
		beginPos = PositionEx.FixSpawnPosition (Camera.main, renderer, beginPos, 1f);

		transform.position = new Vector3(beginPos.x, beginPos.y, transform.position.z);
		
		//set destination
		Destination = PositionEx.FixDestination(Camera.main, renderer, currentPathPoint.nextPoint.GetPosition2D(), 1f);
	}
}
