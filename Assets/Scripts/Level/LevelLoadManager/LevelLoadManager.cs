using UnityEngine;
using System.Collections;

/// <summary>
/// Level load manager.
/// 
/// GameObject that has this script must live forever.
/// This class remember which level is going to load and
/// it load LoadingScene for loading transition.
/// </summary>
public class LevelLoadManager : MonoBehaviour 
{
	/// <summary>
	/// The level name to load.
	/// 
	/// Clear this value after you get this value
	/// </summary>
	[System.NonSerialized]
	public string levelToLoad = "";

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Loads the level.
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadLevel(string levelName)
	{
		if(levelName == "")
		{
			DebugEx.DebugError("Give level name to load");

			return;
		}

		levelToLoad = levelName;

		Time.timeScale = 1.0f;

		Application.LoadLevel("LoadingScene");
	}
}
