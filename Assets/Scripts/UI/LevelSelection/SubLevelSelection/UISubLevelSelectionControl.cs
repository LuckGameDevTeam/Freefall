﻿using UnityEngine;
using System.Collections;
using SIS;

/// <summary>
/// User interface sub level selection control.
/// 
/// This class control all sub level tiem
/// </summary>
public class UISubLevelSelectionControl : MonoBehaviour 
{
	public delegate void EventUpdateUIWithMainLevel(UISubLevelSelectionControl control, int currentMainLevel);
	/// <summary>
	/// Event update UI with main level.
	/// </summary>
	public EventUpdateUIWithMainLevel Evt_UpdateUIWithMainLevel;

	public string selectLevelKey = "SelectLevel";
	public string selectLevelDescKey = "SelectLevelDesc";
	public string notEnoughLifeKey = "NotEnoughLife";
	public string notEnoughLifeDesc = "NotEnoughLifeDesc";

	/// <summary>
	/// The selected sub level.
	/// 
	/// 0 mean no level select
	/// </summary>
	public int selectedSubLevel = 0;

	/// <summary>
	/// The current main level.
	/// </summary>
	private int currentMainLevel = 1;

	/// <summary>
	/// The alert control.
	/// </summary>
	public UIAlertControl alertControl;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Updates the user interface with level.
	/// </summary>
	/// <param name="level">Level.</param>
	public void UpdateUIWithMainLevel(int level)
	{
		//set current main level
		currentMainLevel = level;

		//reset selected sub level to 0
		selectedSubLevel = 0;

		LevelManager.SharedLevelManager.SyncWithMainLevel (currentMainLevel);

		//fire update ui event
		if(Evt_UpdateUIWithMainLevel != null)
		{
			Evt_UpdateUIWithMainLevel(this, currentMainLevel);
		}
	}

	/// <summary>
	/// Loads the game level.
	/// </summary>
	public void LoadGameLevel()
	{
		//check if player has life left otherwise show alert window
		//if(StoreInventory.GetItemBalance(StoreAssets.PLAYER_LIFE_ITEM_ID) <= 0)
		if(DBManager.GetPlayerData(LifeCounter.PlayerLife).AsInt <= 0)
		{
			alertControl.ShowAlertWindow(notEnoughLifeKey, notEnoughLifeDesc);

			return;
		}

		//check if player has level select
		if(selectedSubLevel != 0)
		{
			string levelToLoad = "Level" + currentMainLevel + "-" + selectedSubLevel;
			DebugEx.Debug ("Load game level: " + levelToLoad);

			//take player 1 life
			//StoreInventory.TakeItem(StoreAssets.PLAYER_LIFE_ITEM_ID, 1);
			DBManager.SetPlayerData(LifeCounter.PlayerLife, new SimpleJSON.JSONData(DBManager.GetPlayerData(LifeCounter.PlayerLife).AsInt-1));

#if TestMode
			if(GameObject.FindObjectOfType(typeof(LevelLoadManager)))
			{
				(GameObject.FindObjectOfType(typeof(LevelLoadManager)) as LevelLoadManager).LoadLevel("TestField");
			}
			else
			{
				Application.LoadLevel("TestField");
			}

#else
			//Application.LoadLevel(levelToLoad);
			GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel (levelToLoad);
#endif
		}
		else
		{
			DebugEx.Debug ("Select game level");

			alertControl.ShowAlertWindow(selectLevelKey, selectLevelDescKey);
		}

	}
}
