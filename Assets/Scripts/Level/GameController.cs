using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

/// <summary>
/// Game controller.
/// 
/// Class is used to control game
/// </summary>
[RequireComponent (typeof(ObjectPool))]
public class GameController : MonoBehaviour 
{
	/// <summary>
	/// Singleton GameController
	/// </summary>
	[System.NonSerialized]
	public static GameController sharedGameController;

	public string notEnoughLifeKey = "NotEnoughLife";
	public string notEnoughLifeDesc = "NotEnoughLifeDesc";

	/// <summary>
	/// The main level index
	/// 
	/// start from 1
	/// 
	/// define current main level
	/// </summary>
	public int currentMainLevel = 1;

	/// <summary>
	/// The sub level index
	/// 
	/// start from 1
	/// 
	/// define current sub level 
	/// </summary>
	public int currentSubLevel = 1;

	/// <summary>
	/// The next main level to unlock.
	/// 
	/// Indicate the main level that will be unlock
	/// 
	/// give 0 no level will be unlock
	/// </summary>
	public int nextMainLevelToUnlock = 0;

	/// <summary>
	/// The next sub level to unlock.
	/// 
	/// Indicate the sub level that will be unlock
	/// 
	/// give 0 no level will be unlock
	/// </summary>
	public int nextSubLevelToUnlock = 0;

	/// <summary>
	/// The max fish bone in this level
	/// This will be used to calculate final score
	/// The max start count.
	/// </summary>
	public int maxStartCount = 100;

	/// <summary>
	/// how many fish bone character eat
	/// this is for fish bone
	/// </summary>
	private int starCount = 0;

	/// <summary>
	/// The invulnerable per star count.
	/// How many star count each time to give
	/// character invulnerable ability
	/// </summary>
	public int invulnerablePerStarCount = 500;

	/// <summary>
	/// The invulnerable ability prefab.
	/// Assign the invulnerable ability prefab to give character.
	/// 
	/// When character receive certain Star count every time
	/// character will have invulnerable ability.
	/// </summary>
	public GameObject invulnerableAbilityPrefab;

	/// <summary>
	/// how many coin character eat
	/// </summary>
	private int coinCount = 0;

	/// <summary>
	/// character gameobject
	/// </summary>
	[System.NonSerialized]
	public GameObject character;
	
	/// <summary>
	/// background gameobject
	/// </summary>
	private GameObject[] backgrounds;

	/// <summary>
	/// reference to MileageController
	/// </summary>
	private MileageController mileController;

	/// <summary>
	/// reference to EventManager
	/// </summary>
	private EventManager eventManager;

	/// <summary>
	/// spawner that spawn obstacles
	/// </summary>
	private GameObject[] spawners;

	/// <summary>
	/// The object pool manager.
	/// </summary>
	[System.NonSerialized]
	public ObjectPool objectPool;

	/// <summary>
	/// Reference to UIHUDControl
	/// </summary>
	public UIHUDControl hudControl;
	
	void Awake()
	{
		if(currentMainLevel == 0)
		{
			Debug.LogError("You can not assigned 0 to main level in GameController");
		}

		if(currentSubLevel == 0)
		{
			Debug.LogError("You can not assigend 0 to sub level in GameController");
		}

		if(maxStartCount == 0)
		{
			Debug.LogError("You can not assigend 0 to max start count, at least 1");
		}

		if((nextMainLevelToUnlock == currentMainLevel) && (nextSubLevelToUnlock == currentSubLevel))
		{
			Debug.LogError("next main&sub level to unlock is as same as curren main&sub level");
		}

		if(hudControl == null)
		{
			Debug.LogError("You did not assign UIHUDControl");
		}

		//store instance
		sharedGameController = this;

		Time.timeScale = 1f;

		//find scrollable background
		backgrounds = GameObject.FindGameObjectsWithTag (Tags.scrollingBackground);

		//find mile controller
		mileController = GetComponent<MileageController> ();

		//find event manager
		eventManager = GetComponent<EventManager> ();

		//find object pool
		objectPool = GetComponent<ObjectPool> ();

		//find all obstacle spawners
		spawners = GameObject.FindGameObjectsWithTag (Tags.obstacleSpawner);

	}

	void Start()
	{

		//find character
		//character = GameObject.FindGameObjectWithTag (Tags.player) as GameObject;
		character = LoadCharacter ();

		//disable input manager
		InputManager inputMgr = character.GetComponent<InputManager> ();
		inputMgr.inputManagerEnabled = false;

	}

	void OnEnable()
	{


		//register MileageController event
		mileController.Evt_MileGoalReach += EventGoalMileReached;
		mileController.Evt_MileReach += EventMileReached;
		mileController.Evt_ReportMile += EventCurrentMile;

		//register HUD event
		hudControl.Evt_OnPuaseButtonClick += OnHUDPauseClick;

		//puase menu event
		hudControl.pauseControl.Evt_ResumeClick += OnPauseMenuResumeClick;
		hudControl.pauseControl.Evt_RestartClick += OnPauseMenuRestartClick;
		hudControl.pauseControl.Evt_ExitClick += OnPauseMenuExitClick;

		//ability button press event
		hudControl.abilityControl.Evt_AbilityButtonPress += OnAbilityButtonPress;

		//result menu button event
		hudControl.resultControl.Evt_ConfirmButtonClick += OnResultConfirmButtonClick;
		hudControl.resultControl.Evt_RestartButtonClick += OnResultRestartButtonClick;

		//alert window event
		hudControl.alertControl.Evt_CloseAlertWindow += OnAlertWindowClose;

	}

	void OnDisable()
	{
		if (character) 
		{
			//unregister character dead event
			CharacterControl chaControl = character.GetComponent<CharacterControl> ();
			chaControl.Evt_CharacterDeadFinished -= EventCharacterDeadFinished;
			chaControl.Evt_CharacterVictoryFinished -= EventCharacterVictoryFinished;
			chaControl.Evt_CharacterDead -= EventCharacterDead;
			chaControl.Evt_CharacterVictory -= EventCharacterVictory;

			chaControl.transform.GetComponent<CharacterHealth> ().Evt_HealthChanged -= OnCharacterHealthChanged;
		}

		if(mileController)
		{
			//unregister MileageController event
			mileController.Evt_MileGoalReach -= EventGoalMileReached;
			mileController.Evt_MileReach -= EventMileReached;
			mileController.Evt_ReportMile -= EventCurrentMile;
		}

		if(hudControl)
		{
			//register HUD event
			hudControl.Evt_OnPuaseButtonClick -= OnHUDPauseClick;
			
			//puase menu event
			hudControl.pauseControl.Evt_ResumeClick -= OnPauseMenuResumeClick;
			hudControl.pauseControl.Evt_RestartClick -= OnPauseMenuRestartClick;
			hudControl.pauseControl.Evt_ExitClick -= OnPauseMenuExitClick;

			//ability button press event
			hudControl.abilityControl.Evt_AbilityButtonPress -= OnAbilityButtonPress;

			//result menu button event
			hudControl.resultControl.Evt_ConfirmButtonClick -= OnResultConfirmButtonClick;
			hudControl.resultControl.Evt_RestartButtonClick -= OnResultRestartButtonClick;

			//alert window event
			hudControl.alertControl.Evt_CloseAlertWindow -= OnAlertWindowClose;
		}

		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	void Update () 
	{

	}

	////////////////////////////////Internal////////////////////////////////

	/// <summary>
	/// Load character from persistant save data
	/// </summary>
	GameObject LoadCharacter()
	{
		PlayerCharacter pc = SaveLoadManager.SharedManager.Load<PlayerCharacter> ();

		if(pc != null)
		{
			Object[] characterAssets = Resources.LoadAll("Characters", typeof(GameObject));

			if(characterAssets.Length > 0)
			{
				for(int i = 0; i<characterAssets.Length; i++)
				{
					if(pc.characterName == characterAssets[i].name)
					{
						GameObject cPrefab = (GameObject)characterAssets[i];
						GameObject retCharacter = Instantiate(cPrefab) as GameObject;

						retCharacter.name = cPrefab.name;

						//register character dead event
						CharacterControl chaControl = retCharacter.GetComponent<CharacterControl> ();
						chaControl.Evt_CharacterDeadFinished += EventCharacterDeadFinished;
						chaControl.Evt_CharacterVictoryFinished += EventCharacterVictoryFinished;
						chaControl.Evt_CharacterDead += EventCharacterDead;
						chaControl.Evt_CharacterVictory += EventCharacterVictory;
						
						chaControl.transform.GetComponent<CharacterHealth> ().Evt_HealthChanged += OnCharacterHealthChanged;

						return retCharacter;
					}
				}
			}
			else
			{
				Debug.LogError("No character assets in Resources folder");

				return null;
			}
		}
		else
		{
			Debug.LogError("Can not load character no character data was saved");

			return null;
		}

		return null;
	}

	/// <summary>
	/// Unlocks the next level.
	/// </summary>
	private void UnlockNextLevel()
	{
		if((nextMainLevelToUnlock == 0) || (nextSubLevelToUnlock == 0))
		{
			return;
		}

		SubLevelData data = SubLevelData.Load ();

		if(data.UnlockSubLevel (nextMainLevelToUnlock, nextSubLevelToUnlock))
		{
			Debug.LogError("Error when trying to unlock Level"+ nextMainLevelToUnlock + "-" + nextSubLevelToUnlock);
		}
	}

	/// <summary>
	/// Calculates the score and save.
	/// </summary>
	/// <returns>The score.</returns>
	private int CalculateScoreAndSave(bool success)
	{
		//player fail... do not save score keep old score
		if(!success)
		{
			return 0;
		}

		int score = 0;

		//calculate socre base on player life remain... add 1 score if player life greater or equal than
		//3
		if(StoreInventory.GetItemBalance(StoreAssets.PLAYER_LIFE_ITEM_ID) >= 3)
		{
			score += 1;
		}

		float currentHealth = character.GetComponent<CharacterHealth>().GetCurrentHealth();
		float maxHealth = character.GetComponent<CharacterHealth>().maxHealth;

		//calculate score base on player health... add 1 score if player health is greater or equal than 
		//max health
		if(currentHealth >= maxHealth)
		{
			score += 1;
		}

		//calculate score base on fish bone(star count)... add 1 score if fish bone greater or equal than
		//max star count defined by level
		if(starCount >= maxStartCount)
		{
			score += 1;
		}

		SubLevelData data = SubLevelData.Load ();

		//if new score greater than old score save it
		if(score > data.GetSubLevelScore(currentMainLevel, currentSubLevel))
		{
			if(!data.SaveScoreForSubLevel(currentMainLevel, currentSubLevel, score))
			{
				Debug.LogError("save score fail Level"+currentMainLevel+"-"+currentSubLevel);
			}
		}


		return score;
	}

	private void ShowFinalResult(bool success)
	{


		if(success)
		{
			//show result
			hudControl.resultControl.ShowResult (success, CalculateScoreAndSave (success), mileController.totalMileage, coinCount, starCount);
		}
		else
		{
			//show result
			hudControl.resultControl.ShowResult (success, CalculateScoreAndSave (success),mileController.totalMileage - mileController.CurrentMile, coinCount, starCount);
		}
		
		//make game time scale to 0
		Time.timeScale = 0f;
		
		//pause EventManager
		eventManager.isRunning = false;
		
		//pause MileageController
		mileController.isRunning = false;
		
		//recycle all objects
		objectPool.RecycleAllObjects ();
		
		//stop all events
		eventManager.StopAllEvents ();
		
		//stop input manager
		InputManager inputMgr = character.GetComponent<InputManager> ();
		inputMgr.inputManagerEnabled = false;
	}

	////////////////////////////////Internal////////////////////////////////

	////////////////////////////////Public Interface////////////////////////////////

	/// <summary>
	/// Start the game.
	/// </summary>
	public void StartGame()
	{
		//make game time scale to 1
		Time.timeScale = 1f;

		//start input manager
		InputManager inputMgr = character.GetComponent<InputManager> ();
		inputMgr.inputManagerEnabled = true;
		
		//start EventManager
		eventManager.StartEventManager ();
		
		//start MileageController
		mileController.StartCounting ();

		//unlock ability panel so player can interact with
		hudControl.abilityControl.UnLockAbitliyPanel ();

	}
	
	/// <summary>
	/// Pauses the game.
	/// </summary>
	public void PauseGame()
	{
		//make game time scale to 0
		Time.timeScale = 0f;

		//stop input manager
		InputManager inputMgr = character.GetComponent<InputManager> ();
		inputMgr.inputManagerEnabled = false;
		
		//pause EventManager
		eventManager.isRunning = false;
		
		//pause MileageController
		mileController.isRunning = false;
		

	}
	
	/// <summary>
	/// Resumes the game.
	/// </summary>
	public void ResumeGame()
	{
		//make game time scale to 1
		Time.timeScale = 1f;

		//start input manager
		InputManager inputMgr = character.GetComponent<InputManager> ();
		inputMgr.inputManagerEnabled = true;
		
		//run EventManager
		eventManager.isRunning = true;
		
		//run MileageController
		mileController.isRunning = true;
		

	}
	
	/// <summary>
	/// Restarts the game.
	/// </summary>
	public void RestartGame()
	{
		//stop input manager
		InputManager inputMgr = character.GetComponent<InputManager> ();
		inputMgr.inputManagerEnabled = false;

		//make game time scale to 0
		Time.timeScale = 0f;
		
		//pause EventManager
		eventManager.isRunning = false;
		
		//pause MileageController
		mileController.isRunning = false;

		//recycle all objects
		objectPool.RecycleAllObjects ();

		//stop all events
		eventManager.StopAllEvents ();

		//clear star count
		starCount = 0;

		//clear coin count
		coinCount = 0;


		//make sure background is scrolling
		StartBackgroundScrolling ();

		//unlock ability panel so player can interact with
		hudControl.abilityControl.UnLockAbitliyPanel ();

		//tell hud to reset
		hudControl.ResetHUD ();

		//Restart character
		//character.GetComponent<CharacterControl> ().Restart ();
		if(character)
		{
			GameObject.Destroy(character);
		}
		
		character = LoadCharacter ();

		//make game time scale to 1
		Time.timeScale = 1f;
	}

	/// <summary>
	/// Stops the background scrolling.
	/// </summary>
	public void StopBackgroundScrolling()
	{
		//stop background scrolling
		foreach(GameObject b in backgrounds)
		{
			b.GetComponent<ScrollableBackground>().StopScrolling();
		}
	}

	/// <summary>
	/// Starts the background scrolling.
	/// </summary>
	public void StartBackgroundScrolling()
	{
		//start background scrolling
		foreach(GameObject b in backgrounds)
		{
			b.GetComponent<ScrollableBackground>().StartScrolling();
		}
	}

	/// <summary>
	/// Slows down background scrolling.
	/// </summary>
	public void SlowDownBackgroundScrolling()
	{
		//slow down background scrolling
		foreach(GameObject b in backgrounds)
		{
			b.GetComponent<ScrollableBackground>().StartSlowDown();
		}
	}

	////////////////////////////////Public Interface////////////////////////////////

	////////////////////////////////Event////////////////////////////////

	/// <summary>
	/// Handle event at moment character dead
	/// </summary>
	/// <param name="chaControl">Cha control.</param>
	void EventCharacterDead(CharacterControl chaControl)
	{
		//stop background scrolling
		StopBackgroundScrolling ();

		//stop mile counter
		mileController.isRunning = false;

		//local ability panel so player can not interact with
		hudControl.abilityControl.LockAbitliyPanel ();
	}

	/// <summary>
	/// Handle event at moment character victory
	/// </summary>
	/// <param name="chaControl">Cha control.</param>
	void EventCharacterVictory(CharacterControl chaControl)
	{
		//stop background scrolling
		//StopBackgroundScrolling();

		//local ability panel so player can not interact with
		hudControl.abilityControl.LockAbitliyPanel ();
	}

	/// <summary>
	/// Handle event that character dead and action is finished.
	/// </summary>
	/// <param name="chaControl">Cha control.</param>
	void EventCharacterDeadFinished(CharacterControl chaControl)
	{
		//show result
		ShowFinalResult (false);

		//give player coin they eat from this level
		StoreInventory.GiveItem (StoreAssets.CAT_COIN_CURRENCY_ITEM_ID, coinCount);

		//test
		//RestartGame ();
	}

	/// <summary>
	/// Handle event that character victory and action is finished.
	/// </summary>
	/// <param name="chaControl">Cha control.</param>
	void EventCharacterVictoryFinished(CharacterControl chaControl)
	{
		//show result
		ShowFinalResult (true);

		//give player coin they eat from this level
		StoreInventory.GiveItem (StoreAssets.CAT_COIN_CURRENCY_ITEM_ID, coinCount);

		//unlock next level
		UnlockNextLevel ();

		//tset
		//RestartGame ();
	}

	/// <summary>
	/// Handle event character's health changed.
	/// </summary>
	/// <param name="damageAmount">Damage amount.</param>
	/// <param name="healthBefore">Health before.</param>
	/// <param name="healthAfter">Health after.</param>
	void OnCharacterHealthChanged(float healthBefore, float healthAfter)
	{
		//update health hud if character health change
		hudControl.UpdateHealthHUD (healthAfter);
	}

	/// <summary>
	/// Handle event that a mile reached
	/// </summary>
	/// <param name="mc">Mc.</param>
	/// <param name="reachedMileage">Reached mileage.</param>
	void EventMileReached(MileageController mc, int reachedMileage)
	{
		//Debug.Log ("a mile reached: "+reachedMileage);

		//tell hud a mile reach and present mile indicator
		hudControl.mileageLineControl.PresentMileageLine (reachedMileage);
	}

	/// <summary>
	/// Handle event that goal mile reached
	/// </summary>
	/// <param name="mc">Mc.</param>
	/// <param name="goalMileage">Goal mileage.</param>
	void EventGoalMileReached(MileageController mc, int goalMileage)
	{
		//Debug.Log ("goal mile reached: " + goalMileage);

		//stop input manager
		InputManager inputMgr = character.GetComponent<InputManager> ();
		inputMgr.inputManagerEnabled = false;

		//make character victory
		CharacterControl chaControl = character.GetComponent<CharacterControl>();
		
		if((chaControl.IsDead == false) && (chaControl.IsVictory == false))
		{
			chaControl.PlayerVictory();
		}

	}

	/// <summary>
	/// Handle event that current mile report
	/// </summary>
	/// <param name="mc">Mc.</param>
	/// <param name="currentMileage">Current mileage.</param>
	void EventCurrentMile(MileageController mc, int currentMileage)
	{
		//Debug.Log ("current mile: " + currentMileage);

		//update hud for current mile
		hudControl.UpdateMileageHUD (currentMileage);
	}

	/// <summary>
	/// Raises the ability button press event.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	void OnAbilityButtonPress(string itemId)
	{
		//get ability from pool
		GameObject ability = objectPool.GetObjectFromPool (itemId + "Ability", Vector3.zero, Quaternion.identity);

		//give ability to player
		character.GetComponent<CharacterControl> ().AddAbility (ability);
	}

	/// <summary>
	/// Handle the pause menu resume click event.
	/// </summary>
	/// <param name="control">Control.</param>
	void OnPauseMenuResumeClick(UIPauseControl control)
	{
		ResumeGame ();
	}

	/// <summary>
	/// Handle the pause menu restart click event.
	/// </summary>
	/// <param name="control">Control.</param>
	void OnPauseMenuRestartClick(UIPauseControl control)
	{
		RestartGame ();
	}

	/// <summary>
	/// Handle the pause menu exit click event.
	/// </summary>
	/// <param name="control">Control.</param>
	void OnPauseMenuExitClick(UIPauseControl control)
	{
		Time.timeScale = 1.0f;

		GameObject.FindGameObjectWithTag (Tags.levelSelection).GetComponent<LevelSelection> ().SetMainLevelSelected(currentMainLevel);

		//Application.LoadLevel("LevelSelection");

		//(GameObject.FindObjectOfType (typeof(AdControl)) as AdControl).DestroyAd ();

		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel ("LevelSelection");
	}

	/// <summary>
	/// Handle the HUD pause click event.
	/// </summary>
	/// <param name="control">Control.</param>
	void OnHUDPauseClick(UIHUDControl control)
	{
		PauseGame ();
	}

	/// <summary>
	/// Handle the result confirm button click event.
	/// </summary>
	/// <param name="control">Control.</param>
	void OnResultConfirmButtonClick(UIResultControl control)
	{
		control.CloseResult ();

		Time.timeScale = 1.0f;

		GameObject.FindGameObjectWithTag (Tags.levelSelection).GetComponent<LevelSelection> ().SetMainLevelSelected(currentMainLevel);

		//Application.LoadLevel ("LevelSelection");

		//(GameObject.FindObjectOfType (typeof(AdControl)) as AdControl).DestroyAd ();

		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel ("LevelSelection");
	}

	/// <summary>
	/// Handle the result restart button click event.
	/// </summary>
	/// <param name="control">Control.</param>
	void OnResultRestartButtonClick(UIResultControl control)
	{
		if(StoreInventory.GetItemBalance(StoreAssets.PLAYER_LIFE_ITEM_ID) > 0)
		{
			control.CloseResult ();

			StoreInventory.TakeItem(StoreAssets.PLAYER_LIFE_ITEM_ID, 1);
			
			RestartGame ();
		}
		else
		{
			hudControl.alertControl.ShowAlertWindow(notEnoughLifeKey, notEnoughLifeDesc);
		}

	}

	/// <summary>
	/// Handle the alert window close event.
	/// 
	/// alter window show because player has no enough life to restart game
	/// </summary>
	/// <param name="control">Control.</param>
	void OnAlertWindowClose(UIAlertControl control)
	{
		//show in game store
		hudControl.inGameStore.ShowInGameStore ();
	}

	////////////////////////////////Event////////////////////////////////


	////////////////////////////////Properties////////////////////////////////

	public int StarCount
	{
		get
		{
			return starCount;
		}

		set
		{
			starCount = value;

			//update fish bone hud
			hudControl.UpdateFishBoneHUD(starCount);

			//check if reach every certain amount of star count...give character invulnerable ability
			if((starCount % invulnerablePerStarCount) == 0)
			{
				if(invulnerableAbilityPrefab != null)
				{
					//create ability gameobject
					//GameObject ability = Instantiate(invulnerableAbilityPrefab) as GameObject;
					//ability.name = invulnerableAbilityPrefab.name;
					GameObject ability = objectPool.GetObjectFromPool(invulnerableAbilityPrefab, Vector3.zero, Quaternion.identity);
					
					//give to player
					character.GetComponent<CharacterControl>().AddAbility(ability);
				}
				else
				{
					Debug.Log("You did not assign Invulnerable ability prefab to GameController");
				}

			}


		}
	}

	public int CoinCount
	{
		get
		{
			return coinCount;
		}

		set
		{
			coinCount = value;

			//update conin hud
			hudControl.UpdateCoinHUD(coinCount);
		}
	}
	////////////////////////////////Properties////////////////////////////////
}
