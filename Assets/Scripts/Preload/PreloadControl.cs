using UnityEngine;
using System.Collections;
using SIS;
using System;

public class PreloadControl : MonoBehaviour 
{
	/// <summary>
	/// The level to load.
	/// </summary>
	public string levelToLoad = "MainMenu";

	/// <summary>
	/// The logo display time.
	/// How long should logo display
	/// </summary>
	public float logoDisplayTime = 2.5f;

	// Use this for initialization
	void Start () 
	{
		PreloadData ();

		Invoke ("LoadNextScene", logoDisplayTime);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void LoadNextScene()
	{
		//load next scene
		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel (levelToLoad);
	}

	void PreloadData()
	{

		//test

		/*
		//////////clean all data///////////
		PlayerCharacter pcs = PlayerCharacter.Load ();
		pcs.characterName = "";
		PlayerCharacter.Save (pcs);
		PlayerPrefs.SetInt ("TimeOfLaunch", 0);
		DBManager.ClearAll ();
		DBManager.GetInstance ().Init ();
		//////////clean all data///////////
		*/

		//log player prefabs data that was stored by DBManager
		Debug.Log(PlayerPrefs.GetString("data"));

		int firstTimeLaunch = 0;
		
		firstTimeLaunch = PlayerPrefs.GetInt("TimeOfLaunch");
		
		if(firstTimeLaunch == 0)
		{
			/*
			//give player 3 life, test
			DBManager.SetPlayerData(LifeCounter.PlayerLife, new SimpleJSON.JSONData(100));

			//give BellCat
			DBManager.SetToPurchased("BellCat");

			//select it if need
			if(PlayerCharacter.Load().characterName == "")
			{

				DBManager.SetToSelected("BellCat", true);
				
				PlayerCharacter pc = new PlayerCharacter();
				pc.characterName = "BellCat";
				
				PlayerCharacter.Save(pc);
			}

			//Level 1 virtual product alreay bought
			//We only need to unlock level 1-1
			SubLevelData slData = SubLevelData.Load ();
			slData.UnlockSubLevel (1, 1);
			*/

			ReInitialData();
			
		}

		//increase time launch
		PlayerPrefs.SetInt ("TimeOfLaunch", firstTimeLaunch + 1);


		//Test
		DBManager.SetToPurchased ("UnlockLevel2");
		DBManager.SetToPurchased ("UnlockLevel3");
		//unlock level all sub levels
		for(int j=1; j<=3; j++)
		{
			for(int i=1; i<=5; i++)
			{
				SubLevelData sllData = SubLevelData.Load();
				
				sllData.UnlockSubLevel(j, i);
			}
		}

	}

	/// <summary>
	/// Re-initial data.
	/// Clean player data on device and set to default
	/// </summary>
	public static void ReInitialData()
	{
		DBManager.ClearAll ();

		DBManager.GetInstance ().Init ();

		//give player 3 life, test
		DBManager.SetPlayerData(LifeCounter.PlayerLife, new SimpleJSON.JSONData(100));

		//give BellCat
		DBManager.SetToPurchased("BellCat");

		DBManager.SetToSelected("BellCat", true);
		
		PlayerCharacter pc = PlayerCharacter.Load();
		pc.characterName = "BellCat";
		
		PlayerCharacter.Save(pc);

		SubLevelData slData = SubLevelData.Load ();
		slData.UnlockSubLevel (1, 1);
	}
}
