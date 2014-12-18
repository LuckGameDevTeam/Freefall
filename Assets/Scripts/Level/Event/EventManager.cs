using UnityEngine;
using System.Collections;

/// <summary>
/// Event manager.
/// This class handle trigger by compare event's trigger mileage.
/// EventManager hold all events
/// 
/// It require MileageController to attach at same gameobject to work with
/// 
/// Attach this script to game controller
/// </summary>

[RequireComponent (typeof(MileageController))]

public class EventManager : MonoBehaviour 
{
	/// <summary>
	/// Determine manager is running or not
	/// false manager will pause
	/// </summary>
	[System.NonSerialized]
	public bool isRunning = false;

	/// <summary>
	/// determine EventManager is enabled or not
	/// </summary>
	private bool managerEnabled = false;

	/// <summary>
	/// event group that contain all events
	/// </summary>
	private GameObject eventGroup;

	/// <summary>
	/// all events in event group
	/// </summary>
	private LevelEvent[] events;

	/// <summary>
	/// current event index
	/// </summary>
	private int currentEventIndex = 0;

	/// <summary>
	/// reference to MileageController
	/// </summary>
	private MileageController mileageController;

	/// <summary>
	/// reference to GameController
	/// </summary>
	//private GameController gameController;

	void Awake()
	{
		//find game controller
		//gameController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<GameController> ();

		//find mileage controller
		mileageController = GetComponent<MileageController> ();

		//find event group
		eventGroup = GameObject.FindGameObjectWithTag (Tags.eventGroup) as GameObject;

		//find out all events in event group
		if(eventGroup)
		{
			//if event group has event
			if(eventGroup.transform.childCount > 0)
			{
				//create event array
				events = new LevelEvent[eventGroup.transform.childCount];

				for(int i=0; i<eventGroup.transform.childCount; i++)
				{
					//get event gameobject
					GameObject eventObject = eventGroup.transform.GetChild(i).gameObject;

					//store LevelEvent script
					events[i] = eventObject.GetComponent<LevelEvent>();

					//stop event at beginning
					events[i].StopEvent();
				}
			}
		}
		else
		{
			DebugEx.DebugError("EventManager can not find EventGroup gameobject");
		}
	}

	void OnEnable()
	{
		//register event to MileageController
		mileageController.Evt_ReportMile += EventCurrentMile;
	}

	void OnDisable()
	{
		//unregister event to MileageController
		if(mileageController)
		{
			mileageController.Evt_ReportMile -= EventCurrentMile;
		}

	}

	void Start()
	{

		//sort event
		if(events.Length > 0)
		{
			//sort event by start mile 
			System.Array.Sort(events, (ev1, ev2) => {
			
				LevelEvent le1 = ev1;
				LevelEvent le2 = ev2;

				if(le1.startMileage < le2.startMileage)
				{
					return 1;
				}
				else if(le1.startMileage > le2.startMileage)
				{
					return -1;
				}
				else
				{
					return 0;
				}
			});
		}


	}

	/// <summary>
	/// Tell manager to start running
	/// Every time this is called manager will reset and start again
	/// </summary>
	public void StartEventManager()
	{
		//enable manager
		managerEnabled = true;

		//run manager
		isRunning = true;

		//set current event index
		currentEventIndex = 0;
	}

	/// <summary>
	/// Stops all events.
	/// </summary>
	public void StopAllEvents()
	{
		//stop all event
		foreach(LevelEvent e in events)
		{
			e.StopEvent();
		}
	}

	/// <summary>
	/// Handle event that current mile report
	/// </summary>
	/// <param name="mc">Mc.</param>
	/// <param name="currentMileage">Current mileage.</param>
	void EventCurrentMile(MileageController mc, int currentMileage)
	{
		if(!isRunning)
		{
			return;
		}


		//compare current event's start mileage to current mileage from MileageController
		//if match then trigger event, compare event until it is not the same
		while((currentEventIndex < events.Length) && (events[currentEventIndex].startMileage == currentMileage))
		{
			//trigger event
			events[currentEventIndex].TriggerEvent();
			
			//increas current event index
			currentEventIndex++;
			
		}

	}

}
