using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


/// <summary>
/// This class UserProfile is automatic generated.
/// 
/// Implement your persistant data calss here
/// </summary>
[Serializable]
public class UserProfile : PersistantMetaData
{
	//Declare your attributes here

	public string userName;

	public string password;

	public string uid;

	//Declare your function here

	/// <summary>
	/// Load data.
	/// </summary>
	public static UserProfile Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<UserProfile>())
		{
			//create new one
			UserProfile newFile = new UserProfile();
			
			SaveLoadManager.SharedManager.Save(newFile);
			
			return newFile;
		}
		
		return SaveLoadManager.SharedManager.Load<UserProfile> ();
	}

	/// <summary>
	/// Save data.
	/// </summary>
	/// <param name="data">Data.</param>
	public static bool Save(UserProfile data)
	{
		return SaveLoadManager.SharedManager.Save (data);
		
		
	}
}