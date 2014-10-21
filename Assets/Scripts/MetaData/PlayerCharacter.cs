using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Player character.
/// 
/// This class used to remember the character player selected
/// </summary>

[Serializable]
public class PlayerCharacter : PersistantMetaData 
{
	/// <summary>
	/// This will be reference to character perfab's name
	/// This name must be as same as character prefab
	/// </summary>
	public string characterName = "";

	/// <summary>
	/// Load data.
	/// </summary>
	public static PlayerCharacter Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<PlayerCharacter>())
		{
			//create new one
			PlayerCharacter newFile = new PlayerCharacter();

			newFile.characterName = "";

			SaveLoadManager.SharedManager.Save(newFile);

			return newFile;
		}

		return SaveLoadManager.SharedManager.Load<PlayerCharacter> ();
	}

	/// <summary>
	/// Save data.
	/// </summary>
	/// <param name="data">Data.</param>
	public static bool Save(PlayerCharacter data)
	{
		return SaveLoadManager.SharedManager.Save (data);


	}
}
