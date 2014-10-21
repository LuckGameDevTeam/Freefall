using UnityEngine;
using System.Collections;

/// <summary>
/// UI mileage line control.
/// 
/// This class control mileage line
/// </summary>
public class UIMileageLineControl : MonoBehaviour 
{
	/// <summary>
	/// The mileage line label.
	/// </summary>
	public UILabel mileageLineLabel;

	/// <summary>
	/// The animation tweener.
	/// </summary>
	private UITweener tweener;

	void Awake()
	{
		//find tweener
		tweener = GetComponent<UITweener> ();
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
	/// Reset.
	/// </summary>
	public void Reset()
	{
		//enable tweener
		tweener.enabled = false;

		//reset tweener
		tweener.Reset ();

		//set gameobject not active
		gameObject.SetActive (false);
	}

	/// <summary>
	/// Show the mileage line.
	/// </summary>
	/// <param name="mileage">Mileage.</param>
	public void PresentMileageLine(int mileage)
	{
		//set gameobject active
		gameObject.SetActive (true);

		//set mileage
		mileageLineLabel.text = mileage.ToString ();

		//reset tweener
		tweener.Reset ();

		//play tweener
		tweener.Play (true);
	}

	/// <summary>
	/// Handle animation done event.
	/// </summary>
	public void OnAnimationDone()
	{
		//set gameobject not active
		gameObject.SetActive (false);
	}
}
