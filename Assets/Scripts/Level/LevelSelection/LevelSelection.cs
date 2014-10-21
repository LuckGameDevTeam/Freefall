using UnityEngine;
using System.Collections;

/// <summary>
/// Level selection.
/// 
/// GameObject has this script must live forever.
/// 
/// This class remember which main level should present while
/// in level selection scene.
/// 
/// e.g after level1-2 complete then back to level selection, it will 
/// direct you to main level 1's sub level selection instead main level 
/// selection
/// </summary>
public class LevelSelection : MonoBehaviour 
{
	/// <summary>
	/// The mainlevel to selected.
	/// 
	/// set to value 0 mean no main level select
	/// </summary>
	private int mainlevelToSelected = 0;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Sets the main level selected.
	/// </summary>
	/// <param name="level">Level.</param>
	public void SetMainLevelSelected(int level)
	{
		mainlevelToSelected = level;
	}

	/// <summary>
	/// Gets the main level selected.
	/// </summary>
	/// <returns>The main level selected.</returns>
	public int GetMainLevelSelected()
	{
		return mainlevelToSelected;
	}
}
