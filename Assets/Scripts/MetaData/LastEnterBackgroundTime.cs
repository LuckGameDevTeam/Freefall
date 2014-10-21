using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Last enter background time.
/// 
/// This class used to remember the last time application
/// enter background.
/// </summary>

[Serializable]
public class LastEnterBackgroundTime : PersistantMetaData 
{

	/// <summary>
	/// Saved years.
	/// </summary>
	public int years = 0;

	/// <summary>
	/// Saved months.
	/// </summary>
	public int months = 0;

	/// <summary>
	/// Saved days.
	/// </summary>
	public int days = 0;

	/// <summary>
	/// Saved hours.
	/// </summary>
	public int hours = 0;

	/// <summary>
	/// Saved minutes.
	/// </summary>
	public int minutes = 0;

	/// <summary>
	/// Saved seconds.
	/// </summary>
	public int seconds = 0;

	/// <summary>
	/// Determines if is file exist.
	/// </summary>
	/// <returns><c>true</c> if is file exist; otherwise, <c>false</c>.</returns>
	public static bool IsFileExist()
	{
		return SaveLoadManager.SharedManager.IsFileExist<LastEnterBackgroundTime> ();
	}

	/// <summary>
	/// Load data.
	/// </summary>
	public static LastEnterBackgroundTime Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<LastEnterBackgroundTime>())
		{
			//create new one
			LastEnterBackgroundTime newFile = new LastEnterBackgroundTime();
			
			SaveLoadManager.SharedManager.Save(newFile);
			
			return newFile;
		}
		
		return SaveLoadManager.SharedManager.Load<LastEnterBackgroundTime> ();
	}

	/// <summary>
	/// Save data.
	/// </summary>
	/// <param name="data">Data.</param>
	public static bool Save(LastEnterBackgroundTime data)
	{
		return SaveLoadManager.SharedManager.Save (data);
	}
}
