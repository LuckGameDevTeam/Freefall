using UnityEngine;
using System.Collections;

/// <summary>
/// Assistant item.
/// 
/// This is generic class for all items that will randomly appear
/// in level for character, such Cat Crown, Big Fish Bone, Big Cat Cookei...so on.
/// 
/// This class has another feature Magnet. As the gameobject that is attached with this
/// script and Magnet feature is enabled then gameobject will automatically move toward
/// designated target. If parent of this gameobject has component of AssistantItemHolder then
/// magnet feature will not work.
/// 
/// Magnet method require subclass to create same gameobject and then call BecomeMagnet method
/// to created gameobject after creation.
/// 
/// This class responsible to recycle gameobject, send out event and deactive gameobject.
/// 
/// </summary>
public class AssistantItem : MonoBehaviour 
{

	/// <summary>
	/// Event for bonus eaten by player
	/// </summary>
	public delegate void EventBonusEaten();
	public EventBonusEaten Evt_BonusEaten;

	//reference to GameController
	//protected GameController gameController;

	/// <summary>
	/// The magnet target.
	/// Set target to make it move
	/// </summary>
	[System.NonSerialized]
	public GameObject magnetTarget;

	/// <summary>
	/// True this is magnetable item.
	/// </summary>
	[System.NonSerialized]
	public bool isMagnet = false;
	
	/// <summary>
	/// The max magnet speed.
	/// </summary>
	public float maxMagnetSpeed = 5f;
	
	/// <summary>
	/// The magnet speed.
	/// </summary>
	public float magnetSpeed = 5f;
	
	/// <summary>
	/// The initial speed.
	/// </summary>
	public float initialSpeed = 0f;
	
	/// <summary>
	/// The magnet current speed.
	/// </summary>
	protected float magnetCurrentSpeed = 0f;

	/// <summary>
	/// The sound player.
	/// </summary>
	protected SFXPlayer soundPlayer;

	protected virtual void Awake()
	{
		//find GameController
		//gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();

		//item is always a trigger
		GetComponent<Collider2D> ().isTrigger = true;

		//find SFXPlayer
		soundPlayer = GetComponent<SFXPlayer> ();

		if(soundPlayer == null)
		{
			soundPlayer = gameObject.AddComponent<SFXPlayer>();
		}
	}

	protected virtual void Update()
	{
		//if this assistant item is one of child in assistant item holder...return, this is not a magnet item
		if((transform.parent != null) && (transform.parent.GetComponent<AssistantItemHolder>() != null))
		{
			return;
		}

		//if this item has magnet target...move to target...if no target then recycle game object
		if(magnetTarget != null)
		{
			magnetCurrentSpeed += Mathf.Pow((magnetSpeed * Time.deltaTime), 2f);
			
			magnetCurrentSpeed = Mathf.Clamp(magnetCurrentSpeed, 0f, maxMagnetSpeed);
			
			Vector2 direction = magnetTarget.transform.ConvertPositionToVector2() - transform.ConvertPositionToVector2();
			direction = direction.normalized;
			
			Vector2 amount = direction * magnetCurrentSpeed;
			
			transform.position = new Vector3(transform.position.x + amount.x, transform.position.y + amount.y, transform.position.z);
		}
		else if(isMagnet)
		{
			//GameController.sharedGameController.objectPool.RecycleObject(gameObject);
			TrashMan.despawn(gameObject);
		}
		else
		{
			Debug.LogError("Assistant item: "+gameObject.name+" lost it's magnet target");
		}
	}

	protected virtual void OnEnable()
	{

	}

	protected virtual void OnDisable()
	{
		magnetCurrentSpeed = 0f;
		magnetTarget = null;
		isMagnet = false;
	}

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		if(isMagnet)
		{
			if(magnetTarget != null)
			{
				TrashMan.despawn(gameObject);
			}
		}
		else
		{
			if(Evt_BonusEaten != null)
			{
				Evt_BonusEaten();
			}
			
			gameObject.SetActive(false);
		}

		/*
		if((magnetTarget != null) && isMagnet && (transform.parent.GetComponent<AssistantItemHolder>() == null))
		{
			//GameController.sharedGameController.objectPool.RecycleObject(gameObject);
			TrashMan.despawn(gameObject);
		}
		else
		{
			if(Evt_BonusEaten != null)
			{
				Evt_BonusEaten();
			}
			
			gameObject.SetActive(false);

		}
		*/
	}

	/// <summary>
	/// Tell item to create same item but magnetable.
	/// The item responsible to create magnetable item
	/// then call BecomeMagnet to make item magnetable.
	/// </summary>
	/// <param name="target">Target.</param>
	public virtual void Magnet(GameObject target)
	{

	}

	/// <summary>
	/// Make this item magnetable.
	/// 
	/// Only call this method by the gameobject that create me.
	/// </summary>
	/// <param name="target">Target.</param>
	public virtual void BecomeMagnet(GameObject target)
	{
		magnetTarget = target;
		isMagnet = true;
	}
}
