using UnityEngine;
using System.Collections;

public class RankInfo
{
	public delegate void EventOnFetchPlayerAvatarSuccess(RankInfo rankInfo, Texture playerAvatar);
	public EventOnFetchPlayerAvatarSuccess Evt_OnFetchPlayerAvatarSuccess;

	public string playerId;
	/// <summary>
	/// The name of the player.
	/// </summary>
	public string playerName;

	/// <summary>
	/// The player mile.
	/// </summary>
	public int playerMile;

	/// <summary>
	/// The info.
	/// </summary>
	private FacebookUserInfo info;


	public RankInfo(string id, string name, int mile, FacebookUserInfo theInfo)
	{
		playerId = id;
		playerName = name;
		playerMile = mile;
		info = theInfo;
	}

	~RankInfo()
	{
		if(info != null)
		{
			info.removeEventListener(FacebookUserInfo.PROFILE_IMAGE_LOADED, OnImageLoaded);
		}
	}

	public void FetchPlayerAvatar()
	{
		info.addEventListener(FacebookUserInfo.PROFILE_IMAGE_LOADED, OnImageLoaded);

		if(info.GetProfileImage(FacebookProfileImageSize.square) != null)
		{
			if(Evt_OnFetchPlayerAvatarSuccess != null)
			{
				Evt_OnFetchPlayerAvatarSuccess(this, info.GetProfileImage(FacebookProfileImageSize.square));
			}
		}
		else
		{
			info.LoadProfileImage (FacebookProfileImageSize.square);
		}

	}

	void OnImageLoaded()
	{
		info.removeEventListener(FacebookUserInfo.PROFILE_IMAGE_LOADED, OnImageLoaded);

		if(Evt_OnFetchPlayerAvatarSuccess != null)
		{
			Evt_OnFetchPlayerAvatarSuccess(this, info.GetProfileImage(FacebookProfileImageSize.square));
		}
	}
}
