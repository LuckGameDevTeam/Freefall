using UnityEngine;
using System.Collections;

/// <summary>
/// Mileage controller.
/// This class controller mileage and counting.
/// Recommend to attch this script to game controller
/// </summary>

public class MileageController : MonoBehaviour 
{
	/// <summary>
	/// Determine mileage is running or not
	/// false mileage will pause
	/// </summary>
	[System.NonSerialized]
	public bool isRunning = false;

	/// <summary>
	/// Event when a mile reached
	/// 
	/// <param name="reachedMileage">reached mileage</param>
	/// </summary>
	public delegate void EventMileReach(MileageController mc, int reachedMileage);
	public EventMileReach Evt_MileReach;

	/// <summary>
	/// Event when goal mileage reached
	/// 
	/// <param name="goalMileage">goal mileage</param>
	/// </summary>
	public delegate void EventGoalReach(MileageController mc, int goalMileage);
	public EventGoalReach Evt_MileGoalReach;

	/// <summary>
	/// Event for report current mile when mile reduce by one
	/// 
	///  <param name="currentMileage">current mileage</param>
	/// </summary>
	public delegate void EventReportMile(MileageController mc, int currentMileage);
	public EventReportMile Evt_ReportMile;

	/// <summary>
	/// The total mileage.
	/// Change this value to tell system
	/// how many mileage is going to fall.
	/// </summary>
	public int totalMileage = 100;

	/// <summary>
	/// The goal mileage.
	/// If current mile reach this value
	/// system will stop counting and fire
	/// event
	/// </summary>
	public int goalMileage = 0;

	/// <summary>
	/// The second per mile.
	/// How many in game second will reduce 1 mile.
	/// 
	/// e.g value is 2f then every 2 second in game
	/// will reduce 1 mile.
	/// </summary>
	public float secondPerMile = 1f;

	/// <summary>
	/// The mile reachs.
	/// A bunch of value that will be checked if 
	/// current mile match any of them and fire
	/// event
	/// </summary>
	public int[] mileReachs = null;
 

	/// <summary>
	/// current mile reach
	/// </summary>
	private int currentMileage = 0;

	/// <summary>
	/// determine system is counting or not
	/// </summary>
	private bool isCounting = false;

	/// <summary>
	/// Time for next counting process
	/// </summary>
	private float nextCountingTime = 0f;

	void Start()
	{
		//init current mileage
		currentMileage = totalMileage;
	}

	void Update()
	{
		if(!isRunning)
		{
			return;
		}

		if(isCounting)
		{
			//if current mileage reach goal mileage...otherwise counting mileage
			if(currentMileage == goalMileage)
			{
				//stop counting
				isCounting = false;

				//fire goal reach event
				if(Evt_MileGoalReach != null)
				{
					Evt_MileGoalReach(this, goalMileage);
				}
			}
			else
			{
				//if it is time to count
				if(Time.time > nextCountingTime)
				{
					//reduce 1 mile
					currentMileage--;

					//report current mile
					if(Evt_ReportMile != null)
					{
						Evt_ReportMile(this, currentMileage);
					}

					//check if any mile reached
					CheckMileReached(currentMileage);

					//set next counting time
					nextCountingTime = Time.time + secondPerMile;
				}
			}
		}
	}

	/// <summary>
	/// Start mile counting.
	/// Every time this is called MileageController will reset and start again
	/// </summary>
	public void StartCounting()
	{
		//set current mileage to total mileage
		currentMileage = totalMileage;

		//run mileage 
		isRunning = true;

		//set isCounting to true
		isCounting = true;
	}

	/// <summary>
	/// Check if any mile reached
	/// 
	/// <param name="curMile">current mile</param>
	/// </summary>
	void CheckMileReached(int curMile)
	{
		if(mileReachs.Length > 0)
		{
			for(int i=0; i<mileReachs.Length; i++)
			{
				int comparedMile = mileReachs[i];

				//if match
				if(curMile == comparedMile)
				{
					//fire event
					if(Evt_MileReach != null)
					{
						Evt_MileReach(this, comparedMile);
					}

					break;
				}
			}
		}
	}

	public int CurrentMile
	{
		get
		{
			return currentMileage;
		}
	}

}
