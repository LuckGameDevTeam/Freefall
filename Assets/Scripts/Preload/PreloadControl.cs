using UnityEngine;
using System.Collections;
using SIS;

public class PreloadControl : MonoBehaviour 
{
	/// <summary>
	/// The level to load.
	/// </summary>
	public string levelToLoad = "MainMenu";

	// Use this for initialization
	void Start () 
	{
		PreloadData ();

		//load next scene
		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel (levelToLoad);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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

		int firstTimeLaunch = 0;
		
		firstTimeLaunch = PlayerPrefs.GetInt("TimeOfLaunch");
		
		if(firstTimeLaunch == 0)
		{
			//give player 3 life
			DBManager.SetPlayerData(LifeCounter.PlayerLife, new SimpleJSON.JSONData(1));

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
			
		}

		//increase time launch
		PlayerPrefs.SetInt ("TimeOfLaunch", firstTimeLaunch + 1);

		
		/*
		//unlock level 1's all sub levels
		for(int i=1; i<=5; i++)
		{
			SubLevelData slData = SubLevelData.Load();

			slData.UnlockSubLevel(1, i);
		}
		*/

		//Level 1 virtual product alreay bought
		//We only need to unlock level 1-1
		SubLevelData slData = SubLevelData.Load ();
		slData.UnlockSubLevel (1, 1);

	}
}
