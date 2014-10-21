using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// UI balance.
/// 
/// This class is for currency balance control
/// </summary>
public class UIBalance : MonoBehaviour 
{
	void Awake()
	{

	}

	void OnEnable()
	{
		//register event for currency balance changed
		StoreEvents.OnCurrencyBalanceChanged += CurrencyBalanceChange;
	}

	void OnDisable()
	{
		//unregister event for currency balance changed
		StoreEvents.OnCurrencyBalanceChanged -= CurrencyBalanceChange;
	}

	// Use this for initialization
	void Start () 
	{
		//set label to currency balance
		GetComponent<UILabel> ().text = StoreInventory.GetItemBalance (StoreAssets.CAT_COIN_CURRENCY_ITEM_ID).ToString ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void CurrencyBalanceChange(VirtualCurrency virtualCurrency, int balance, int amountAdded)
	{
		//set label to currency balance
		GetComponent<UILabel> ().text = StoreInventory.GetItemBalance (StoreAssets.CAT_COIN_CURRENCY_ITEM_ID).ToString ();
	}
}
