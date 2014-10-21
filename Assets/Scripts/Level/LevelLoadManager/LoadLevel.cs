using UnityEngine;
using System.Collections;

/// <summary>
/// Load level.
/// 
/// This class work with LevelLoadManager.
/// 
/// This class take level name that is going to load and
/// load the level.
/// </summary>
public class LoadLevel : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		Invoke ("BeginLoadLevel", 1f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void BeginLoadLevel()
	{
		//get level name
		string levelName = GameObject.FindGameObjectWithTag(Tags.levelLoadManager).GetComponent<LevelLoadManager>().levelToLoad;

		//clear level name
		GameObject.FindGameObjectWithTag(Tags.levelLoadManager).GetComponent<LevelLoadManager>().levelToLoad = "";

		//load level
		Application.LoadLevel(levelName);
	}
}
