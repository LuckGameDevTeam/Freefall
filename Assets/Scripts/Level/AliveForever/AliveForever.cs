using UnityEngine;
using System.Collections;

public class AliveForever : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad (gameObject);
	}
	

}
