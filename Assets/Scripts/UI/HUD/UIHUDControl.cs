using UnityEngine;
using System.Collections;

/// <summary>
/// UI HUD control.
/// 
/// This class is HUD control
/// 
/// HUD hold all reference to each independent UI
/// </summary>
public class UIHUDControl : MonoBehaviour 
{
	public delegate void EventOnPauseButtonClick(UIHUDControl control);
	/// <summary>
	/// Event when puase button click from pause menu.
	/// </summary>
	public EventOnPauseButtonClick Evt_OnPuaseButtonClick;

	/// <summary>
	/// The pause button.
	/// </summary>
	public UIButton pauseButton;

	/// <summary>
	/// The mileage label.
	/// </summary>
	public UILabel mileageLabel;

	/// <summary>
	/// The fish bone label.
	/// </summary>
	public UILabel fishBoneLabel;

	/// <summary>
	/// The fish bone tweener.
	/// </summary>
	public UITweener fishBoneTweener;

	/// <summary>
	/// The coin label.
	/// </summary>
	public UILabel coinLabel;

	/// <summary>
	/// The coin tweener.
	/// </summary>
	public UITweener coinTweener;

	/// <summary>
	/// The mileage line control.
	/// </summary>
	public UIMileageLineControl mileageLineControl;

	/// <summary>
	/// The danger sign control.
	/// </summary>
	public UIDangerControl dangerSignControl;

	/// <summary>
	/// The ability control.
	/// </summary>
	public UIAbilityControl abilityControl;

	/// <summary>
	/// The result control.
	/// </summary>
	public UIResultControl resultControl;

	/// <summary>
	/// The alert control.
	/// </summary>
	public UIAlertControl alertControl;

	/// <summary>
	/// The in game store.
	/// </summary>
	public UIInGameStore inGameStore;

	/// <summary>
	/// The portriat sprite.
	/// </summary>
	public UISprite portriatSprite;

	/// <summary>
	/// The health display.
	/// </summary>
	public UIHealthDisplay healthDisplay;

	/// <summary>
	/// Determine pause menu is display or not.
	/// </summary>
	private bool showingPauseMenu = false;

	/// <summary>
	/// The pause control.
	/// 
	/// Pause menu
	/// </summary>
	public UIPauseControl pauseControl;

	private int maxFishBone = 0;

	void Awake()
	{
		//register event for pause menu close
		pauseControl.Evt_PauseMenuClose += OnPauseMenuClose;
	}

	// Use this for initialization
	void Start () 
	{
		InitHUD ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Init HUD.
	/// </summary>
	private void InitHUD()
	{
		//close pause menu 
		pauseControl.ClosePauseMenu ();

		//close result 
		resultControl.CloseResult ();

		//inGameStore.CloseInGameStore ();
		
		PlayerCharacter pc = PlayerCharacter.Load ();

		//set character portriat sprite name
		portriatSprite.spriteName = pc.characterName+"_Head";

		maxFishBone = GameController.sharedGameController.maxStarCount;

		UpdateFishBoneHUD (0);
	}

	///////////////////////////////////////////////////////////////Update HUD////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Update mileage HUD
	/// </summary>
	public void UpdateMileageHUD(int mile)
	{
		//set mileage label
		mileageLabel.text = mile.ToString ();
	}

	/// <summary>
	/// Update health HUD
	/// </summary>
	public void UpdateHealthHUD (float health)
	{
		//update health display
		healthDisplay.UpdateHealthDispaly (health);
	}

	/// <summary>
	/// Updates the fish bone HUD.
	/// </summary>
	/// <param name="value">Value.</param>
	public void UpdateFishBoneHUD(int value)
	{
		//player animation
		fishBoneTweener.Play (true);

		//set fish bone label
		fishBoneLabel.text = value.ToString ()+"/"+maxFishBone;
	}

	/// <summary>
	/// Stops the finsh bone tweener.
	/// </summary>
	public void StopFinshBoneTweener()
	{
		//reverse animatoin
		fishBoneTweener.Play (false);
	}

	/// <summary>
	/// Updates the coin HUD.
	/// </summary>
	/// <param name="value">Value.</param>
	public void UpdateCoinHUD(int value)
	{
		//play animation
		coinTweener.Play (true);

		//set coin label
		coinLabel.text = value.ToString ();
	}

	/// <summary>
	/// Stops the coin tweener.
	/// </summary>
	public void StopCoinTweener()
	{
		//reverse animatoin
		coinTweener.Play (false);
	}

	///////////////////////////////////////////////////////////////Update HUD////////////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Reset HUD
	/// </summary>
	public void ResetHUD()
	{
		//set mileage label to 0
		mileageLabel.text = "0";

		//set coin label to 0
		coinLabel.text = "0";

		//set fish bone label to 0
		fishBoneLabel.text = "0";

		//reset mileage line control
		mileageLineControl.Reset ();

		//reset health display 
		healthDisplay.Reset ();

		//reset danger sign
		dangerSignControl.Reset ();

		//reset ability control
		abilityControl.Reset ();

		//reset result control
		resultControl.CloseResult ();

		//reset in game store
		inGameStore.CloseInGameStore ();

		//init HUD
		InitHUD ();
	}

	/// <summary>
	/// Pause button click
	/// </summary>
	public void PauseButtonClick()
	{
		if(Evt_OnPuaseButtonClick != null)
		{
			Evt_OnPuaseButtonClick(this);
		}

		//set showing pause menu flag to true
		showingPauseMenu = true;
		
		//disable pause button
		pauseButton.isEnabled = false;
		
		//show pause menu
		pauseControl.ShowPauseMenu ();
	}

	/// <summary>
	/// Event handle pause menu close
	/// </summary>
	/// <param name="control">Control.</param>
	void OnPauseMenuClose(UIPauseControl control)
	{
		//set showing pause menu flag to false
		showingPauseMenu = false;
		
		//disable pause button
		pauseButton.isEnabled = true;
	}
}
