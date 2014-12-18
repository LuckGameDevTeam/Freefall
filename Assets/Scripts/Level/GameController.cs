using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SIS;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Game controller.
/// 
/// Class is used to control game
/// </summary>
//[RequireComponent (typeof(ObjectPool))]//replace by TrashMan
public class GameController : MonoBehaviour 
{
	/// <summary>
	/// Singleton GameController
	/// </summary>
	[System.NonSerialized]
	public static GameController sharedGameController;

	public string notEnoughLifeKey = "NotEnoughLife";
	public string notEnoughLifeDesc = "NotEnoughLifeDesc";
	public string syncDataFailKey = "SyncDataFail";

	/// <summary>
	/// The victory clip.
	/// </summary>
	public AudioClip victoryClip;

	/// <summary>
	/// The fail clip.
	/// </summary>
	public AudioClip failClip;

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
	public int maxStarCount = 100;

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

	/*
	/// <summary>
	/// The object pool manager.
	/// replace by TrashMan
	/// </summary>
	//[System.NonSerialized]
	//public ObjectPool objectPool;
	*/

	/// <summary>
	/// The object pool manager.
	/// </summary>
	private TrashMan trashMan;

	/// <summary>
	/// Reference to UIHUDControl
	/// </summary>
	public UIHUDControl hudControl;

	/// <summary>
	/// The sound player.
	/// </summary>
	private SFXPlayer soundPlayer;

	private FBController fbController;

	private SISDataSync sisDs;

#if TestMode
	//for test mode only
	public enum CharacterName
	{
		BellCat,
		CandyCat,
		GhostCat,
		HulkCat,
		IronCat,
		NinjaCat,
		PunpkinCat,
		TarzanCat
	}
	//for test mode
	public CharacterName testCharacterName = CharacterName.BellCat;
#endif

	void Awake()
	{
#if UNITY_EDITOR
		if(!GameObject.FindObjectOfType(typeof(SFXManager)))
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/AudioManager/SFXManager.prefab", typeof(GameObject)) as GameObject;
			
			Instantiate(prefab, Vector3.zero, Quaternion.identity).name = prefab.name;
		}
#endif

		if(currentMainLevel == 0)
		{
			DebugEx.DebugError("You can not assigned 0 to main level in GameController");
		}

		if(currentSubLevel == 0)
		{
			DebugEx.DebugError("You can not assigend 0 to sub level in GameController");
		}

		if(maxStarCount == 0)
		{
			DebugEx.DebugError("You can not assigend 0 to max start count, at least 1");
		}

		if((nextMainLevelToUnlock == currentMainLevel) && (nextSubLevelToUnlock == currentSubLevel))
		{
			DebugEx.DebugError("next main&sub level to unlock is as same as curren main&sub level");
		}

		if(hudControl == null)
		{
			DebugEx.DebugError("You did not assign UIHUDControl");
		}

		//store instance
		sharedGameController = this;

		Time.timeScale = 1f;

		//find FBController
		fbController = GameObject.FindObjectOfType (typeof(FBController)) as FBController;

		//find scrollable background
		backgrounds = GameObject.FindGameObjectsWithTag (Tags.scrollingBackground);

		//find mile controller
		mileController = GetComponent<MileageController> ();

		//find event manager
		eventManager = GetComponent<EventManager> ();

		//find object pool, replace by TrashMan
		//objectPool = GetComponent<ObjectPool> ();

		//find TrashMan
		trashMan = GetComponent<TrashMan> ();

		//find all obstacle spawners
		spawners = GameObject.FindGameObjectsWithTag (Tags.obstacleSpawner);

		//find sound player
		soundPlayer = GetComponent<SFXPlayer> ();

		if(soundPlayer == null)
		{
			soundPlayer = gameObject.AddComponent<SFXPlayer>();
		}

		sisDs = GetComponent<SISDataSync> ();
		sisDs.Evt_OnUploadDataComplete += OnUploadDataSuccess;
		sisDs.Evt_OnUploadDataFail += OnUploadDataFail;
		sisDs.Evt_OnAccountLoginFromOtherDevice += OnLoginOtherDevice;

		ServerSync.SharedInstance.Evt_OnOtherDeviceLogin += OnLoginFromOtherDevice;
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
#if TestMode

					if(testCharacterName.ToString() == characterAssets[i].name)
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
#else
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
#endif
				}
			}
			else
			{
				DebugEx.DebugError("No character assets in Resources folder");

				return null;
			}
		}
		else
		{
			DebugEx.DebugError("Can not load character no character data was saved");

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

		if(!data.UnlockSubLevel (nextMainLevelToUnlock, nextSubLevelToUnlock))
		{
			DebugEx.DebugError("Error when trying to unlock Level"+ nextMainLevelToUnlock + "-" + nextSubLevelToUnlock);
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

#if TestMode
		score += 1;
#else

		//calculate socre base on player life remain... add 1 score if player life greater or equal than
		//3
		//if(StoreInventory.GetItemBalance(StoreAssets.PLAYER_LIFE_ITEM_ID) >= 3)
		if(DBManager.GetPlayerData(LifeCounter.PlayerLife).AsInt >= 3)
		{
			score += 1;
		}
#endif

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
		if(starCount >= maxStarCount)
		{
			score += 1;
		}

		SubLevelData data = SubLevelData.Load ();

		//if new score greater than old score save it
		if(score > data.GetSubLevelScore(currentMainLevel, currentSubLevel))
		{
			if(!data.SaveScoreForSubLevel(currentMainLevel, currentSubLevel, score))
			{
				DebugEx.DebugError("save score fail Level"+currentMainLevel+"-"+currentSubLevel);
			}
		}


		return score;
	}

	private void ShowFinalResult(bool success)
	{


		if(success)
		{
			PostMileToFacebook(mileController.totalMileage);

			ServerSync.SharedInstance.UploadScore (mileController.totalMileage);

			//show result
			hudControl.resultControl.ShowResult (success, CalculateScoreAndSave (success), mileController.totalMileage, coinCount, starCount);
		}
		else
		{
			PostMileToFacebook(mileController.totalMileage - mileController.CurrentMile);

			ServerSync.SharedInstance.UploadScore (mileController.totalMileage - mileController.CurrentMile);

			//show result
			hudControl.resultControl.ShowResult (success, CalculateScoreAndSave (success),mileController.totalMileage - mileController.CurrentMile, coinCount, starCount);
		}
		
		//make game time scale to 0
		Time.timeScale = 0f;
		
		//pause EventManager
		eventManager.isRunning = false;
		
		//pause MileageController
		mileController.isRunning = false;
		
		//recycle obstacles
		RecycleObstacles ();
		
		//stop all events
		eventManager.StopAllEvents ();
		
		//stop input manager
		InputManager inputMgr = character.GetComponent<InputManager> ();
		inputMgr.inputManagerEnabled = false;
	}

	/// <summary>
	/// Posts the mile to facebook.
	/// </summary>
	/// <param name="mile">Mile.</param>
	private void PostMileToFacebook(int mile)
	{
#if TestMode
		return;
#endif
		//submit score only if we play fb game
		if((GameObject.FindObjectOfType(typeof(GameType)) as GameType).currentGameType == TypeOfGame.FB)
		{
			//submit mile only if mile is greater than previous one
			if(fbController.GetScoreById(fbController.PlayerInfo.id) < mile)
			{
				DebugEx.Debug("Submit score: "+mile);
				fbController.Evt_OnScoreSubmitted += OnScoreSubmitted;

				fbController.SubmitScore (mile);
			}
		}
	}

	void OnScoreSubmitted(FBController controller, FacebookUserInfo userInfo, int score)
	{
		fbController.Evt_OnScoreSubmitted -= OnScoreSubmitted;

		DebugEx.Debug("Submit score complete: "+score);
	}

	/// <summary>
	/// Recycles obstacles.
	/// </summary>
	void RecycleObstacles()
	{
		/*
		Obstacle[] obstacles = GameObject.FindObjectsOfType<Obstacle> ();

		for(int i=0; i<obstacles.Length; i++)
		{
			TrashMan.despawn(obstacles[i].gameObject);
		}
		*/

		GameObject[] objs = GameObject.FindObjectsOfType<GameObject> ();

		for(int i=0; i<objs.Length; i++)
		{
			objs[i].SendMessage("GameRestart", SendMessageOptions.DontRequireReceiver);
		}
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

		//stop sound
		if(soundPlayer)
		{
			if(soundPlayer.IsPlaying)
			{
				soundPlayer.StopSound();
			}
		}
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
		//stop sound
		if(soundPlayer)
		{
			if(soundPlayer.IsPlaying)
			{
				soundPlayer.StopSound();
			}
		}

		//stop input manager
		InputManager inputMgr = character.GetComponent<InputManager> ();
		inputMgr.inputManagerEnabled = false;

		//make game time scale to 0
		Time.timeScale = 0f;
		
		//pause EventManager
		eventManager.isRunning = false;
		
		//pause MileageController
		mileController.isRunning = false;

		//recycle all obstacles
		RecycleObstacles ();

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

		//stop music
		GameObject.FindObjectOfType<MusicManager> ().PlayMusic ();
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
	#region SISDataSync callback
	void OnUploadDataSuccess()
	{
		DebugEx.Debug("upload client data to server");
	}

	void OnUploadDataFail()
	{
		DebugEx.DebugError("sync data fail");
	}

	void OnLoginOtherDevice()
	{
		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel ("LoginScene");
	}
	#endregion SISDataSync callback

	#region ServerSync callback
	void OnLoginFromOtherDevice(ServerSync syncControl, int errorCode)
	{
		GameObject.FindGameObjectWithTag (Tags.levelLoadManager).GetComponent<LevelLoadManager> ().LoadLevel ("LoginScene");
	}
	#endregion ServerSync callback

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

		//stop music
		GameObject.FindObjectOfType<MusicManager> ().StopMusic ();

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

		//stop music
		GameObject.FindObjectOfType<MusicManager> ().StopMusic ();
	}

	/// <summary>
	/// Handle event that character dead and action is finished.
	/// </summary>
	/// <param name="chaControl">Cha control.</param>
	void EventCharacterDeadFinished(CharacterControl chaControl)
	{


		//play fail sound
		if(failClip != null)
		{
			if(soundPlayer == null)
			{
				soundPlayer = gameObject.AddComponent<SFXPlayer>();
			}

			if(soundPlayer.IsPlaying)
			{
				soundPlayer.StopSound();
			}

			soundPlayer.sfxClip = failClip;
			soundPlayer.ignoreTimeScale = true;
			soundPlayer.PlaySound();
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" unable to play fail clip, fail clip not assigned");
		}

#if TestMode

		DebugEx.DebugError("TestMode can not increase funds for player");
#else
		//give player coin they eat from this level
		//StoreInventory.GiveItem (StoreAssets.CAT_COIN_CURRENCY_ITEM_ID, coinCount);
		DBManager.IncreaseFunds ((IAPManager.GetCurrency () [0]).name, coinCount);
#endif

		//upload client data
		sisDs.UploadData ();
		
		//show result
		ShowFinalResult (false);

		//test
		//RestartGame ();
	}

	/// <summary>
	/// Handle event that character victory and action is finished.
	/// </summary>
	/// <param name="chaControl">Cha control.</param>
	void EventCharacterVictoryFinished(CharacterControl chaControl)
	{


		//play victory sound
		if(victoryClip != null)
		{
			if(soundPlayer == null)
			{
				soundPlayer = gameObject.AddComponent<SFXPlayer>();
			}
			
			if(soundPlayer.IsPlaying)
			{
				soundPlayer.StopSound();
			}
			
			soundPlayer.sfxClip = victoryClip;
			soundPlayer.ignoreTimeScale = true;
			soundPlayer.PlaySound();
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" unable to play victory clip, victory clip not assigned");
		}

#if TestMode
		DebugEx.DebugError("TestMode can not increase funds for player");
#else
		//give player coin they eat from this level
		//StoreInventory.GiveItem (StoreAssets.CAT_COIN_CURRENCY_ITEM_ID, coinCount);
		DBManager.IncreaseFunds ((IAPManager.GetCurrency () [0]).name, coinCount);
#endif

		//unlock next level
		UnlockNextLevel ();

		//upload client data
		sisDs.UploadData ();
		
		//show result
		ShowFinalResult (true);

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
		//DebugEx.Debug ("a mile reached: "+reachedMileage);

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
		//DebugEx.Debug ("goal mile reached: " + goalMileage);

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
		//DebugEx.Debug ("current mile: " + currentMileage);

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
		//GameObject ability = objectPool.GetObjectFromPool (itemId + "Ability", Vector3.zero, Quaternion.identity);
		GameObject ability = TrashMan.spawn (itemId + "Ability", Vector3.zero, Quaternion.identity);

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
		sisDs.UploadData ();

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
		//upload client data
		sisDs.UploadData ();

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
#if TestMode
		RestartGame();
#else
		//if(StoreInventory.GetItemBalance(StoreAssets.PLAYER_LIFE_ITEM_ID) > 0)
		if(DBManager.GetPlayerData(LifeCounter.PlayerLife).AsInt > 0)
		{
			control.CloseResult ();
			
			//StoreInventory.TakeItem(StoreAssets.PLAYER_LIFE_ITEM_ID, 1);
			DBManager.SetPlayerData(LifeCounter.PlayerLife, new SimpleJSON.JSONData(DBManager.GetPlayerData(LifeCounter.PlayerLife).AsInt-1));
			
			RestartGame ();
		}
		else
		{
			hudControl.alertControl.ShowAlertWindow(notEnoughLifeKey, notEnoughLifeDesc);
		}
#endif


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
					//GameObject ability = objectPool.GetObjectFromPool(invulnerableAbilityPrefab, Vector3.zero, Quaternion.identity);
					GameObject ability = TrashMan.spawn(invulnerableAbilityPrefab, Vector3.zero, Quaternion.identity);
					
					//give to player
					character.GetComponent<CharacterControl>().AddAbility(ability);
				}
				else
				{
					DebugEx.Debug("You did not assign Invulnerable ability prefab to GameController");
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
