using UnityEngine;
using System.Collections;

/// <summary>
/// UI danger control.
/// 
/// This class control danger sign in game
/// </summary>
public class UIDangerControl : MonoBehaviour 
{
	public delegate void EventDangerAnimationFinished(UIDangerControl control);
	/// <summary>
	/// Event when danger sign animation finished.
	/// </summary>
	public EventDangerAnimationFinished Evt_DangerAnimationFinished;

	/// <summary>
	/// The start position.
	/// </summary>
	public Vector3 start;

	/// <summary>
	/// The end position.
	/// </summary>
	public Vector3 end;

	/// <summary>
	/// The animation tweener.
	/// </summary>
	private UITweener tweener;

	/// <summary>
	/// The sound player.
	/// </summary>
	private SFXPlayer soundPlayer;
	

	void Awake()
	{
		//find animation tweener
		tweener = GetComponent<UITweener> ();

		//find sound player
		soundPlayer = GetComponent<SFXPlayer> ();
	}

	// Use this for initialization
	void Start () 
	{
		//set gameobject active
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void Reset()
	{
		//stop tweener
		tweener.enabled = false;

		//reset tweener
		tweener.Reset ();

		//set gameobject not active
		gameObject.SetActive (false);
	}

	public void PresentDangerSign()
	{
		//set gameobject active
		gameObject.SetActive (true);

		//animation start position
		Vector3 animStartPosition = new Vector3 (start.x, start.y, start.z);
		
		//animation end position
		Vector3 animEndPosition = new Vector3 (end.x, end.y, end.z);
		
		//set position to animation start position
		transform.localPosition = animStartPosition;
		
		TweenPosition tweenPos = (TweenPosition)tweener;
		
		//set animation from
		tweenPos.from = animStartPosition;
		
		//set animation to
		tweenPos.to = animEndPosition;

		//reset tweener
		tweener.Reset ();

		//play tweener
		tweener.Play (true);

		//make sound fx to loop
		soundPlayer.loop = true;

		//play sound fx
		soundPlayer.PlaySound ();

	}

	public void onAnimationDone()
	{
		//stop sound fx
		soundPlayer.StopSound ();

		//set gameobject not active
		gameObject.SetActive (false);

		if(Evt_DangerAnimationFinished != null)
		{
			Evt_DangerAnimationFinished(this);
		}
	}
}
