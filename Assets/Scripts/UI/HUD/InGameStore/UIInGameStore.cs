using UnityEngine;
using System.Collections;

/// <summary>
/// UI in game store.
/// 
/// This class control the store in game
/// </summary>
public class UIInGameStore : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowInGameStore()
	{
		gameObject.SetActive (true);
	}

	public void CloseInGameStore()
	{
		gameObject.SetActive (false);
	}
}
