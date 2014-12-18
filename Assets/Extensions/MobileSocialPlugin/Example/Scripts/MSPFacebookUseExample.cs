////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class MSPFacebookUseExample : MonoBehaviour {
	
	
	private static bool IsUserInfoLoaded = false;
	private static bool IsFrindsInfoLoaded = false;
	private static bool IsAuntifivated = false;
	
	
	
	public DefaultPreviewButton[] ConnectionDependedntButtons;
	
	public DefaultPreviewButton connectButton;
	public SA_Texture avatar;
	public SA_Label Location;
	public SA_Label Language;
	public SA_Label Mail;
	public SA_Label Name;
	
	
	public SA_Label f1;
	public SA_Label f2;
	
	public SA_Texture fi1;
	public SA_Texture fi2;
	
	
	public Texture2D ImageToShare;
	
	public GameObject friends;
	
	private int startScore = 555;
	
	
	void Awake() {
		
		
		SPFacebook.instance.addEventListener(FacebookEvents.FACEBOOK_INITED, 			 OnInit);
		SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	 OnAuth);
		
		
		SPFacebook.instance.addEventListener(FacebookEvents.USER_DATA_LOADED,  			OnUserDataLoaded);
		SPFacebook.instance.addEventListener(FacebookEvents.USER_DATA_FAILED_TO_LOAD,   OnUserDataLoadFailed);
		
		SPFacebook.instance.addEventListener(FacebookEvents.FRIENDS_DATA_LOADED,  			OnFriendsDataLoaded);
		SPFacebook.instance.addEventListener(FacebookEvents.FRIENDS_FAILED_TO_LOAD,   		OnFriendDataLoadFailed);
		
		SPFacebook.instance.addEventListener(FacebookEvents.POST_FAILED,  			OnPostFailed);
		SPFacebook.instance.addEventListener(FacebookEvents.POST_SUCCEEDED,   		OnPost);
		
		
		SPFacebook.instance.addEventListener(FacebookEvents.GAME_FOCUS_CHANGED,   OnFocusChanged);
		
		//scores Api events
		SPFacebook.instance.addEventListener(FacebookEvents.PLAYER_SCORES_REQUEST_COMPLETE,   OnPlayerScoreRequestComplete);
		SPFacebook.instance.addEventListener(FacebookEvents.APP_SCORES_REQUEST_COMPLETE,   	  OnAppScoreRequestComplete);
		SPFacebook.instance.addEventListener(FacebookEvents.SUBMIT_SCORE_REQUEST_COMPLETE,    OnSubmitScoreRequestComplete);
		SPFacebook.instance.addEventListener(FacebookEvents.DELETE_SCORES_REQUEST_COMPLETE,   OnDeleteScoreRequestComplete);
		
		
		
		SPFacebook.instance.Init();
		
		
		
		SA_StatusBar.text = "initializing Facebook";
		
		
		
	}
	
	void FixedUpdate() {
		if(IsAuntifivated) {
			connectButton.text = "Disconnect";
			Name.text = "Player Connected";
			foreach(DefaultPreviewButton btn in ConnectionDependedntButtons) {
				btn.EnabledButton();
			}
		} else {
			foreach(DefaultPreviewButton btn in ConnectionDependedntButtons) {
				btn.DisabledButton();
			}
			connectButton.text = "Connect";
			Name.text = "Player Disconnected";
			
			friends.SetActive(false);
			return;
		}
		
		if(IsUserInfoLoaded) {
			if(SPFacebook.instance.userInfo.GetProfileImage(FacebookProfileImageSize.square) != null) {
				avatar.texture = SPFacebook.instance.userInfo.GetProfileImage(FacebookProfileImageSize.square);
				Name.text = SPFacebook.instance.userInfo.name + " aka " + SPFacebook.instance.userInfo.username;
				Location.text = SPFacebook.instance.userInfo.location;
				Language.text = SPFacebook.instance.userInfo.locale;
			}
		}
		
		
		if(IsFrindsInfoLoaded) {
			friends.SetActive(true);
			int i = 0;
			foreach(FacebookUserInfo friend in SPFacebook.instance.friendsList) {
				
				if(i == 0) {
					f1.text = friend.name;
					if(friend.GetProfileImage(FacebookProfileImageSize.square) != null) {
						fi1.texture = friend.GetProfileImage(FacebookProfileImageSize.square);
					} 
				} else {
					f2.text = friend.name;
					if(friend.GetProfileImage(FacebookProfileImageSize.square) != null) {
						fi2.texture = friend.GetProfileImage(FacebookProfileImageSize.square);
					} 
				}
				
				i ++;
			}
		} else {
			friends.SetActive(false);
		}
		
		
		
	}


	private void TestPayment() {
		SPFacebook.instance.Pay ("YOUR_PRODUCT_URL", 2);
	}


	
	private void PostWithAuthCheck() {
		SPFacebook.instance.PostWithAuthCheck (
			link: "https://example.com/myapp/?storyID=thelarch",
			linkName: "The Larch",
			linkCaption: "I thought up a witty tagline about larches",
			linkDescription: "There are a lot of larch trees around here, aren't there?",
			picture: "https://example.com/myapp/assets/1/larch.jpg"
			);
	}
	
	
	private void PostNativeScreenshot() {
		StartCoroutine(PostFBScreenshot());
	}
	
	
	private void PostImage() {
		SPShareUtility.FacebookShare("This is my text to share", ImageToShare);
	}


	private void PostMSG() {
		SPShareUtility.FacebookShare("This is my text to share");
	}
	
	
	
	private IEnumerator PostFBScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		SPShareUtility.FacebookShare("This is my text to share", tex);
		
		
		Destroy(tex);
		
	}
	
	
	private void Connect() {
		if(!IsAuntifivated) {
			SPFacebook.instance.Login("email,publish_actions");
			SA_StatusBar.text = "Log in...";
		} else {
			LogOut();
			SA_StatusBar.text = "Logged out";
		}
	}
	
	private void LoadUserData() {
		SPFacebook.instance.LoadUserData();
		SA_StatusBar.text = "Loadin user data..";
	}
	
	private void PostMessage() {
		SPFacebook.instance.Post (
			link: "https://example.com/myapp/?storyID=thelarch",
			linkName: "The Larch",
			linkCaption: "I thought up a witty tagline about larches",
			linkDescription: "There are a lot of larch trees around here, aren't there?",
			picture: "https://example.com/myapp/assets/1/larch.jpg"
			);
		
		SA_StatusBar.text = "Positng..";
	}
	
	private void PostScreehShot() {
		StartCoroutine(PostScreenshot());
		SA_StatusBar.text = "Positng..";
	}
	
	private void LoadFriends() {
		SPFacebook.instance.LoadFrientdsInfo(5);
		SA_StatusBar.text = "Loading friends..";
	}
	
	private void AppRequest() {
		SPFacebook.instance.AppRequest("Come play this great game!");
	}
	
	
	private void LoadScore() {
		SPFacebook.instance.LoadPlayerScores();
	}
	
	private void LoadAppScores() {
		SPFacebook.instance.LoadAppScores();
	}
	
	public void SubmitScore() {
		startScore++;
		SPFacebook.instance.SubmitScore(startScore);
	}
	
	
	public void DeletePlayerScores() {
		SPFacebook.instance.DeletePlayerScores();
	}
	
	public void LikePage() {
		Application.OpenURL("https://www.facebook.com/unionassets");
	}
	
	
	private string UNION_ASSETS_PAGE_ID = "1435528379999137";
	public void CheckLike() {
		
		//checking if current user likes the page
		
		
		bool IsLikes = SPFacebook.instance.IsUserLikesPage(SPFacebook.instance.UserId, UNION_ASSETS_PAGE_ID);
		if(IsLikes) {
			SA_StatusBar.text ="Current user Likes union assets";
		} else {
			//user do not like the page or we han't yet downloaded likes data
			//downloading likes for this page
			SPFacebook.instance.addEventListener(FacebookEvents.LIKES_LIST_LOADED, OnLikesLoaded);
			SPFacebook.instance.LoadLikes(SPFacebook.instance.UserId, UNION_ASSETS_PAGE_ID);
		}
		
		
	}
	
	// --------------------------------------
	// EVENTS
	// --------------------------------------


	private void OnLikesLoaded() {
		//The likes is loaded so now we can find out for sure if user is like our page
		bool IsLikes = SPFacebook.instance.IsUserLikesPage(SPFacebook.instance.UserId, UNION_ASSETS_PAGE_ID);
		if(IsLikes) {
			SA_StatusBar.text ="Current user Likes union assets";
		} else {
			SA_StatusBar.text ="Current user does not like union assets";
		}
	}

	
	private void OnFocusChanged(CEvent e) {
		bool focus = (bool) e.data;
		
		if (!focus)  {                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		} else  {                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}   
	}
	
	
	private void OnUserDataLoadFailed() {
		SA_StatusBar.text ="Opps, user data load failed, something was wrong";
		Debug.Log("Opps, user data load failed, something was wrong");
	}
	
	
	private void OnUserDataLoaded() {
		SA_StatusBar.text = "User data loaded";
		IsUserInfoLoaded = true;
		SPFacebook.instance.userInfo.LoadProfileImage(FacebookProfileImageSize.square);
	}
	
	private void OnFriendDataLoadFailed() {
		SA_StatusBar.text = "Opps, friends data load failed, something was wrong";
		Debug.Log("Opps, friends data load failed, something was wrong");
	}
	
	private void OnFriendsDataLoaded() {
		SA_StatusBar.text = "Friends data loaded";
		foreach(FacebookUserInfo friend in SPFacebook.instance.friendsList) {
			friend.LoadProfileImage(FacebookProfileImageSize.square);
		}
		
		IsFrindsInfoLoaded = true;
	}
	
	
	
	
	private void OnInit() {
		
		if(SPFacebook.instance.IsLoggedIn) {
			OnAuth();
		} else {
			SA_StatusBar.text = "user Login -> fale";
		}
	}
	
	
	private void OnAuth() {
		IsAuntifivated = true;
		SA_StatusBar.text = "user Login -> true";
	}
	
	private void OnPost() {
		SA_StatusBar.text = "Posting complete";
	}
	
	private void OnPostFailed() {
		SA_StatusBar.text = "Opps, post failed, something was wrong";
		Debug.Log("Opps, post failed, something was wrong");
	}
	
	//scores Api events
	private void OnPlayerScoreRequestComplete(CEvent e) {
		FB_APIResult result = e.data as FB_APIResult;
		
		if(result.IsSucceeded) {
			string msg = "Player has scores in " + SPFacebook.instance.userScores.Count + " apps" + "\n";
			msg += "Current Player Score = " + SPFacebook.instance.GetCurrentPlayerIntScoreByAppId(FB.AppId);
			SA_StatusBar.text = msg;
			
		} else {
			SA_StatusBar.text = result.responce;
		}
		
		
	}
	
	private void OnAppScoreRequestComplete(CEvent e) {
		FB_APIResult result = e.data as FB_APIResult;
		
		if(result.IsSucceeded) {
			string msg = "Loaded " + SPFacebook.instance.appScores.Count + " scores results" + "\n";
			msg += "Current Player Score = " + SPFacebook.instance.GetScoreByUserId(FB.UserId);
			SA_StatusBar.text = msg;
			
		} else {
			SA_StatusBar.text = result.responce;
		}
		
	}
	
	private void OnSubmitScoreRequestComplete(CEvent e) {
		
		FB_APIResult result = e.data as FB_APIResult;
		if(result.IsSucceeded) {
			string msg = "Score successfully submited" + "\n";
			msg += "Current Player Score = " + SPFacebook.instance.GetScoreByUserId(FB.UserId);
			SA_StatusBar.text = msg;
			
		} else {
			SA_StatusBar.text = result.responce;
		}
		
		
	}
	
	private void OnDeleteScoreRequestComplete(CEvent e) {
		FB_APIResult result = e.data as FB_APIResult;
		if(result.IsSucceeded) {
			string msg = "Score successfully deleted" + "\n";
			msg += "Current Player Score = " + SPFacebook.instance.GetScoreByUserId(FB.UserId);
			SA_StatusBar.text = msg;
			
		} else {
			SA_StatusBar.text = result.responce;
		}
		
		
	}
	
	
	
	
	
	
	
	// --------------------------------------
	// PRIVATE METHODS
	// --------------------------------------
	
	// --------------------------------------
	// PRIVATE METHODS
	// --------------------------------------
	
	private IEnumerator PostScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		SPFacebook.instance.PostImage("My app ScreehShot", tex);;
		
		Destroy(tex);
		
	}
	
	private void LogOut() {
		IsUserInfoLoaded = false;
		
		IsAuntifivated = false;
		
		SPFacebook.instance.Logout();
	}
	
	
	
}
