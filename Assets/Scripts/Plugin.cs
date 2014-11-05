using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Plugin : MonoBehaviour 
{
#if UNITY_IPHONE || UNITY_EDITOR
	[DllImport("__Internal")]
#else
	[DllImport("MyPlugin")]
#endif
	private static extern int getValue ();


	// Use this for initialization
	void Start () 
	{
		Debug.Log ("plugin return value: " + getValue ());
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
