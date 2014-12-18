using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FFLocalization : MonoBehaviour 
{
	public TextAsset localizationFile;

	public bool debugEnabled = false;

	/// <summary>
	/// The language table.
	/// Mapping from Unity system language to NGUI language
	/// </summary>
	private Dictionary<string,string> langTable;

	void Awake()
	{
		//init language table
		InitLangTable ();

		//load CSV localization file
		if(localizationFile != null)
		{
			if(Localization.LoadCSV (localizationFile))
			{
				DebugEx.Debug("Load CSV localization file success");

				if(debugEnabled)
				{
					DebugEx.Debug("Number of know languages: "+Localization.knownLanguages.Length);

					foreach(string lang in Localization.knownLanguages)
					{
						DebugEx.Debug(lang);
					}
				}
			}
			else
			{
				DebugEx.DebugError("Load CSV localization file fail");
			}
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" localization file not assigned");
		}

	}

	// Use this for initialization
	void Start () 
	{
		if(localizationFile != null)
		{
			//load last saved language
			LanguageSetting ls = LanguageSetting.Load ();

			//if there is no last saved language
			if(string.IsNullOrEmpty(ls.currentLanguage))
			{
				//use Unity detected system language to get NGUI language
				string systemlang = GetLanguage(Application.systemLanguage);

				//if language not define...fallback to English
				if(string.IsNullOrEmpty(systemlang))
				{
					systemlang = GetLanguage(SystemLanguage.English);

					//save to default language
					ls.currentLanguage = systemlang;

					LanguageSetting.Save(ls);
				}
				else
				{
					//save to default language
					ls.currentLanguage = systemlang;

					LanguageSetting.Save(ls);
				}
			}

			//set to current NGUI language
			Localization.language = ls.currentLanguage;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void InitLangTable()
	{
		langTable = new Dictionary<string, string> ();

		////// table that mapping unity detected system language with NGUI localization language
		////////////// Unity system language, NGUI localization language key //////////// 
		langTable.Add ("Chinese", "Chinese-Traditional");
		langTable.Add ("English", "English");
	}

	/// <summary>
	/// Return NGUI localization langauge by given Unity detectd system language.
	/// Otherwise return null
	/// </summary>
	/// <returns>The language.</returns>
	/// <param name="sl">Sl.</param>
	private string GetLanguage(SystemLanguage sl)
	{
		string langKey = sl.ToString ();

		if(langTable.ContainsKey(langKey))
		{
			return langTable[langKey];
		}

		return null;
	}
}
