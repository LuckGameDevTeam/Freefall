using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// UI life counter time.
/// 
/// This class display player's life counter timer
/// </summary>
public class UILifeCounterTime : MonoBehaviour 
{
	/// <summary>
	/// The minute label.
	/// </summary>
	public UILabel MinuteLabel;

	/// <summary>
	/// The second label.
	/// </summary>
	public UILabel SecondLabel;

	/// <summary>
	///  Reference to LifeCounter.
	/// </summary>
	private LifeCounter lifeCounter;

	void Awake()
	{
		//find LifeCounter
		GameObject o = GameObject.FindGameObjectWithTag (Tags.lifeCounter);

		if(o == null)
		{
			Debug.LogError("Can't find LifeCounter from scene time will not display");
		}
		else
		{
			lifeCounter = o.GetComponent<LifeCounter> ();

			//register event
			lifeCounter.Evt_LifeFull += LifeCounterLifeFull;
			lifeCounter.Evt_LifeRegenTick += LifeCoutnerRegenTick;
		}



	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void LifeCounterLifeFull(LifeCounter counter)
	{
		MinuteLabel.text = "00";
		SecondLabel.text = "00";
	}

	void LifeCoutnerRegenTick(LifeCounter counter, TimeSpan timeLeft)
	{
		//deal with minute
		if(timeLeft.Minutes > 9)//double digitals
		{
			MinuteLabel.text = timeLeft.Minutes.ToString();
		}
		else if(timeLeft.Minutes < 10 && timeLeft.Minutes > 0)//single digital
		{
			MinuteLabel.text = "0"+timeLeft.Minutes.ToString();
		}
		else//is 0 minute
		{
			MinuteLabel.text = "00";
		}

		//deal with second
		if(timeLeft.Seconds > 9)
		{
			SecondLabel.text = timeLeft.Seconds.ToString();
		}
		else if(timeLeft.Seconds < 10 && timeLeft.Seconds > 0)
		{
			SecondLabel.text = "0"+timeLeft.Seconds.ToString();
		}
		else
		{
			MinuteLabel.text = "00";
		}
	}
}
