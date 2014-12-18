using UnityEngine;
using System.Collections;
using SIS;

public class UIUpdatePlayerLife : MonoBehaviour 
{
	public UILabel lifeLabel;

	void Awake()
	{
		//register update event 
		DBManager.updatedDataEvent += ConfigurePlayerLife;
	}

	void OnDisable()
	{
		//unregister update event 
		DBManager.updatedDataEvent -= ConfigurePlayerLife;
	}

	// Use this for initialization
	void Start () 
	{
		ConfigurePlayerLife ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void ConfigurePlayerLife()
	{
#if TestMode
		DebugEx.DebugError("TestMode do not configure player life");
		return;
#endif
		if(gameObject.activeInHierarchy)
		{
			int lifeCount = DBManager.GetPlayerData (LifeCounter.PlayerLife).AsInt;
			
			//deal with digital display, should always display double digitals
			if(lifeCount > 9)
			{
				lifeLabel.text = lifeCount.ToString();
			}
			else if(lifeCount < 10 && lifeCount > 0)
			{
				lifeLabel.text = "0"+lifeCount.ToString();
			}
			else
			{
				lifeLabel.text = "00";
			}
		}

	}
}
