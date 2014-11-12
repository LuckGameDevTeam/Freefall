using UnityEngine;
using System.Collections;
using SIS;

public class UIUpdateGold : MonoBehaviour 
{
	public UILabel goldLabel;

	void OnEnable()
	{
		ConfigureGold ();

		//register currency balance change event
		DBManager.updatedDataEvent += ConfigureGold;
	}
	
	void OnDisable()
	{

		//unregister currency balance change event
		DBManager.updatedDataEvent -= ConfigureGold;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void ConfigureGold()
	{
		if(gameObject.activeInHierarchy)
		{
			goldLabel.text = DBManager.GetFunds ("Cat coins").ToString();
		}

	}
}
