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
		popupList.eventReceiver = gameObject;
	}

	void Start()
	{
		//load last saved language
		LanguageSetting ls = LanguageSetting.Load ();

		//set current language to saved language setting
		Localization.instance.currentLanguage = ls.currentLanguage;

		popupList.selection = ls.currentLanguage;
	}

	/// <summary>
	/// PopupList selection change.
	/// </summary>
	/// <param name="val">Value.</param>
	public void OnSelectionChange(string val)
	{
		//save to current language
		LanguageSetting ls = new LanguageSetting ();
		ls.currentLanguage = val;
		LanguageSetting.Save (ls);

		Localization.instance.currentLanguage = val;
	}
}
