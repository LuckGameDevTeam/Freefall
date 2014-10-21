using UnityEngine;
using System.Collections;

public class UIPauseControl : MonoBehaviour 
{
	public delegate void EventResumeClick(UIPauseControl pauseControl);
	/// <summary>
	/// Event when resume click.
	/// </summary>
	public EventResumeClick Evt_ResumeClick;

	public delegate void EventRestartClick(UIPauseControl pauseControl);
	/// <summary>
	/// Event when restart click.
	/// </summary>
	public EventRestartClick Evt_RestartClick;

	public delegate void EventExitClick(UIPauseControl pauseControl);
	/// <summary>
	/// Event when exit click.
	/// </summary>
	public EventExitClick Evt_ExitClick;

	public delegate void EventPauseMenuClose(UIPauseControl pauseControl);
	/// <summary>
	/// Event when pause menu close.
	/// </summary>
	public EventPauseMenuClose Evt_PauseMenuClose;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Shows the pause menu.
	/// </summary>
	public void ShowPauseMenu()
	{
		//set gameobject active
		gameObject.SetActive (true);
	}

	/// <summary>
	/// Closes the pause menu.
	/// </summary>
	public void ClosePauseMenu()
	{
		//set gameobject not active
		gameObject.SetActive (false);
	}

	/// <summary>
	/// Hides the pause menu.
	/// </summary>
	private void HidePauseMenu()
	{
		if(Evt_PauseMenuClose != null)
		{
			Evt_PauseMenuClose(this);
		}

		//set gameobject not active
		gameObject.SetActive (false);
	}

	/// <summary>
	/// Resume button click.
	/// </summary>
	public void Resume()
	{
		if(Evt_ResumeClick != null)
		{
			Evt_ResumeClick(this);
		}

		HidePauseMenu ();
	}

	/// <summary>
	/// Restart button click.
	/// </summary>
	public void Restart()
	{
		if(Evt_RestartClick != null)
		{
			Evt_RestartClick(this);
		}

		HidePauseMenu ();
	}

	/// <summary>
	/// Exit button click.
	/// </summary>
	public void Exit()
	{
		if(Evt_ExitClick != null)
		{
			Evt_ExitClick(this);
		}

		HidePauseMenu ();
	}
}
