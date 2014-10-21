using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Sub level data.
/// 
/// This class used to remember which sub level in game was unlocked or not.
/// 
/// As well as, it's score and unlock sublevel
/// </summary>

[Serializable]
public class SubLevelData : PersistantMetaData 
{
	public const string levelPrefix = "Level";

	/// <summary>
	/// The sub level data.
	/// contain unlocked sub level and 
	/// it's score
	/// </summary>
	public Dictionary<string, int> subLevelData = new Dictionary<string, int>();

	/// <summary>
	/// Load data.
	/// </summary>
	public static SubLevelData Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<SubLevelData>())
		{
			//create new one
			SubLevelData newFile = new SubLevelData();
			
			newFile.Init();
			
			SaveLoadManager.SharedManager.Save(newFile);
			
			return newFile;
		}
		
		return SaveLoadManager.SharedManager.Load<SubLevelData> ();
	}

	/// <summary>
	/// Gets the level title.
	/// </summary>
	/// <returns>The level title.</returns>
	/// <param name="mainLevelNumber">Main level number.</param>
	/// <param name="subLevelNumber">Sub level number.</param>
	public static string GetLevelTitle(int mainLevelNumber, int subLevelNumber)
	{
		return levelPrefix + mainLevelNumber.ToString () + "-" + subLevelNumber.ToString ();
	}

	/// <summary>
	/// Init.
	/// </summary>
	private void Init()
	{
		//always unlock level1-1
		string levelTitle = GetLevelTitle (1, 1);
		
		if(!subLevelData.ContainsKey(levelTitle))
		{
			subLevelData.Add(levelTitle, 0);
		}
	}

	/// <summary>
	/// Determines whether sub level is unlocked or not.
	/// </summary>
	/// <returns><c>true</c> if this instance is sub level unlocked the specified mainLevelNumber SubLevelNumber; otherwise, <c>false</c>.</returns>
	/// <param name="mainLevelNumber">Main level number.</param>
	/// <param name="SubLevelNumber">Sub level number.</param>
	public bool IsSubLevelUnlocked(int mainLevelNumber, int SubLevelNumber)
	{
		string levelTitle = GetLevelTitle (mainLevelNumber, SubLevelNumber);

		return subLevelData.ContainsKey (levelTitle);
	}

	/// <summary>
	/// Unlocks the sub level.
	/// </summary>
	/// <returns><c>true</c>, if sub level was unlocked, <c>false</c> otherwise.</returns>
	/// <param name="mainLevelNumber">Main level number.</param>
	/// <param name="subLevelNumber">Sub level number.</param>
	/// <param name="score">Score.</param>
	public bool UnlockSubLevel(int mainLevelNumber, int subLevelNumber, int score = 0)
	{
		string levelTitle = GetLevelTitle (mainLevelNumber, subLevelNumber);

		if(subLevelData.ContainsKey(levelTitle))
		{
			return true;
		}
		else
		{
			subLevelData.Add(levelTitle, score);
		}

		return Save(this);
	}

	/// <summary>
	/// Saves the score for sub level.
	/// </summary>
	/// <returns><c>true</c>, if score for sub level was saved, <c>false</c> otherwise.</returns>
	/// <param name="mainLevelNumber">Main level number.</param>
	/// <param name="subLevelNumber">Sub level number.</param>
	/// <param name="score">Score.</param>
	public bool SaveScoreForSubLevel(int mainLevelNumber, int subLevelNumber, int score = 0)
	{
		string levelTitle = GetLevelTitle (mainLevelNumber, subLevelNumber);

		if(subLevelData.ContainsKey(levelTitle))
		{
			subLevelData[levelTitle] = score;

			return Save(this);
		}

		return false;
	}

	/// <summary>
	/// Gets the sub level score.
	/// </summary>
	/// <returns>The sub level score.</returns>
	/// <param name="mainLevelNumber">Main level number.</param>
	/// <param name="SubLevelNumber">Sub level number.</param>
	public int GetSubLevelScore(int mainLevelNumber, int SubLevelNumber)
	{
		string levelTitle = GetLevelTitle (mainLevelNumber, SubLevelNumber);

		if(subLevelData.ContainsKey(levelTitle))
		{
			return subLevelData[levelTitle];
		}
		else
		{
			//sub level not unlock
			return 0;
		}
	}

	/// <summary>
	/// Save data.
	/// </summary>
	/// <param name="data">Data.</param>
	private bool Save(SubLevelData data)
	{
		return SaveLoadManager.SharedManager.Save (data);
	}

}
