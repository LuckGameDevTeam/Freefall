﻿using UnityEngine;
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
	/// 
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
				Debug.LogError(gameObject.name+" did not have default avatar assigned");
			}
		}
	}

	void OnImageLoaded(RankInfo rankInfo, Texture playerAvatar)
	{
		playerRankInfo.Evt_OnFetchPlayerAvatarSuccess -= OnImageLoaded;

		playerAvatarTexture.mainTexture = playerAvatar;
	}

}