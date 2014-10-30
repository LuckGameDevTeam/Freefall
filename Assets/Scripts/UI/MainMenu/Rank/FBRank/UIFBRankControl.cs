using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIFBRankControl : MonoBehaviour 
{
	/// <summary>
	/// The rows for each players.
	/// </summary>
	public UIRankRow[] rows;

	private FBController fbController;

	void Awake()
	{
		fbController = GameObject.FindObjectOfType (typeof(FBController)) as FBController;
	}
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	#region Public interface

	public void ShowRank()
	{
		if(rows.Length <= 0)
		{
			Debug.LogError(gameObject.name+"Unable to show rank in row, there is no row assigned");

			return;
		}

		//deactive all rows
		for(int i=0; i<rows.Length; i++)
		{
			rows[i].gameObject.SetActive(false);
		}

		if(fbController.IsLogin)
		{
			fbController.Evt_OnNumberOfFriendReceived += OnNumberOfFriendReceived;
			
			fbController.GetNumberOfFriend ();
		}
		else
		{
			fbController.Evt_OnFacebookLoginSuccess += OnFacebookLoginSuccess;
			fbController.Evt_OnFacebookLoginFail += OnFacebookLoginFail;

			fbController.Login();
		}

	}

	#endregion Public interface

	#region Internal

	private void DoShowRank(List<FacebookUserInfo> friendList)
	{
		List<RankInfo> rankInfoList = new List<RankInfo> ();

		for(int i=0; i<friendList.Count; i++)
		{
			FacebookUserInfo info = friendList[i];

			RankInfo rInfo = new RankInfo(info.id, info.name, fbController.GetScoreById(info.id), info);

			rankInfoList.Add(rInfo);
		}

		//add player
		RankInfo pInfo = new RankInfo (fbController.PlayerInfo.id, fbController.PlayerInfo.name, fbController.GetScoreById (fbController.PlayerInfo.id), fbController.PlayerInfo);
		rankInfoList.Add (pInfo);

		//sort list
		rankInfoList = rankInfoList.OrderByDescending (v => v.playerMile).ToList ();

		//check if player is in top 3 or not
		bool playerInTop3 = false;
		for(int i=0; i<rankInfoList.Count; i++)
		{
			if(fbController.PlayerInfo.id == rankInfoList[i].playerId)
			{
				playerInTop3 = true;

				break;
			}
		}

		//player is in top 3 get 4 ranks, otherwise get 3 ranks and add player to last
		if(playerInTop3)
		{
			Debug.Log("Player is in top 3");

			//max of info is 4
			if(rankInfoList.Count > 4)
			{
				//get top 4
				rankInfoList.RemoveRange(4, rankInfoList.Count-4);
			}
		}
		else
		{
			Debug.Log("Player is not in top 3");

			//max of info is 3
			if(rankInfoList.Count > 3)
			{
				//get top 3 by remove rest
				rankInfoList.RemoveRange(3, rankInfoList.Count-3);
			}


			//add player to last
			rankInfoList.Add(pInfo);
		}

		//display rows
		for(int i=0; i<rows.Length; i++)
		{
			UIRankRow displayRow = rows[i];

			if(i < rankInfoList.Count)
			{
				displayRow.SetInfo(rankInfoList[i]);
			}
			else
			{
				displayRow.SetInfo();
			}
		}
	}

	private void DoShowSelfRank()
	{
		//display rows
		for(int i=0; i<rows.Length; i++)
		{
			UIRankRow displayRow = rows[i];
			
			if(i == (rows.Length-1))
			{
				displayRow.SetInfo(new RankInfo(fbController.PlayerInfo.id, fbController.PlayerInfo.name, fbController.GetScoreById (fbController.PlayerInfo.id), fbController.PlayerInfo));
			}
			else
			{
				displayRow.SetInfo();
			}
		}
	}

	#endregion Internal

	#region FBController callback

	void OnFacebookLoginSuccess(FBController controller)
	{
		fbController.Evt_OnFacebookLoginSuccess -= OnFacebookLoginSuccess;
		fbController.Evt_OnFacebookLoginFail -= OnFacebookLoginFail;

		ShowRank ();
	}

	void OnFacebookLoginFail(FBController controll)
	{
		fbController.Evt_OnFacebookLoginSuccess -= OnFacebookLoginSuccess;
		fbController.Evt_OnFacebookLoginFail -= OnFacebookLoginFail;

		Debug.Log(gameObject.name+" unable to login to facebook");
	}

	void OnNumberOfFriendReceived(FBController controller, int number)
	{
		fbController.Evt_OnNumberOfFriendReceived -= OnNumberOfFriendReceived;

		fbController.Evt_OnFriendDataLoaded += OnFriendDataLoaded;
		fbController.Evt_OnFriendDataFailToLoad += OnFriendDataFailToLoad;

		fbController.LoadFriendData (number);
	}

	void OnFriendDataLoaded(FBController controller, List<FacebookUserInfo> friendList)
	{
		if(friendList != null)
		{
			fbController.Evt_OnFriendDataLoaded -= OnFriendDataLoaded;
			fbController.Evt_OnFriendDataFailToLoad -= OnFriendDataFailToLoad;
			
			DoShowRank (friendList);
		}
		else
		{
			DoShowSelfRank();
		}

	}

	void OnFriendDataFailToLoad(FBController controller)
	{
		fbController.Evt_OnFriendDataLoaded -= OnFriendDataLoaded;
		fbController.Evt_OnFriendDataFailToLoad -= OnFriendDataFailToLoad;

		Debug.Log ("can't show rank unable to load friend data");

		DoShowSelfRank ();
	}

	#endregion FBController callback
}
