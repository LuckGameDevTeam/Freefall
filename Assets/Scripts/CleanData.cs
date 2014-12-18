using UnityEngine;
using System.Collections;
using SIS;

public class CleanData : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoCleanData()
	{
		PlayerCharacter pcs = PlayerCharacter.Load ();
		pcs.characterName = "";
		PlayerEquippedItems.Load ().UnEquipAllItems ();
		PlayerCharacter.Save (pcs);
		PlayerPrefs.SetInt ("TimeOfLaunch", 0);
		DBManager.ClearAll ();
		DBManager.GetInstance ().Init ();

		DBManager.SetPlayerData(LifeCounter.PlayerLife, new SimpleJSON.JSONData(1));
	}
}
