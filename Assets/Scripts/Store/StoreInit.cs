using UnityEngine;
using System.Collections;

/// <summary>
/// Store init.
/// 
/// 
/// </summary>
public class StoreInit : MonoBehaviour 
{
//	/// <summary>
//	/// The level to load.
//	/// </summary>
//	public string levelToLoad = "MainMenu";
//
//	/// <summary>
//	/// The no store.
//	/// </summary>
//	public bool noStore = false;
//
//	void Awake()
//	{
//#if TestMode
//		//in test mode there is no store
//		noStore = true;
//#endif
//	}
//	// Use this for initialization
//	void Start () 
//	{
//		if(!noStore)
//		{
//			StoreControl.SharedStoreControl.Evt_StoreInitComplete += LoadLevel;
//		}
//		else
//		{
//			GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel (levelToLoad);
//		}
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//	
//	}
//
//	void LoadLevel()
//	{
//		//Application.LoadLevel(levelToLoad);
//
//		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel (levelToLoad);
//	}
}
