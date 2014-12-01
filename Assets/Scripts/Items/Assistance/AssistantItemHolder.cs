using UnityEngine;
using System.Collections;

/// <summary>
/// Assistant item holder.
/// 
/// This class hold all child assistant items such Big Cat Coine, Cat Crown, Small Cat Cookie.
/// All assistant items must be child gameobject of this gameobject.
/// 
/// The holder responsible to move it self so when it move all child move as well. 
/// The holder also responsible to recycle it self when all child is eaten or out of screen.
/// 
/// The holder will send magnet event to child when isMagnet is marked true. The holder decide
/// which child suppose to receive event then send magnet event to child.
/// 
/// 
/// </summary>
public class AssistantItemHolder : MonoBehaviour 
{
	/// <summary>
	/// How many child bonus this holder have
	/// value will be change by it self
	/// </summary>
	[System.NonSerialized]
	public int childItemCount = 0;

	/// <summary>
	/// Moving speed
	/// </summary>
	public float speed = 3f;

	/// <summary>
	/// True certain child item will become
	/// magnetable to character
	/// </summary>
	[System.NonSerialized]
	public bool isMagnet = false;

	/// <summary>
	/// hold bounch of bonus child
	/// </summary>
	private GameObject[] childItems;

	/// <summary>
	/// hold last child bonus
	/// </summary>
	private GameObject lastItem;

	/// <summary>
	/// hold first child bonus
	/// </summary>
	private GameObject firstItem;

	/// <summary>
	/// Reference to GameController
	/// </summary>
	//private GameController gameController;

	void Awake()
	{
		//find GameController
		//gameController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<GameController> ();

		childItems = new GameObject[transform.childCount];

		//reference to all child and register event
		for(int i=0; i<transform.childCount; i++)
		{
			childItems[i] = transform.GetChild(i).gameObject;

			AssistantItem bChild = childItems[i].GetComponent<AssistantItem>();
			
			//register event
			bChild.Evt_BonusEaten += EventBonusEaten;
		}
	}

	void Start()
	{
		//find out last child bonuse by it's height
		if(childItems.Length > 0)
		{
			float lastHeight = childItems[0].transform.position.y - childItems[0].renderer.bounds.extents.y;

			foreach(GameObject c in childItems)
			{
				SpriteRenderer renderer = c.GetComponent<SpriteRenderer>();

				if((c.transform.position.y - renderer.bounds.extents.y) <= lastHeight)
				{
					lastItem = c;

					lastHeight = c.transform.position.y - c.renderer.bounds.extents.y;
				}
			}
		}

		//find out first child bonus by it's height
		if(childItems.Length > 0)
		{
			float mostHeight = childItems[0].transform.position.y + childItems[0].renderer.bounds.extents.y;
			
			foreach(GameObject c in childItems)
			{
				SpriteRenderer renderer = c.GetComponent<SpriteRenderer>();
				
				if((c.transform.position.y + renderer.bounds.extents.y) >= mostHeight)
				{
					firstItem = c;
					
					mostHeight = c.transform.position.y + c.renderer.bounds.extents.y;
				}
			}
		}


	}

	void OnEnable()
	{
		/*
		childItems = new GameObject[transform.childCount];

		for(int i=0; i<transform.childCount; i++)
		{
			GameObject child = transform.GetChild(i).gameObject;

			child.SetActive(true);

			childItems[i] = child;

			AssistantItem bChild = child.GetComponent<AssistantItem>();

			//register event
			bChild.Evt_BonusEaten += EventBonusEaten;

			//increase child bonus count
			childItemCount++;
		}
		*/

		/*
		for(int i=0; i<childItems.Length; i++)
		{
			childItems[i].SetActive(true);

			AssistantItem bChild = childItems[i].GetComponent<AssistantItem>();

			//register event
			bChild.Evt_BonusEaten += EventBonusEaten;
			
			//increase child bonus count
			childItemCount++;
		}
		*/
	}

	void OnDisable()
	{
		if(childItems.Length > 0)
		{
			for(int i=0; i<childItems.Length; i++)
			{
				//enable child that was eaten and disabled
				childItems[i].SetActive(true);
				/*
				if(childItems[i] != null)
				{
					AssistantItem bChild = childItems[i].GetComponent<AssistantItem>();
					
					//unregister event
					bChild.Evt_BonusEaten -= EventBonusEaten;
				}
				*/

			}
		}

		//set child bonus count to 0
		//childItemCount = 0;

		childItemCount = childItems.Length;

		isMagnet = false;
	}

	void Update()
	{
		//if holder's childs become magnet
		if(isMagnet)
		{
			for(int i=0; i<childItems.Length; i++)
			{
				GameObject child = childItems[i];
				
				//child is active
				if(child.activeInHierarchy)
				{
					//if child is in screen
					if((child.transform.position.y - child.renderer.bounds.extents.y) >= Camera.main.GetBottomBorderWorldSpace(transform.position.z) &&
					   (child.transform.position.y + child.renderer.bounds.extents.y) <= Camera.main.GetTopBorderWorldSpace(transform.position.z))
					{
						//tell child it is magnet
						child.GetComponent<AssistantItem>().Magnet(GameController.sharedGameController.character);
					}
				}
			}
		}

		//if all child been eaten or out of top of screen... recycled...otherwise move
		if((childItemCount <= 0) || 
		   ((lastItem.transform.position.y - lastItem.renderer.bounds.extents.y) > Camera.main.GetTopBorderWorldSpace(transform.position.z)))
		{
			//GameController.sharedGameController.objectPool.RecycleObject(gameObject);
			TrashMan.despawn(gameObject);
		}

		else
		{
			//only move on y axis and move upward
			Vector2 direction = new Vector2(0f, 1f);

			//calculate amout of movement
			Vector2 amount = direction * (speed * Time.deltaTime);

			//move holder
			transform.position = new Vector3(transform.position.x, transform.position.y + amount.y, transform.position.z);
		}
	}

	/// <summary>
	/// Get the half of height
	/// Take child bonus in to count
	/// </summary>
	/// <returns>The half height.</returns>
	public float GetHalfHeight()
	{
		if(firstItem)
		{
			float retVal = Mathf.Abs((firstItem.transform.position.y + firstItem.renderer.bounds.extents.y) - transform.position.y);

			return retVal;
		}
		else
		{
			return 0f;
		}
	}

	public void GameRestart()
	{
		TrashMan.despawn (gameObject);
	}

	void EventBonusEaten()
	{
		childItemCount--;
	}
}
