using UnityEngine;
using System.Collections;

/// <summary>
/// Level manager.
/// 
/// This class handle level data sync
/// Before any action for retrieving data of level from disk
/// you should make a sync from LevelManager
/// </summary>
public class LevelManager
{
	//max sub level per level
	public const int maxSubPerLevel = 5;

	private static LevelManager instance;

	public void SyncWithMainLevel(int mainLevel)
	{
		if(mainLevel <= 0)
		{
			Debug.LogError("Can't sync level, level must above equal than 1");
			return;
		}

		SubLevelData slData = SubLevelData.Load ();

		//if main level is 1
		if(mainLevel == 1)
		{
			//always unlock level1-1 if it is not unlocked
			if(!slData.IsSubLevelUnlocked(mainLevel, 1))
			{
				slData.UnlockSubLevel(mainLevel, 1);
			}
		}
		else
		{
			//unlock first sub level of this main level if previous sub level are all unlocked from previous main level
			bool unlockFirstLevel = true;

			for(int i=1; i<=maxSubPerLevel; i++)
			{
				if(!slData.IsSubLevelUnlocked(mainLevel-1, i))
				{
					unlockFirstLevel = false;

					break;
				}
			}

			if(unlockFirstLevel)
			{
				slData.UnlockSubLevel(mainLevel, 1);
			}
		}
	}

	public static LevelManager SharedLevelManager
	{
		get
		{
			if(instance == null)
			{
				instance = new LevelManager();
			}

			return instance;
		}
	}
}
