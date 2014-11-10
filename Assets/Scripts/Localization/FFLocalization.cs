using UnityEngine;
using System.Collections;

public class FFLocalization : MonoBehaviour 
{
	public TextAsset localizationFile;

	public bool debugEnabled = false;

	void Awake()
	{
		if(localizationFile != null)
		{
			if(Localization.LoadCSV (localizationFile))
			{
				Debug.Log("Load CSV localization file success");

				if(debugEnabled)
				{
					Debug.Log("Number of know languages: "+Localization.knownLanguages.Length);

					foreach(string lang in Localization.knownLanguages)
					{
						Debug.Log(lang);
					}
				}
			}
			else
			{
				Debug.LogError("Load CSV localization file fail");
			}
		}
		else
		{
			Debug.LogError(gameObject.name+" localization file not assigned");
		}

	}

	// Use this for initialization
	void Start () 
	{
		if(localizationFile != null)
		{
			//load last saved language
			LanguageSetting ls = LanguageSetting.Load ();

			//set to current language
			Localization.language = ls.currentLanguage;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
