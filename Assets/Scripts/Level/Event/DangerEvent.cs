using UnityEngine;
using System.Collections;

public class DangerEvent : LevelEvent 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void TriggerEvent()
	{
		base.TriggerEvent ();

		GameController.sharedGameController.hudControl.dangerSignControl.PresentDangerSign ();
	}
	
	public override void StopEvent()
	{
		base.StopEvent ();
	}
}
