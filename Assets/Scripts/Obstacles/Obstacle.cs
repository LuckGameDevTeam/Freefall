using UnityEngine;
using System.Collections;

[System.Flags]
public enum MonsterTypes
{
	Boss = 1,
	MonsterSmall = 2,
	MonsterMedium = 4
}

/// <summary>
/// Obstacle.
/// 
/// This generic class for all obstacles.
/// 
/// This class handle obstacle's dead effect playing, obstacle's life, movement or bouncing.
/// 
/// Remember to call InitObstacle
/// </summary>
[RequireComponent (typeof(Rigidbody2D))]
public class Obstacle : MonoBehaviour 
{
	/// <summary>
	/// The type of this monster
	/// </summary>
	public MonsterTypes monsterType = MonsterTypes.MonsterSmall;

	/// <summary>
	/// The bounce clip.
	/// </summary>
	public AudioClip bounceClip;

	/// <summary>
	/// Prefab effect for obstacle dead
	/// </summary>
	public GameObject deadEffectPrefab;

	/// <summary>
	/// is this obstacle dead or not.
	/// change this value cause obstacle remove from scene
	/// </summary>
	[System.NonSerialized]
	public bool isDead = false;

	/// <summary>
	/// can obstacle to be destroy whe contact with character
	/// </summary>
	[System.NonSerialized]
	public bool canDestroy = true;

	/// <summary>
	/// The obstacle's moving speed.
	/// </summary>
	public float speed = 5f;

	/// <summary>
	/// Is this obstacle rotatable
	/// </summary>
	public bool canRotate = false;

	/// <summary>
	/// True then obstacle will rotate random direction
	/// </summary>
	public bool randomRotate = false;

	/// <summary>
	/// The rotate speed.
	/// </summary>
	public float rotateSpeed = 90f;

	/// <summary>
	/// The stop radius.
	/// If distance from obstacle to it's destination is less
	/// then or equal to this value, obstacle will stop moving
	/// </summary>
	//public float stopRadius = 0.2f;

	/// <summary>
	/// How long this obstacle can live
	/// Time must greater than the time that obstacle enter screen from spawn point
	/// </summary>
	public float lifeSpan = 2f;

	/// <summary>
	/// The obstacle's damage.
	/// </summary>
	public float damage = 1f;

	/// <summary>
	/// The force for bunce
	/// </summary>
	private float bounceForce = 5f;

	/// <summary>
	/// give non zero value to make obstacle start bounce
	/// </summary>
	[System.NonSerialized]
	public Vector2 bounceDirection = Vector2.zero;

	/// <summary>
	/// The rotate direction.
	/// </summary>
	private float rotateDirection = 1f;

	/// <summary>
	/// The moving velocity.
	/// </summary>
	private Vector2 movingVelocity = Vector2.zero;

	/// <summary>
	/// The lastposition.
	/// </summary>
	private Vector2 lastposition = Vector2.zero;

	/// <summary>
	/// obstacle destination
	/// </summary>
	private Vector2 dest;

	/// <summary>
	/// used to check if life spane reach
	/// </summary>
	private float lifeSpanTime = 0f;

	/// <summary>
	/// Reference to game controller
	/// </summary>
	//protected GameController gameController = null;

	/// <summary>
	/// Reference to rigidbody
	/// </summary>
	private Rigidbody2D theRigidbody = null;

	/// <summary>
	/// Reference to collider
	/// </summary>
	private Collider2D collision = null;

	/// <summary>
	/// Reference to SFXPlayer
	/// </summary>
	protected SFXPlayer soundPlayer = null;

	protected virtual void Awake()
	{
		//find game controller
		//gameController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<GameController> ();

		//find rigidbody
		theRigidbody = GetComponent<Rigidbody2D> ();

		//find collider
		collision = GetComponent<Collider2D> ();

		//find SFXPlayer
		soundPlayer = GetComponent<SFXPlayer> ();

		//add one if there is no SFXPlayer
		if(soundPlayer == null)
		{
			soundPlayer = (SFXPlayer)gameObject.AddComponent<SFXPlayer>();
		}

	}

	protected virtual void Start()
	{
		//disable gravity for obstacle
		theRigidbody.gravityScale = 0f;

		//set continuous collision mode for obstacle
		theRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

		//set collision as trigger
		collision.isTrigger = true;

		//if random rotation
		if(canRotate && randomRotate)
		{
			//pick any direction
			if(Random.Range(0, 2) == 1)
			{
				rotateDirection = -1f;
			}
		}

		lastposition = transform.ConvertPositionToVector2 ();

	}

	protected virtual void OnEnable()
	{
		//set life spane time
		lifeSpanTime = Time.time + lifeSpan;
	}

	protected virtual void OnDisable()
	{
		lifeSpanTime = 0f;

		isDead = false;

		canDestroy = true;

		bounceDirection = Vector2.zero;
	}

	// Update is called once per frame
	protected virtual void Update () 
	{
		if(isDead && canDestroy)
		{
			//play dead effect
			if(deadEffectPrefab != null)
			{
				GameObject effect = GameController.sharedGameController.objectPool.GetObjectFromPool(deadEffectPrefab, transform.position, Quaternion.identity);
				effect.GetComponent<EffectAnimation>().PlayAnimation();
			}


			//recycle obstacle
			GameController.sharedGameController.objectPool.RecycleObject(gameObject);

			return;
		}

		//if obstacle life reach
		if(Time.time >= lifeSpanTime)
		{
			//if out of camera/screen recycle object
			if(renderer.IsVisibleFromCamera(Camera.main) == false)
			{
				//recycle obstacle immediately
				GameController.sharedGameController.objectPool.RecycleObject(gameObject);
				
				return;
			}
		}

		//if obstacle is bouncing or moving
		if(bounceDirection != Vector2.zero)
		{
			bounceDirection = bounceDirection.normalized;

			BounceObstacle(bounceDirection);
		}
		else
		{
			//move obstacle
			MoveObstacle ();
		}

		//calcualte moving velocity
		CalculateMovingVelocity ();


		//if obstacle can rotate...rotate obstacle
		if(canRotate)
		{
			RotateObstcale ();
		}

	}

	////////////////////////////////Public interface////////////////////////////////

	/// <summary>
	/// Initialize obstacle
	/// </summary>
	public virtual void InitObstacle()
	{

	}

	/// <summary>
	/// Is this obstacle visible from certain camera.
	/// </summary>
	/// <value>True visible from camera..otherwise false.</value>
	public bool IsVisibleFromCamera(Camera cam)
	{
		return renderer.IsVisibleFromCamera (cam);
	}

	/// <summary>
	/// Call to make medium obstacle become the small obstacle.
	/// </summary>
	public virtual void BecomeSmallObstacle()
	{

	}

	////////////////////////////////Public interface////////////////////////////////

	////////////////////////////////Internal////////////////////////////////

	/// <summary>
	/// Moves the obstacle.
	/// </summary>
	protected virtual void MoveObstacle()
	{

		//calculate direction from obstacle to destination
		Vector2 direction = (dest - transform.ConvertPositionToVector2 ()).normalized;

		//calculate amout of movement
		Vector2 amount = direction * (speed * Time.deltaTime);

		//move obstacle
		transform.position = new Vector3 (transform.position.x + amount.x, transform.position.y + amount.y, transform.position.z);
	}

	/// <summary>
	/// Calculates the moving velocity.
	/// </summary>
	protected virtual void CalculateMovingVelocity()
	{
		//calcualte moving velocity
		movingVelocity = transform.ConvertPositionToVector2 () - lastposition;
		lastposition = transform.ConvertPositionToVector2 ();
	}

	/// <summary>
	/// Rotates the obstcale.
	/// </summary>
	void RotateObstcale()
	{

		transform.Rotate (Vector3.forward * (rotateDirection * (rotateSpeed * Time.deltaTime)));

	}

	/// <summary>
	/// Bounces the obstacle.
	/// </summary>
	/// <param name="bounceDir">Bounce direction and it is normalized.</param>
	protected virtual void BounceObstacle(Vector2 bounceDir)
	{
		Vector2 amount = (bounceForce * Time.deltaTime) * bounceDir;
		
		transform.position = new Vector3 (transform.position.x + amount.x, transform.position.y + amount.y, transform.position.z);

	}


	////////////////////////////////Internal////////////////////////////////

	////////////////////////////////Properties////////////////////////////////
	
	/// <summary>
	/// Set/Get the destination.
	/// </summary>
	/// <value>The destination.</value>
	public Vector2 Destination
	{
		get
		{
			return dest;
		}

		set
		{ 
			dest = value; 
		} 
	}

	/// <summary>
	/// Obstacle's current moving velocity
	/// </summary>
	/// <value>The moving velocity.</value>
	public Vector2 MovingVelocity{get{return movingVelocity;}}

	/// <summary>
	/// Gets or sets the bounce force.
	/// </summary>
	/// <value>The bounce force.</value>
	public float BounceForce
	{
		get
		{
			return bounceForce;
		}

		set
		{
			bounceForce = value;

			//play bounce clip
			if(bounceClip != null)
			{
				if(soundPlayer == null)
				{
					soundPlayer = (SFXPlayer)gameObject.AddComponent<SFXPlayer>();
				}
				
				if(!soundPlayer.IsPlaying)
				{
					soundPlayer.sfxClip = bounceClip;
					soundPlayer.PlaySound();
				}
				
			}
			else
			{
				Debug.LogError("Unable to play sound effect "+gameObject.name+" audio bounce clip not assigned");
			}
		}
	}

	////////////////////////////////Properties////////////////////////////////
}
