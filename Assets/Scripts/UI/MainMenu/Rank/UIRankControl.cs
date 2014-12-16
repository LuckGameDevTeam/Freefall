using UnityEngine;
using System.Collections;

public enum RankType
{
	FBRank,
	WorkRank
}

public class UIRankControl : MonoBehaviour 
{
	public delegate void EventOnRankClose(UIRankControl control);
	public EventOnRankClose Evt_OnRankClose;

	public UIFBRankControl fbRankControl;
	public UIWorldRankControl worldRankControl;

	public GameObject fbRankBtn;
	public GameObject worldRankBtn;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowRankWithRankType(RankType typeOfRank = RankType.FBRank)
	{
		gameObject.SetActive (true);

		switch(typeOfRank)
		{
		case RankType.FBRank:

			fbRankBtn.GetComponent<UIShopSwitchButton> ().Select ();
			worldRankBtn.GetComponent<UIShopSwitchButton> ().Deselect ();

			fbRankControl.gameObject.SetActive(true);
			worldRankControl.gameObject.SetActive(false);

			fbRankControl.ShowRank();

			break;

		case RankType.WorkRank:

			worldRankBtn.GetComponent<UIShopSwitchButton> ().Select ();
			fbRankBtn.GetComponent<UIShopSwitchButton> ().Deselect ();

			worldRankControl.gameObject.SetActive(true);
			fbRankControl.gameObject.SetActive(false);

			//show world rank
			worldRankControl.ShowRank();

			break;
		}
	}

	public void ShowFBRank()
	{
		ShowRankWithRankType (RankType.FBRank);
	}

	public void ShowWorldRank()
	{
		ShowRankWithRankType (RankType.WorkRank);
	}

	public void CloseRank()
	{
		gameObject.SetActive (false);

		if(Evt_OnRankClose != null)
		{
			Evt_OnRankClose(this);
		}
	}
}
