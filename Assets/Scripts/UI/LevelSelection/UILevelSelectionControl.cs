using UnityEngine;
using System.Collections;

/// <summary>
/// UI level selection control.
/// 
/// This class control between main level and sub level selection
/// </summary>
public class UILevelSelectionControl : MonoBehaviour 
{
	/// <summary>
	/// The level selection.
	/// </summary>
	public GameObject levelSelection;

	/// <summary>
	/// The sub level selection.
	/// </summary>
	public GameObject subLevelSelection;

	public UIPurchaseControl purchaseControl;

	// Use this for initialization
	void Start () 
	{
		subLevelSelection.SetActive (false);
		levelSelection.SetActive (true);
		//purchaseControl.ClosePurchaseWindow ();

		//find out which level should present
		int selectedLevel = GameObject.FindGameObjectWithTag (Tags.levelSelection).GetComponent<LevelSelection> ().GetMainLevelSelected ();
		if(selectedLevel != 0)
		{
			ShowSubLevelSelection(selectedLevel);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Gos to main menu.
	/// </summary>
	public void GoToMainMenu()
	{
		//Application.LoadLevel("MainMenu");
		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel ("MainMenu");
	}

	/// <summary>
	/// Shows the sub level selection.
	/// </summary>
	/// <param name="mainLevel">Main level.</param>
	public void ShowSubLevelSelection(int mainLevel)
	{
		levelSelection.SetActive (false);
		subLevelSelection.SetActive (true);

		UISubLevelSelectionControl control = subLevelSelection.GetComponent<UISubLevelSelectionControl> ();
		control.selectedSubLevel = 0;
		control.UpdateUIWithMainLevel (mainLevel);

	}

	/// <summary>
	/// Hides the sub level selection.
	/// </summary>
	public void HideSubLevelSelection()
	{
		UISubLevelSelectionControl control = subLevelSelection.GetComponent<UISubLevelSelectionControl> ();
		control.selectedSubLevel = 0;

		subLevelSelection.SetActive (false);
		levelSelection.SetActive (true);
	}

}
