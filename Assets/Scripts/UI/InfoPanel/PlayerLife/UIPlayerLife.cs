using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// UI player life.
/// 
/// This class handle player life display
/// </summary>
public class UIPlayerLife : MonoBehaviour 
{

	public UILabel lifeLabel;

	void Awake()
	{
		//register item balance change event
		StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
	}

	// Use this for initialization
	void Start () 
	{
		ConfigurePlayerLife ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void ConfigurePlayerLife()
	{
		//get back life count
		int lifeCount = StoreInventory.GetItemBalance (StoreAssets.PLAYER_LIFE_ITEM_ID);

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

	public void onGoodBalanceChanged(VirtualGood good, int balance, int amountAdded)
	{
		if(good.ItemId == StoreAssets.PLAYER_LIFE_ITEM_ID)
		{
			ConfigurePlayerLife();
		}
	}

}
