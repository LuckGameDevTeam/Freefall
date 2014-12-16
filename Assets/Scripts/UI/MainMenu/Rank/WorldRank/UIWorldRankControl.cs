using UnityEngine;
using System.Collections;

public class UIWorldRankControl : MonoBehaviour 
{
	/// <summary>
	/// The rows for each players.
	/// </summary>
	public UIRankRow[] rows;

	// Use this for initialization
	void Start () 
	{
		
	}

	void OnEnable()
	{
		ServerSync.SharedInstance.Evt_OnGetScoreSuccess += OnGetScoreSuccess;
		ServerSync.SharedInstance.Evt_OnGetScoreFail += OnGetScoreFail;
	}

	void OnDisable()
	{
		ServerSync.SharedInstance.Evt_OnGetScoreSuccess -= OnGetScoreSuccess;
		ServerSync.SharedInstance.Evt_OnGetScoreFail -= OnGetScoreFail;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowRank()
	{
		if((rows == null) || (rows.Length <= 0))
		{
			Debug.LogError(gameObject.name+" "+"rows not assigned");

			return;
		}

		//deactive all rows
		for(int i=0; i<rows.Length; i++)
		{
			rows[i].gameObject.SetActive(false);
		}

		//get score
		ServerSync.SharedInstance.GetScore ();
	}

	#region ServerSync callback
	void OnGetScoreSuccess(ServerSync syncControl, ServerSync.PlayerScore[] scoreInfo)
	{
		for(int i=0; i<rows.Length; i++)
		{
			UIRankRow displayRow = rows[i];

			if(i < scoreInfo.Length)
			{
				ServerSync.PlayerScore ps = scoreInfo[i];

				displayRow.SetWorldRankInfo(ps.name, ps.score.ToString());
			}
			else
			{
				displayRow.SetWorldRankInfo();
			}
		}
	}

	void OnGetScoreFail(ServerSync syncControl, int errorCode)
	{

	}
	#endregion ServerSync callback
}
