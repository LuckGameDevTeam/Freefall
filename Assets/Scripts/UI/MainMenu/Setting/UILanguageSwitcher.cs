using UnityEngine;
using System.Collections;

/// <summary>
/// UI language switcher.
/// 
/// This class handle switch language
/// </summary>
public class UILanguageSwitcher : MonoBehaviour 
{
	/// <summary>
	/// Reference to popup list.
	/// </summary>
	UIPopupList popupList;

	void Awake()
	{
		//find UIPopupList
		popupList = GetComponent<UIPopupList> ();

		//set this gameobject as eventReceiver to UIPopupList
		//NGUI 2.7
		//popupList.eventReceiver = gameObject;

		//NGUI 3.x.x
		EventDelegate.Set (popupList.onChange, OnSelectionChange);
	}

	void Start()
	{
		//load last saved language
		LanguageSetting ls = LanguageSetting.Load ();

		//set current language to saved language setting
		//NGUI 2.7
		//Localization.instance.currentLanguage = ls.currentLanguage;

		//NGUI 3.x.x
		Localization.language = ls.currentLanguage;
	}

	/*
	/// <summary>
	/// PopupList selection change.
	/// </summary>
	/// <param name="val">Value.</param>
	/// NGUI 2.7
	public void OnSelectionChange(string val)
	{
		//save to current language
		LanguageSetting ls = new LanguageSetting ();
		ls.currentLanguage = val;
		LanguageSetting.Save (ls);

		Localization.instance.currentLanguage = val;
	}
	*/

	public void OnSelectionChange()
	{
		//save to current language
		LanguageSetting ls = new LanguageSetting ();
		ls.currentLanguage = UIPopupList.current.value;
		LanguageSetting.Save (ls);
		
		Localization.language = UIPopupList.current.value;
	}
}
