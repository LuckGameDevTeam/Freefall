using UnityEngine;
using System.Collections;

/// <summary>
/// UI star control.
/// </summary>
public class UIStarControl : MonoBehaviour 
{
	/// <summary>
	/// The star covers.
	/// </summary>
	public GameObject[] starCovers;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void UpdateStarsWithScroe(int score)
	{
		DebugEx.Debug ("update star: " + score);

		//disable all cover stars
		for(int i=0; i<starCovers.Length; i++)
		{
			starCovers[i].SetActive(false);
		}

		//enable cover stars dependen on score
		for(int i=0; i<score; i++)
		{
			starCovers[i].SetActive(true);
		}
	}
}
