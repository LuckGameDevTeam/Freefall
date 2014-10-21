using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Language setting.
/// 
/// This class used to remember the language setting.
/// 
/// Default is Chinese-Traditional
/// 
/// Remember assigned the name that as same as the localization file name
/// when you save, otherwise it will not match localization file.
/// </summary>

[Serializable]
public class LanguageSetting : PersistantMetaData 
{
	/// <summary>
	/// The current language.
	/// </summary>
	public string currentLanguage = "Chinese-Traditional";

	public static LanguageSetting Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<LanguageSetting>())
		{
			//create new one
			LanguageSetting newFile = new LanguageSetting();
			
			SaveLoadManager.SharedManager.Save(newFile);
			
			return newFile;
		}
		
		return SaveLoadManager.SharedManager.Load<LanguageSetting> ();
	}
	
	public static bool Save(LanguageSetting data)
	{
		return SaveLoadManager.SharedManager.Save (data);
	}
}
