using UnityEngine;
using System.Collections;
using SIS;

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
	/// mile, coin, fishbone
	/// time for animating the current start to end value
	/// </summary>
	public float duration = 2f;

	/// <summary>
	/// Show star animation. second per star
	/// </summary>
	public float starAnimDuration = 3f;

	/// <summary>
	/// The rank control.
	/// </summary>
	public UIRankControl rankControl;

	/// <summary>
	/// The spin light.
	/// </summary>
	public GameObject spinLight;

	private FBController fbController;

	private int targetMile = 0;
	private int targetCoin = 0;
	private int targetFishBone = 0;
	private int targetScore = 0;

	void Awake()
	{
		fbController = GameObject.FindObjectOfType (typeof(FBController)) as FBController;
	}

	// Use this for initialization
	void Start () 
	{
		rankControl.CloseRank ();
	}

	void OnDestroy()
	{
		StopCoroutine ("MileCountTo");
		StopCoroutine ("CoinCountTo");
		StopCoroutine ("FishBoneCountTo");
		StopCoroutine ("StarCountTo");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowRank()
	{
		rankControl.ShowRankWithRankType (RankType.FBRank);
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

		//show mile, coin, fish bone
		//set value
		targetMile = distance;
		targetCoin = coinEarn;
		targetFishBone = fishBoneEarn;
		targetScore = score;

		//set all stars not active
		for(int i=0; i<resultStartFills.Length; i++)
		{
			resultStartFills[i].SetActive(false);
		}
		
		//set disatance label with animation
		if(mileResultLabel)
		{
			StartCoroutine ("MileCountTo", targetMile);
		}
		else
		{
			Debug.LogError(gameObject.name+" mileResultLabel not assigned");
		}



	}

	#region CountTo animation
	/// <summary>
	/// Miles count to animation.
	/// </summary>
	/// <returns>The count to.</returns>
	/// <param name="target">Target.</param>
	IEnumerator MileCountTo(int target)
	{
		//remember current value as starting position
		int start = 0;
		int curVal = 0;

		//play tween
		UITweener tweener = mileResultLabel.gameObject.GetComponent<UITweener> ();

		if(tweener)
		{
			tweener.ResetToBeginning();
			tweener.PlayForward();
		}
		else
		{
			Debug.LogError(gameObject.name+" unable to play tween, UITween component not found");
		}
		
		//over the duration defined, lerp value from start to target value
		//and set the UILabel text to this value
		for (float timer = 0; timer < duration; timer += RealTime.deltaTime)
		{

			float progress = timer / duration;
			curVal = (int)Mathf.Lerp(start, target, progress);
			mileResultLabel.text = curVal + "";

			if((target-start) <= 1)
			{
				break;
			}

			yield return null;
		}

		mileResultLabel.text = target.ToString();

		//set coin earn label with animation
		if(coinEarnLabel)
		{
			StartCoroutine ("CoinCountTo", targetCoin);
		}
		else
		{
			Debug.LogError(gameObject.name+" coinEarnLabel not assigned");
		}
	}

	/// <summary>
	/// Coins count to animation.
	/// </summary>
	/// <returns>The count to.</returns>
	/// <param name="target">Target.</param>
	IEnumerator CoinCountTo(int target)
	{
		//remember current value as starting position
		int start = 0;
		int curVal = 0;

		//play tween
		UITweener tweener = coinEarnLabel.gameObject.GetComponent<UITweener> ();
		
		if(tweener)
		{
			tweener.ResetToBeginning();
			tweener.PlayForward();
		}
		else
		{
			Debug.LogError(gameObject.name+" unable to play tween, UITween component not found");
		}
		
		//over the duration defined, lerp value from start to target value
		//and set the UILabel text to this value
		for (float timer = 0; timer < duration; timer += RealTime.deltaTime)
		{


			float progress = timer / duration;
			curVal = (int)Mathf.Lerp(start, target, progress);
			coinEarnLabel.text = curVal + "";

			if((target-start) <= 1)
			{
				break;
			}

			yield return null;
		}
		
		coinEarnLabel.text = target.ToString();

		//set fish bone label with animaiton
		if(fishBoneEarnLabel)
		{
			StartCoroutine ("FishBoneCountTo", targetFishBone);
		}
		else
		{
			Debug.LogError(gameObject.name+" fishBoneEarnLabel not assigned");
		}
	}

	/// <summary>
	/// Fishs bone count to animation.
	/// </summary>
	/// <returns>The bone count to.</returns>
	/// <param name="target">Target.</param>
	IEnumerator FishBoneCountTo(int target)
	{
		//remember current value as starting position
		int start = 0;
		int curVal = 0;

		//play tween
		UITweener tweener = fishBoneEarnLabel.gameObject.GetComponent<UITweener> ();
		
		if(tweener)
		{
			tweener.ResetToBeginning();
			tweener.PlayForward();
		}
		else
		{
			Debug.LogError(gameObject.name+" unable to play tween, UITween component not found");
		}
		
		//over the duration defined, lerp value from start to target value
		//and set the UILabel text to this value
		for (float timer = 0; timer < duration; timer += RealTime.deltaTime)
		{


			float progress = timer / duration;
			curVal = (int)Mathf.Lerp(start, target, progress);
			fishBoneEarnLabel.text = curVal + "";

			if((target-start) <= 1)
			{
				break;
			}

			yield return null;
		}
		
		fishBoneEarnLabel.text = target.ToString();

		//show stars
		//if stars greater or equal to score
		if(resultStartFills.Length >= targetScore)
		{
			
			StartCoroutine("StarCountTo", targetScore);
		}
		else
		{
			Debug.LogError(gameObject.name+" number of star is greater than score");
		}
	}

	/// <summary>
	/// Stars count to animation.
	/// </summary>
	/// <returns>The count to.</returns>
	/// <param name="score">Score.</param>
	IEnumerator StarCountTo(int score)
	{
		Debug.Log ("Player score: " + score);

		//set stars active depend on score
		for(int i=0; i<score; i++)
		{
			for (float timer = 0; timer < starAnimDuration; timer += RealTime.deltaTime)
			{
				resultStartFills[i].SetActive(true);

				UITweener[] tweeners = resultStartFills[i].GetComponents<UITweener>();

				if((tweeners != null) && (tweeners.Length > 0))
				{
					for(int j=0; j<tweeners.Length; j++)
					{
						tweeners[j].ResetToBeginning();
						tweeners[j].PlayForward();
					}
				}
				else
				{
					Debug.LogError(gameObject.name+" unable to play tweener, can not get UITweener component");
				}


			}

			yield return null;
		}
	}
	#endregion CountTo animation

	public void PostToFBWall()
	{
#if TestMode
		return;
#endif
		if(fbController.IsLogin)
		{
			fbController.Evt_OnFacebookPostSuccess += OnFacebookPostSuccess;
			fbController.Evt_OnFacebookPostFail += OnFacebookPostFail;

			fbController.PostToFacebook ();
		}
		else
		{
			fbController.Evt_OnFacebookLoginSuccess += OnFacebookLoginSuccess;
			fbController.Evt_OnFacebookLoginFail += OnFacebookLoginFail;

			fbController.Login();
		}

	}

	#region FB controller event

	void OnFacebookPostSuccess(FBController controller)
	{
		fbController.Evt_OnFacebookPostSuccess -= OnFacebookPostSuccess;
		fbController.Evt_OnFacebookPostFail -= OnFacebookPostFail;

		//StoreInventory.GiveItem (StoreAssets.PLAYER_LIFE_ITEM_ID, 1);
		DBManager.IncrementPlayerData (LifeCounter.PlayerLife, 1);
	}

	void OnFacebookPostFail(FBController controller)
	{
		fbController.Evt_OnFacebookPostSuccess -= OnFacebookPostSuccess;
		fbController.Evt_OnFacebookPostFail -= OnFacebookPostFail;

		Debug.Log("Unable to post to facebook");
	}

	void OnFacebookLoginSuccess(FBController controller)
	{
		fbController.Evt_OnFacebookLoginSuccess -= OnFacebookLoginSuccess;
		fbController.Evt_OnFacebookLoginFail -= OnFacebookLoginFail;

		PostToFBWall ();
	}

	void OnFacebookLoginFail(FBController controller)
	{
		fbController.Evt_OnFacebookLoginSuccess -= OnFacebookLoginSuccess;
		fbController.Evt_OnFacebookLoginFail -= OnFacebookLoginFail;

		Debug.Log("Unable to login to facebook");
	}
	#endregion FB controller event
}
