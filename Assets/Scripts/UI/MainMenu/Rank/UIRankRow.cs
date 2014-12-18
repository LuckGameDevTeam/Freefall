using UnityEngine;
using System.Collections;

public class UIRankRow : MonoBehaviour 
{
	public Texture defaultAvatar;

	/// <summary>
	/// The player avatar.
	/// </summary>
	public UITexture playerAvatarTexture;

	/// <summary>
	/// The player name label.
	/// </summary>
	public UILabel playerNameLabel;

	/// <summary>
	/// The player mile label.
	/// </summary>
	public UILabel playerMileLabel;

	private RankInfo playerRankInfo;

	void OnDisable()
	{
		if(playerRankInfo != null)
		{
			playerRankInfo.Evt_OnFetchPlayerAvatarSuccess -= OnImageLoaded;
		}

		playerRankInfo = null;
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Sets the info.
	/// For FB
	/// give null to set it as default, otherwise give data
	/// </summary>
	public void SetInfo(RankInfo info = null)
	{
		gameObject.SetActive (true);

		if(info != null)
		{
			playerRankInfo = info;
			
			playerNameLabel.text = info.playerName;
			
			playerMileLabel.text = info.playerMile.ToString ()+"m";
			
			playerRankInfo.Evt_OnFetchPlayerAvatarSuccess += OnImageLoaded;
			
			playerRankInfo.FetchPlayerAvatar ();
		}
		else
		{
			playerNameLabel.text = "";

			playerMileLabel.text = "";

			if(defaultAvatar != null)
			{
				playerAvatarTexture.mainTexture = defaultAvatar;
			}
			else
			{
				DebugEx.DebugError(gameObject.name+" did not have default avatar assigned");
			}
		}
	}

	/// <summary>
	/// Sets the world rank info.
	/// If any of params is null it will not show info
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="mile">Mile.</param>
	public void SetWorldRankInfo(string name = null, string mile = null)
	{
		gameObject.SetActive (true);

		if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mile))
		{
			playerNameLabel.text = "";
			
			playerMileLabel.text = "";
		}
		else
		{
			playerNameLabel.text = name;
			
			playerMileLabel.text = mile+"m";
		}


		if(defaultAvatar != null)
		{
			playerAvatarTexture.mainTexture = defaultAvatar;
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" did not have default avatar assigned");
		}
	}

	void OnImageLoaded(RankInfo rankInfo, Texture playerAvatar)
	{
		playerRankInfo.Evt_OnFetchPlayerAvatarSuccess -= OnImageLoaded;

		if(gameObject.activeInHierarchy)
		{
			playerAvatarTexture.mainTexture = playerAvatar;
		}

	}

}
