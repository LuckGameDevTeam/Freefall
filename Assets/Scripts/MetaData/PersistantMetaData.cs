using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Persistant meta data.
/// 
/// This is generic class for data saving.
/// 
/// Any class that will be used to save data must derive from 
/// this class and Must add [Serializable] keyword before class.
/// 
/// </summary>

[Serializable]
public class PersistantMetaData
{
	/// <summary>
	/// The name of this class.
	/// 
	/// If there is an inherited class then
	/// name will be that inherited class.
	/// </summary>
	private string name;
	
	public PersistantMetaData() : base()
	{
		//get the name of this class
		name = this.GetType ().ToString ();
	}
	
}
