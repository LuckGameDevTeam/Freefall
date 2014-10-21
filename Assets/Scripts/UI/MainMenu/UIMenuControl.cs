using UnityEngine;
using System.Collections;

/// <summary>
/// UI menu control.
/// 
/// This class is main control in main menu
/// </summary>
public class UIMenuControl : MonoBehaviour 
{
	/// <summary>
	/// Reference to setting menu.
	/// </summary>
	public GameObject settingMenu;

	/// <summary>
	/// Reference to main menu
	/// </summary>
	public GameObject mainMenu;

	// Use this for initialization
	void Start () 
	{
		//disable setting menu
		settingMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Shows the setting menu.
	/// </summary>
	public void ShowSettingMenu()
	{
		mainMenu.SetActive (false);
		settingMenu.SetActive (true);
	}

	/// <summary>
	/// Hides the setting menu.
	/// </summary>
	public void HideSettingMenu()
	{
		settingMenu.SetActive (false);
		mainMenu.SetActive (true);
	}

	/// <summary>
	/// Gos to level selection.
	/// </summary>
	public void GoToLevelSelection()
	{
		//Application.LoadLevel("LevelSelection");

		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel ("LevelSelection");
	}
}
