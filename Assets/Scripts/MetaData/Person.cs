using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// This is an example of structure of saving data
/// </summary>
[Serializable]
public class Person : PersistantMetaData 
{

	public string personName;
	public int age;
	public List<string> items;
	public Dictionary<string, int> books;
}
