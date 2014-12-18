using UnityEngine;
using System.Collections;

/// <summary>
/// UI alert control.
/// 
/// This class control alert window
/// </summary>
public class UIAlertControl : MonoBehaviour 
{
	public delegate void EventCloseAlertWindow(UIAlertControl control);
	public EventCloseAlertWindow Evt_CloseAlertWindow;

	public string errorTitleKey = "ErrorTitle";
	public string errorDescKey = "ErrorDescription";

	/// <summary>
	/// The alert title localize.
	/// </summary>
	public UILocalize alertTitleLocalize;

	/// <summary>
	/// The alert desc localize.
	/// </summary>
	public UILocalize alertDescLocalize;

	/// <summary>
	/// The error clip.
	/// </summary>
	public AudioClip errorClip;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Shows the alert window.
	/// 
	/// titleKey, descKey are localized
	/// </summary>
	/// <param name="titleKey">Title key.</param>
	/// <param name="descKey">Desc key.</param>
	public void ShowAlertWindow(string titleKey = null, string descKey = null)
	{
		//set title
		alertTitleLocalize.key = titleKey;
		if(titleKey == null)
		{
			alertTitleLocalize.key = errorTitleKey;
		}

		//set desc
		alertDescLocalize.key = descKey;
		if(descKey == null)
		{
			alertDescLocalize.key = errorDescKey;
		}

		gameObject.SetActive (true);

		//play error sound
		if(errorClip != null)
		{
			NGUITools.PlaySound(errorClip);
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" unable to play error clip, error clip not assigned");
		}
	}

	/// <summary>
	/// Closes the alert window.
	/// </summary>
	public void CloseAlertWindow()
	{
		if(Evt_CloseAlertWindow != null)
		{
			Evt_CloseAlertWindow(this);
		}

		//reset localize
		alertTitleLocalize.key = errorTitleKey;
		alertDescLocalize.key = errorDescKey;

		gameObject.SetActive (false);
	}
}
