using UnityEngine;
using System.Collections;

public class UIMenuControl : MonoBehaviour 
{
	public delegate void EventOnFBLoginClick(UIMenuControl control);
	public EventOnFBLoginClick Evt_OnFBLoginClick;

	public delegate void EventOnSinglePlayerClick(UIMenuControl control);
	public EventOnSinglePlayerClick Evt_OnSinglePlayerClick;

	public delegate void EventOnRankClick(UIMenuControl control);
	public EventOnRankClick Evt_OnRankClick;

	public delegate void EventOnTutorialClick(UIMenuControl control);
	public EventOnTutorialClick Evt_OnTutorialClick;

	public delegate void EventOnSettingClick(UIMenuControl control);
	public EventOnSettingClick Evt_OnSettingClick;

	public delegate void EventOnCreditClick(UIMenuControl control);
	public EventOnCreditClick Evt_OnCreditClick;



	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowMenu()
	{
		gameObject.SetActive (true);
	}

	public void CloseMenu()
	{
		gameObject.SetActive (false);
	}

	public void FBLoginClick()
	{
		if(Evt_OnFBLoginClick != null)
		{
			Evt_OnFBLoginClick(this);
		}
	}

	public void SinglePlayerClick()
	{
		if(Evt_OnSinglePlayerClick != null)
		{
			Evt_OnSinglePlayerClick(this);
		}
	}

	public void RankClick()
	{
		if(Evt_OnRankClick != null)
		{
			Evt_OnRankClick(this);
		}
	}

	public void TutorialClick()
	{
		if(Evt_OnTutorialClick != null)
		{
			Evt_OnTutorialClick(this);
		}
	}

	public void SettingClick()
	{
		if(Evt_OnSettingClick != null)
		{
			Evt_OnSettingClick(this);
		}
	}

	public void CreditClick()
	{
		if(Evt_OnCreditClick != null)
		{
			Evt_OnCreditClick(this);
		}
	}
}
