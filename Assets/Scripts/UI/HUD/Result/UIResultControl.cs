using UnityEngine;
using System.Collections;

/// <summary>
/// UI result control.
/// 
/// This class control Result UI
/// </summary>
public class UIResultControl : MonoBehaviour 
{
	public delegate void EventConfirmButtonClick(UIResultControl control);
	/// <summary>
	/// Event when confirm button click.
	/// </summary>
	public EventConfirmButtonClick Evt_ConfirmButtonClick;

	public delegate void EventRestartButtonClick(UIResultControl control);
	/// <summary>
	/// Event when restart button click.
	/// </summary>
	public EventRestartButtonClick Evt_RestartButtonClick;
	
	/// <summary>
	/// The result start fills.
	/// </summary>
	public GameObject[] resultStartFills;

	/// <summary>
	/// The mile result label.
	/// </summary>
	public UILabel mileResultLabel;

	/// <summary>
	/// The coin earn label.
	/// </summary>
	public UILabel coinEarnLabel;

	/// <summary>
	/// The fish bone earn label.
	/// </summary>
	public UILabel fishBoneEarnLabel;

	/// <summary>
	/// The spin light.
	/// </summary>
	public GameObject spinLight;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Confirm button click.
	/// </summary>
	public void Confirm()
	{
		if(Evt_ConfirmButtonClick != null)
		{
			Evt_ConfirmButtonClick(this);
		}

	}

	/// <summary>
	/// Restart button click.
	/// </summary>
	public void Restart()
	{
		if(Evt_RestartButtonClick != null)
		{
			Evt_RestartButtonClick(this);
		}

	}

	/// <summary>
	/// Closes result.
	/// </summary>
	public void CloseResult()
	{
		//set gameobject not active
		gameObject.SetActive (false);
	}

	/// <summary>
	/// Shows the result.
	/// </summary>
	/// <param name="success">If set to <c>true</c> success.</param>
	/// <param name="score">Score.</param>
	/// <param name="distance">Distance.</param>
	/// <param name="coinEarn">Coin earn.</param>
	/// <param name="fishBoneEarn">Fish bone earn.</param>
	public void ShowResult(bool success, int score, int distance, int coinEarn, int fishBoneEarn)
	{
		//set gameobject active
		gameObject.SetActive (true);

		//check if player success or fail
		if(success)
		{
			//player success active spin light
			spinLight.SetActive(true);
		}
		else
		{
			//player fail not active spin light
			spinLight.SetActive(false);
		}

		//if stars greater or equal to score
		if(resultStartFills.Length >= score)
		{
			//set all stars not active
			for(int i=0; i<resultStartFills.Length; i++)
			{
				resultStartFills[i].SetActive(false);
			}

			//set stars active depend on score
			for(int i=0; i<score; i++)
			{
				resultStartFills[i].SetActive(true);
			}
		}

		//set disatance label
		mileResultLabel.text = distance.ToString ();

		//set coin earn label
		coinEarnLabel.text = coinEarn.ToString ();

		//set fish bone label
		fishBoneEarnLabel.text = fishBoneEarn.ToString ();
	}
}
