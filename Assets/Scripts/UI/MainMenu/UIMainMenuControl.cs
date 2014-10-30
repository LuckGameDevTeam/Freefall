using UnityEngine;
using System.Collections;

/// <summary>
/// UI menu control.
/// 
/// This class is main control in main menu
/// </summary>
public class UIMainMenuControl : MonoBehaviour 
{
	public string canNotLoginKey = "CanNotLoginFB";

	/// <summary>
	/// Reference to setting menu.
	/// </summary>
	public UISettingControl settingControl;

	/// <summary>
	/// Reference to Rank
	/// </summary>
	public UIRankControl rankControl;

	/// <summary>
	/// Reference to main menu
	/// </summary>
	public UIMenuControl menuControl;

	public UIAlertControl alertControl;

	private FBController fbController;

	void Awake()
	{
		fbController = GameObject.FindObjectOfType (typeof(FBController)) as FBController;

		//register event
		settingControl.Evt_OnSettingClose += OnSettingClose;

		rankControl.Evt_OnRankClose += OnRankClose;

		menuControl.Evt_OnFBLoginClick += OnFBLoginClick;
		menuControl.Evt_OnSinglePlayerClick += OnSinglePlayerClick;
		menuControl.Evt_OnRankClick += OnRankClick;
		menuControl.Evt_OnTutorialClick += OnTutorialClick;
		menuControl.Evt_OnSettingClick += OnSettingClick;
		menuControl.Evt_OnCreditClick += OnCreditClick;

	}

	// Use this for initialization
	void Start () 
	{
		//hide setting
		settingControl.gameObject.SetActive (false);

		//hide rank 
		rankControl.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	#region UISettingControl callback
	void OnSettingClose(UISettingControl control)
	{
		menuControl.ShowMenu ();
	}
	#endregion UISettingControl callback

	#region UIRankControl callback
	void OnRankClose(UIRankControl control)
	{
		menuControl.ShowMenu ();
	}
	#endregion UIRankControl callback

	#region UIMenuControl callback
	void OnFBLoginClick(UIMenuControl control)
	{
		LoginFB ();
	}

	void OnSinglePlayerClick(UIMenuControl control)
	{
		GoToLevelSelection ();
	}

	void OnRankClick(UIMenuControl control)
	{
		rankControl.ShowRankWithRankType (RankType.FBRank);

		menuControl.CloseMenu ();
	}

	void OnTutorialClick (UIMenuControl control)
	{
		Debug.Log("show tutorial");
	}

	void OnSettingClick(UIMenuControl control)
	{
		settingControl.ShowSetting ();

		menuControl.CloseMenu ();
	}

	void OnCreditClick(UIMenuControl control)
	{
		Debug.Log("hide tutorial");
	}
	#endregion #region UIMenuControl callback

	#region Internal
	/// <summary>
	/// Gos to level selection.
	/// </summary>
	void GoToLevelSelection()
	{
		//Application.LoadLevel("LevelSelection");

		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel ("LevelSelection");
	}

	void LoginFB()
	{
		//register event
		fbController.Evt_OnUserDataLoaded += OnUserDataLoaded;
		fbController.Evt_OnUserDataFailToLoad += OnUserDataFailToLoad;

		//login
		fbController.Login ();
	}
	#endregion Internal

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
