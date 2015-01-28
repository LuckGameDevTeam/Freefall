using UnityEngine;
using System.Collections;

public class UIRestoreAlert : MonoBehaviour 
{

	/// <summary>
	/// The alert title localize.
	/// </summary>
	public UILabel alertTitle;
	
	/// <summary>
	/// The alert desc localize.
	/// </summary>
	public UILabel alertDesc;

	public GameObject button;
	
	/// <summary>
	/// The error clip.
	/// </summary>
	public AudioClip errorClip;

	/// <summary>
	/// Shows the restore alert window.
	/// 
	/// titleKey, descKey are localized
	/// </summary>
	/// <param name="titleKey">Title key.</param>
	/// <param name="descKey">Desc key.</param>
	public void ShowAlertWindow(string titleKey, string descKey, bool showButton = true)
	{
		//set title
		alertTitle.text = Localization.Get (titleKey);
		
		//set desc
		alertDesc.text = Localization.Get (descKey);
		
		gameObject.SetActive (true);

		if(showButton)
		{
			button.SetActive(true);
		}
		else
		{
			button.SetActive(false);
		}
		
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
		gameObject.SetActive (false);
	}
}
