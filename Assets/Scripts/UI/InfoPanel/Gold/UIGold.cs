using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// UI gold.
/// 
/// 
/// </summary>
public class UIGold : MonoBehaviour 
{
	public UILabel goldLabel;

	void OnEnable()
	{
		//register currency balance change event
		StoreEvents.OnCurrencyBalanceChanged += CurrencyBalanceChange;
	}
	
	void OnDisable()
	{
		//unregister currency balance change event
		StoreEvents.OnCurrencyBalanceChanged -= CurrencyBalanceChange;
	}

	// Use this for initialization
	void Start () 
	{
		//set gold label to currency balance
		goldLabel.text = StoreInventory.GetItemBalance (StoreAssets.CAT_COIN_CURRENCY_ITEM_ID).ToString ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void CurrencyBalanceChange(VirtualCurrency virtualCurrency, int balance, int amountAdded)
	{
		//set gold label to currency balance
		goldLabel.text = StoreInventory.GetItemBalance (StoreAssets.CAT_COIN_CURRENCY_ITEM_ID).ToString ();
	}
}
