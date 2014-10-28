using UnityEngine;
using System.Collections;

/// <summary>
/// UI menu control.
/// 
/// This class is main control in main menu
/// </summary>
public class UIMenuControl : MonoBehaviour 
{
	public string canNotLoginKey = "CanNotLoginFB";

	/// <summary>
	/// Reference to setting menu.
	/// </summary>
	public GameObject settingMenu;

	/// <summary>
	/// Reference to main menu
	/// </summary>
	public GameObject mainMenu;

	public UIAlertControl alertControl;

	private FBController fbController;

	void Awake()
	{
		fbController = GameObject.FindObjectOfType (typeof(FBController)) as FBController;
	}

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

	public void LoginFB()
	{
		//register event
		fbController.Evt_OnUserDataLoaded += OnUserDataLoaded;
		fbController.Evt_OnUserDataFailToLoad += OnUserDataFailToLoad;

		//login
		fbController.Login ();
	}

	#region FB controller event

	void OnUserDataLoaded(FBController controller, FacebookUserInfo userInfo)
	{
		//unregister event
		fbController.Evt_OnUserDataLoaded -= OnUserDataLoaded;
		fbController.Evt_OnUserDataFailToLoad -= OnUserDataFailToLoad;

		//go to level selection
		GoToLevelSelection ();
	}

	void OnUserDataFailToLoad(FBController controller)
	{
		//unregister event
		fbController.Evt_OnUserDataLoaded -= OnUserDataLoaded;
		fbController.Evt_OnUserDataFailToLoad -= OnUserDataFailToLoad;

		Debug.Log("FB login fail can not go to level selection");

		alertControl.ShowAlertWindow (null, canNotLoginKey);
	}

	#endregion FB controller event
}
