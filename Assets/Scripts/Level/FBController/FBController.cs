using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;

public class FBController : MonoBehaviour 
{
	public delegate void EventOnFacebookModuleInitialized(FBController controller, SPFacebook fbModule);
	/// <summary>
	/// The event facebook module initialized.
	/// </summary>
	public EventOnFacebookModuleInitialized Evt_OnFacebookModuleInitialized;

	public delegate void EventOnFacebookLoginSuccess(FBController controller);
	/// <summary>
	/// The event on facebook login success.
	/// </summary>
	public EventOnFacebookLoginSuccess Evt_OnFacebookLoginSuccess;

	public delegate void EventOnFacebookLoginFail(FBController controller);
	/// <summary>
	/// The event on facebook login fail.
	/// </summary>
	public EventOnFacebookLoginFail Evt_OnFacebookLoginFail;

	public delegate void EventOnFacebookLogout(FBController controller);
	/// <summary>
	/// The event facebook logout.
	/// </summary>
	public EventOnFacebookLogout Evt_OnFacebookLogout;

	public delegate void EventOnUserDataLoaded(FBController controller, FacebookUserInfo userInfo);
	/// <summary>
	/// The event user data loaded.
	/// </summary>
	public EventOnUserDataLoaded Evt_OnUserDataLoaded;

	public delegate void EventOnUserDataFailToLoad(FBController controller);
	/// <summary>
	/// The event user data fail to load.
	/// </summary>
	public EventOnUserDataFailToLoad Evt_OnUserDataFailToLoad;

	public delegate void EventOnNumberOfFriendReceived(FBController controller, int number);
	/// <summary>
	/// The event number of user's friend received.
	/// </summary>
	public EventOnNumberOfFriendReceived Evt_OnNumberOfFriendReceived;

	public delegate void EventOnFriendDataLoaded(FBController controller, List<FacebookUserInfo> friendList);
	/// <summary>
	/// The event friend data loaded.
	/// </summary>
	public EventOnFriendDataLoaded Evt_OnFriendDataLoaded;

	public delegate void EventOnFriendDataFailToLoad(FBController controller);
	/// <summary>
	/// The event on friend data fail to load.
	/// </summary>
	public EventOnFriendDataFailToLoad Evt_OnFriendDataFailToLoad;

	public delegate void EventOnScoreSubmitted(FBController controller, FacebookUserInfo userInfo, int score);
	/// <summary>
	/// The event user score submitted.
	/// </summary>
	public EventOnScoreSubmitted Evt_OnScoreSubmitted;

	public delegate void EventOnLoadPlayerAndFriendScoreSuccess(FBController controller);
	/// <summary>
	/// The event load player and friend score success.
	/// </summary>
	public EventOnLoadPlayerAndFriendScoreSuccess Evt_OnLoadPlayerAndFriendScoreSuccess;

	public delegate void EventOnFacebookPostSuccess(FBController controller);
	/// <summary>
	/// The event facebook post success.
	/// </summary>
	public EventOnFacebookPostSuccess Evt_OnFacebookPostSuccess;

	public delegate void EventOnFacebookPostFail(FBController controller);
	/// <summary>
	/// The event facebook post fail.
	/// </summary>
	public EventOnFacebookPostFail Evt_OnFacebookPostFail;

	/// <summary>
	/// Init facebook module on start
	/// </summary>
	public bool initOnStart = true;

	/// <summary>
	/// auto login when facebook init finished.
	/// </summary>
	public bool loginOnInit = false;

	/// <summary>
	/// auto load user data when login facebook.
	/// </summary>
	public bool loadUserDataOnlogin = true;

	/// <summary>
	/// The app link for post to wall.
	/// </summary>
	public string postAppLink = "http://www.google.com";

	/// <summary>
	/// The app name localized for post to wall.
	/// </summary>
	public string postAppNameKey = "FBAppName";

	/// <summary>
	/// The caption localized for post to wall.
	/// </summary>
	public string postCaptionkey = "FBCaption";

	/// <summary>
	/// The desc localized for post to wall.
	/// </summary>
	public string postDescKey = "FBDesc";

	/// <summary>
	/// The action name localized for post to wall.
	/// </summary>
	public string postActionNameKey = "FBAction";

	/// <summary>
	/// The action link for post to wall
	/// </summary>
	public string postActionLink = "http://www.google.com";

	/// <summary>
	/// is facebook module initialized or not.
	/// </summary>
	private bool isInitialized = false;

	/// <summary>
	/// is login facebook or not.
	/// </summary>
	private bool isLogin = false;

	/// <summary>
	/// is user data loaded.
	/// </summary>
	private bool isUserDataLoaded = false;

	/// <summary>
	/// User's friend that use this app
	/// Update every time GetNumberOfFriend called
	/// </summary>
	private int numberOfFriend = 0;

	/// <summary>
	/// user submitted score
	/// clear every when complete submitted
	/// </summary>
	private int submittedScore = 0;

	void OnEnable()
	{
		SPFacebook.instance.addEventListener (FacebookEvents.FACEBOOK_INITED, OnInit);
		SPFacebook.instance.addEventListener (FacebookEvents.AUTHENTICATION_SUCCEEDED, OnAuthnicationSuccess);
		SPFacebook.instance.addEventListener (FacebookEvents.AUTHENTICATION_FAILED, OnAuthnicationFail);
		SPFacebook.instance.addEventListener (FacebookEvents.USER_DATA_LOADED, OnUserDataLoaded);
		SPFacebook.instance.addEventListener (FacebookEvents.USER_DATA_FAILED_TO_LOAD, OnUserDataFailToLoad);
		SPFacebook.instance.addEventListener (FacebookEvents.FRIENDS_DATA_LOADED, OnFriendDataLoaded);
		SPFacebook.instance.addEventListener (FacebookEvents.FRIENDS_FAILED_TO_LOAD, OnFriendDataFailToLoad);
		SPFacebook.instance.addEventListener (FacebookEvents.SUBMIT_SCORE_REQUEST_COMPLETE, OnSubmitScoreSuccess);
		SPFacebook.instance.addEventListener (FacebookEvents.POST_SUCCEEDED, OnPostSuccess);
		SPFacebook.instance.addEventListener (FacebookEvents.APP_SCORES_REQUEST_COMPLETE, OnLoadAppScoresSuccess);
		SPFacebook.instance.addEventListener (FacebookEvents.PLAYER_SCORES_REQUEST_COMPLETE, OnPlayerScoreLoaded);
		SPFacebook.instance.addEventListener (FacebookEvents.POST_FAILED, OnPostFail);
	}

	void OnDisable()
	{
		if(SPFacebook.instance != null)
		{
			SPFacebook.instance.removeEventListener (FacebookEvents.FACEBOOK_INITED, OnInit);
			SPFacebook.instance.removeEventListener (FacebookEvents.AUTHENTICATION_SUCCEEDED, OnAuthnicationSuccess);
			SPFacebook.instance.removeEventListener (FacebookEvents.AUTHENTICATION_FAILED, OnAuthnicationFail);
			SPFacebook.instance.removeEventListener (FacebookEvents.USER_DATA_LOADED, OnUserDataLoaded);
			SPFacebook.instance.removeEventListener (FacebookEvents.USER_DATA_FAILED_TO_LOAD, OnUserDataFailToLoad);
			SPFacebook.instance.removeEventListener (FacebookEvents.FRIENDS_DATA_LOADED, OnFriendDataLoaded);
			SPFacebook.instance.removeEventListener (FacebookEvents.FRIENDS_FAILED_TO_LOAD, OnFriendDataFailToLoad);
			SPFacebook.instance.removeEventListener (FacebookEvents.SUBMIT_SCORE_REQUEST_COMPLETE, OnSubmitScoreSuccess);
			SPFacebook.instance.removeEventListener (FacebookEvents.APP_SCORES_REQUEST_COMPLETE, OnLoadAppScoresSuccess);
			SPFacebook.instance.removeEventListener (FacebookEvents.PLAYER_SCORES_REQUEST_COMPLETE, OnPlayerScoreLoaded);
			SPFacebook.instance.removeEventListener (FacebookEvents.POST_SUCCEEDED, OnPostSuccess);
			SPFacebook.instance.removeEventListener (FacebookEvents.POST_FAILED, OnPostFail);
		}

	}
	// Use this for initialization
	void Start () 
	{
		if(initOnStart && (isInitialized == false))
		{
			InitFacebookModule();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{


	}

	#region Public interface

	/// <summary>
	/// Inits the facebook module.
	/// 
	/// trigger Evt_OnFacebookModuleInitialized
	/// </summary>
	public void InitFacebookModule()
	{
		if(isInitialized == false)
		{
			//init facebook module
			SPFacebook.instance.Init();
		}
		else
		{
			Debug.LogWarning("Facebook module has been initialized");
		}
	}

	/// <summary>
	/// Login to facebook.
	/// 
	/// trigger Evt_OnFacebookLoginSuccess on success
	/// trigger Evt_OnFacebookLoginFail on fail
	/// </summary>
	public void Login()
	{
		if(IsFBModuleInitialized())
		{
			if(!SPFacebook.instance.IsLoggedIn)
			{
				//login facebook
				SPFacebook.instance.Login();
			}
			else
			{
				Debug.LogWarning("You have already loggin to facebook");

				OnAuthnicationSuccess();
			}
		}
	}

	/// <summary>
	/// Logout from facebook
	/// 
	/// trigger Evt_OnFacebookLogout
	/// </summary>
	public void Logout()
	{
		if(IsFBModuleInitialized())
		{
			if(SPFacebook.instance.IsLoggedIn)
			{
				//log out facebook
				SPFacebook.instance.Logout();

				isLogin = false;

				if(Evt_OnFacebookLogout != null)
				{
					Evt_OnFacebookLogout(this);
				}
			}
			else
			{
				Debug.LogWarning("You did not login");
			}
		}
	}

	/// <summary>
	/// Load user data.
	/// 
	/// trigger Evt_OnUserDataLoaded on success
	/// trigger Evt_OnUserDataFailToLoad on fail
	/// </summary>
	public void LoadUserData()
	{
		if(IsFBModuleInitialized())
		{
			if(IsLoggin())
			{
				//load user data
				SPFacebook.instance.LoadUserData();
			}

		}
	}

	/// <summary>
	/// Gets the number of user's friend.
	/// This method do not load friend's info, call LoadFriendData instead
	/// 
	/// trigger Evt_OnNumberOfFriendReceived
	/// </summary>
	public void GetNumberOfFriend()
	{
		if(IsFBModuleInitialized())
		{
			if(IsLoggin())
			{
				//find out number of friends use this app
				//Using Unity facebook api
				FB.API ("/" + SPFacebook.instance.UserId + "/friends?fields=installed", HttpMethod.GET, OnNumberOfFriendGetted);
			}
			
		}

	}

	/// <summary>
	/// Load user's friend data by give number of friend you want to load.
	/// 
	/// If you don't know the number of friend use GetNumberOfFriend and then
	/// call this method.
	/// 
	/// Friends who used this app.
	/// 
	/// trigger Evt_OnFriendDataLoaded on success
	/// trigger Evt_OnFriendDataFailToLoad on fail
	/// </summary>
	public void LoadFriendData(int limit)
	{
		if(IsFBModuleInitialized())
		{
			if(IsLoggin())
			{
				//load friend's info
				SPFacebook.instance.LoadFrientdsInfo(limit);
			}
			
		}
	}

	/// <summary>
	/// Submit user score to facebook
	/// </summary>
	/// <param name="score">Score.</param>
	public void SubmitScore(int score)
	{
		if(IsFBModuleInitialized())
		{
			if(IsLoggin())
			{
				SPFacebook.instance.SubmitScore(score);

				submittedScore = score;
			}
			
		}
	}

	/// <summary>
	/// Gets the score by identifier.
	/// </summary>
	/// <returns>The score by identifier.</returns>
	/// <param name="playerId">Player identifier.</param>
	public int GetScoreById(string playerId)
	{
		if(IsFBModuleInitialized())
		{
			if(IsLoggin())
			{
				return SPFacebook.instance.GetScoreByUserId(playerId);
			}
			
		}

		return 0;
	}

	/// <summary>
	/// Loads the player and friend score.
	/// </summary>
	public void LoadPlayerAndFriendScore()
	{
		if(IsFBModuleInitialized())
		{
			if(IsLoggin())
			{
				SPFacebook.instance.LoadAppScores();
			}
			
		}
	}

	/// <summary>
	/// Posts to facebook wall.
	/// Unauthn
	/// </summary>
	public void PostToFacebook()
	{
		if(IsFBModuleInitialized())
		{
			if(IsLoggin())
			{

				SPFacebook.instance.Post(
					SPFacebook.instance.UserId, 
					postAppLink, 
					Localization.Localize(postAppNameKey),
					Localization.Localize(postCaptionkey),
					Localization.Localize(postDescKey),
					"",
					"",
					"",
					""
					);
			}
			
		}
	}

	#endregion Public interface

	#region Internal

	private void LoadPlayerScore()
	{
		if(IsFBModuleInitialized())
		{
			if(IsLoggin())
			{
				SPFacebook.instance.LoadPlayerScores();
			}
			
		}
	}

	/// <summary>
	/// Determines whether FB module initialized.
	/// </summary>
	/// <returns><c>true</c> if this instance is FB module initialized; otherwise, <c>false</c>.</returns>
	private bool IsFBModuleInitialized()
	{
		if(isInitialized)
		{
			return true;
		}
		else
		{
			Debug.LogWarning("You need to initialize facebook module first");

			return false;
		}
	}

	/// <summary>
	/// Determines whether is loggin facebook or not.
	/// </summary>
	/// <returns><c>true</c> if this instance is loggin; otherwise, <c>false</c>.</returns>
	private bool IsLoggin()
	{
		if(isLogin)
		{
			return true;
		}
		else
		{
			Debug.LogWarning("You are not loggin facebook");

			return false;
		}
	}

	#endregion Internal

	#region Unity Facebook API callback

	void OnNumberOfFriendGetted(FBResult result)
	{
		IDictionary JSON =  ANMiniJSON.Json.Deserialize(result.Text) as IDictionary;
		IList f = JSON["data"] as IList;

		numberOfFriend = f.Count;

		if(Evt_OnNumberOfFriendReceived != null)
		{
			Evt_OnNumberOfFriendReceived(this, numberOfFriend);
		}
	}

	#endregion Unity Facebook API callback

	#region SFPFacebook callback

	/// <summary>
	/// Handle facebook inited
	/// </summary>
	void OnInit()
	{
		//set isInitialized to true
		isInitialized = true;

		isLogin = SPFacebook.instance.IsLoggedIn;

		//check if auto login
		if(loginOnInit)
		{
			Login();
		}
		else if(isLogin)//already login
		{
			Debug.LogWarning("You have already loggin to facebook");

			OnAuthnicationSuccess();
		}

		if(Evt_OnFacebookModuleInitialized != null)
		{
			Evt_OnFacebookModuleInitialized(this, SPFacebook.instance);
		}
	}

	/// <summary>
	/// Handle facebook login success
	/// </summary>
	void OnAuthnicationSuccess()
	{
		isLogin = true;

		//check if auto load user data
		if(loadUserDataOnlogin)
		{
			LoadUserData();
		}

		if(Evt_OnFacebookLoginSuccess != null)
		{
			Evt_OnFacebookLoginSuccess(this);
		}
	}

	/// <summary>
	/// Handle facebook login fail
	/// </summary>
	void OnAuthnicationFail()
	{
		isLogin = false;

		if(Evt_OnFacebookLoginFail != null)
		{
			Evt_OnFacebookLoginFail(this);
		}
	}

	/// <summary>
	/// Handle facebook user data has been loaded
	/// </summary>
	void OnUserDataLoaded()
	{
		isUserDataLoaded = true;

		if(Evt_OnUserDataLoaded != null)
		{
			Evt_OnUserDataLoaded(this, SPFacebook.instance.userInfo);
		}
	}

	/// <summary>
	/// Handle facebook user data fail to load
	/// </summary>
	void OnUserDataFailToLoad()
	{
		isUserDataLoaded = false;

		if(Evt_OnUserDataFailToLoad != null)
		{
			Evt_OnUserDataFailToLoad(this);
		}
	}

	/// <summary>
	/// Handle facebook friend data loaded
	/// 
	/// If there is no friend data it will be null
	/// </summary>
	void OnFriendDataLoaded()
	{
		if(Evt_OnFriendDataLoaded != null)
		{
			Evt_OnFriendDataLoaded(this, SPFacebook.instance.friendsList);
		}
	}

	/// <summary>
	/// Handle facebook friend data fail to load
	/// </summary>
	void OnFriendDataFailToLoad()
	{
		if(Evt_OnFriendDataFailToLoad != null)
		{
			Evt_OnFriendDataFailToLoad(this);
		}
	}

	/// <summary>
	/// Handle facebook submit score success
	/// </summary>
	void OnSubmitScoreSuccess()
	{
		if(Evt_OnScoreSubmitted != null)
		{
			Evt_OnScoreSubmitted(this, SPFacebook.instance.userInfo, submittedScore);
		}

		//clear
		submittedScore = 0;
	}

	/// <summary>
	/// Handle the load app scores success event.
	/// </summary>
	void OnLoadAppScoresSuccess()
	{
		LoadPlayerScore ();
	}

	/// <summary>
	/// handle the player score loaded event.
	/// </summary>
	void OnPlayerScoreLoaded()
	{
		if(Evt_OnLoadPlayerAndFriendScoreSuccess != null)
		{
			Evt_OnLoadPlayerAndFriendScoreSuccess(this);
		}
	}

	/// <summary>
	/// Handle facebook post success
	/// </summary>
	void OnPostSuccess()
	{
		if(Evt_OnFacebookPostSuccess != null)
		{
			Evt_OnFacebookPostSuccess(this);
		}
	}

	/// <summary>
	/// Handle facebook post fail
	/// </summary>
	void OnPostFail()
	{
		if(Evt_OnFacebookPostFail != null)
		{
			Evt_OnFacebookPostFail(this);
		}
	}

	#endregion SFPFacebook callback


	#region Properties

	/// <summary>
	/// Gets the facebook module.
	/// </summary>
	/// <value>The facebook module.</value>
	public SPFacebook FacebookModule
	{
		get
		{
			return SPFacebook.instance;
		}
	}

	/// <summary>
	/// Is facebook module inited.
	/// </summary>
	/// <value><c>true</c> if this instance is inited; otherwise, <c>false</c>.</value>
	public bool IsInited
	{
		get
		{
			return isInitialized;
		}
	}

	/// <summary>
	/// Is loggin facebook.
	/// </summary>
	/// <value><c>true</c> if this instance is login; otherwise, <c>false</c>.</value>
	public bool IsLogin
	{
		get
		{
			if(!isInitialized)
			{
				return false;
			}

			return isLogin;
		}
	}

	public bool IsUserDataLoaded
	{
		get
		{
			return isUserDataLoaded;
		}
	}

	public List<FacebookUserInfo> Friends
	{
		get
		{
			return SPFacebook.instance.friendsList;
		}
	}

	public bool IsFriendDataLoaded
	{
		get
		{
			if(SPFacebook.instance.friends != null)
			{
				return true;
			}
			else
			{
				return false;
			}

		}
	}

	public FacebookUserInfo PlayerInfo
	{
		get
		{
			return SPFacebook.instance.userInfo;
		}
	}

	#endregion Properties
}
