using UnityEngine;
using System.Collections;

/// <summary>
/// Level event.
/// 
/// This is generic class for all type of event
/// </summary>
public class LevelEvent : MonoBehaviour 
{
	/// <summary>
	/// The start mileage.
	/// At which mile this event will be triggered
	/// </summary>
	public int startMileage = 100;

	protected virtual void Awake()
	{

	}


	/// <summary>
	/// Triggers the event.
	/// </summary>
	public virtual void TriggerEvent()
	{
		gameObject.SetActive (true);
	}

	/// <summary>
	/// Stops the event.
	/// </summary>
	public virtual void StopEvent()
	{
		gameObject.SetActive (false);
	}
}
