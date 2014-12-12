using UnityEngine;
using System.Collections;
using SIS;

/// <summary>
/// UI purchase control.
/// 
/// This class handle purchase control
/// 
/// All purchase process go through this class
/// </summary>
public class UIPurchaseControl : MonoBehaviour 
{

	public string noFundsKey = "NotEnoughFunds";
	public string errorKey = "BuyError";
	public string syncDataFailKey = "SyncDataFail";
	public string noInternetKey = "NoInternet";

	public delegate void EventItemPurchaseStarted(UIPurchaseControl control, string itemId);
	/// <summary>
	/// Event when item purchase started
	/// </summary>
	public EventItemPurchaseStarted Evt_ItemPurchaseStarted;


	public delegate void EventItemPurchased(UIPurchaseControl control, string itemId);
	/// <summary>
	/// Event for an item purchased
	/// </summary>
	public EventItemPurchased Evt_ItemPurchased;


	public delegate void EventItemPurchaseCancelled(UIPurchaseControl control, string itemId);
	/// <summary>
	/// Event when item purchase cancel
	/// only for item on market
	/// </summary>
	public EventItemPurchaseCancelled Evt_ItemPurchaseCancelled;


	public delegate void EventErrorOccur(UIPurchaseControl control, string itemId, string errorMessage);
	/// <summary>
	/// Event when there is an error in store
	/// </summary>
	public EventErrorOccur Evt_ErrorOccur;


	public delegate void EventInsufficientFunds(UIPurchaseControl control, string itemId);
	/// <summary>
	/// Event if insufficient fund to buy a virtual item
	/// </summary>
	public EventInsufficientFunds Evt_InsufficientFunds;



	public delegate void EventClose();
	/// <summary>
	/// Event when purchase window close
	/// </summary>
	public EventClose Evt_Close;



	/// <summary>
	/// The item title localize.
	/// </summary>
	public UILocalize itemTitleLocalize;
	
	/// <summary>
	/// The item desc localize.
	/// </summary>
	public UILocalize itemDescLocalize;

	public UILabel itemTitle;

	public UILabel itemDesc;

	/// <summary>
	/// The buy button.
	/// </summary>
	public UIButton buyButton;

	/// <summary>
	/// The close button.
	/// </summary>
	public UIButton closeButton;

	/// <summary>
	/// The alert control.
	/// </summary>
	public UIAlertControl alertControl;

	/// <summary>
	/// The current good.
	/// </summary>
	private UIVirtualGood currentGood;

	/// <summary>
	/// The current non good item identifier.
	/// 
	/// If show purchase window but 
	/// </summary>
	private string currentNonGoodItemId;

	private SISDataSync sisDs;

	void Awake()
	{
		if(alertControl == null)
		{
			Debug.LogError("Purchase Window require alert control to be assigned");
		}

		sisDs = GetComponent<SISDataSync> ();

		sisDs.Evt_OnUploadDataComplete += OnUploadDataSuccess;
		sisDs.Evt_OnUploadDataFail += OnUploadDataFail;
		sisDs.Evt_OnAccountLoginFromOtherDevice += OnUploadDataFail;
	}

	void OnEnable()
	{
		/*
		StoreEvents.OnMarketPurchase += onMarketPurchase;
		StoreEvents.OnItemPurchased += onItemPurchased;
		StoreEvents.OnMarketPurchaseStarted += onMarketPurchaseStarted;
		StoreEvents.OnItemPurchaseStarted += onItemPurchaseStarted;
		StoreEvents.OnUnexpectedErrorInStore += onUnexpectedErrorInStore;
		StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;
		*/

		IAPManager.purchaseSucceededEvent += OnPurchaseSucceed;
		IAPManager.purchaseFailedEvent += OnPurchaseFail;
	}

	void OnDisable()
	{
		/*
		StoreEvents.OnMarketPurchase -= onMarketPurchase;
		StoreEvents.OnItemPurchased -= onItemPurchased;
		StoreEvents.OnMarketPurchaseStarted -= onMarketPurchaseStarted;
		StoreEvents.OnItemPurchaseStarted -= onItemPurchaseStarted;
		StoreEvents.OnUnexpectedErrorInStore -= onUnexpectedErrorInStore;
		StoreEvents.OnMarketPurchaseCancelled -= onMarketPurchaseCancelled;
		*/

		IAPManager.purchaseSucceededEvent -= OnPurchaseSucceed;
		IAPManager.purchaseFailedEvent -= OnPurchaseFail;
	}

	/// <summary>
	/// Shows the purchase window.
	/// </summary>
	/// <param name="good">Good.</param>
	public void ShowPurchaseWindow(UIVirtualGood good)
	{
		currentGood = good;
		currentNonGoodItemId = "";

		//title
		itemTitleLocalize.key = good.virtualGoodName;

		itemDescLocalize.key = Localization.Get(good.descriptionTag);

		gameObject.SetActive (true);
	}

	/// <summary>
	/// Shows the purchase window with localized on or off.
	/// localize true then itemTitleKey, itemDescKey is the key in localize file
	/// localize false then you should do localization for itemTitleKey, itemDescKey
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="itemTitleKey">Item title key.</param>
	/// <param name="itemDescKey">Item desc key.</param>
	public void ShowPurchaseWindow(string itemId, string itemTitleKey, string itemDescKey, bool localize = true)
	{
		currentNonGoodItemId = itemId;
		currentGood = null;


		if(localize)
		{
			//title
			itemTitle.text = Localization.Get (itemTitleKey);
			
			itemDesc.text = Localization.Get (itemDescKey);
		}
		else
		{
			itemTitle.text= itemTitleKey;

			itemDesc.text = itemDescKey;
		}

		
		gameObject.SetActive (true);
	}

	/// <summary>
	/// Shows the purchase window.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="itemTitleKey">Item title key.</param>
	/// <param name="itemDescKey">Item desc key.</param>
	/*
	public void ShowPurchaseWindow(string itemId, string itemTitleKey, string itemDescKey)
	{
		currentNonGoodItemId = itemId;
		currentGood = null;

		//title
		itemTitleLocalize.key = itemTitleKey;
		
		itemDescLocalize.key = Localization.Get(itemDescKey);
		
		gameObject.SetActive (true);
	}
	*/

	/// <summary>
	/// Closes the purchase window.
	/// </summary>
	public void ClosePurchaseWindow()
	{
		currentGood = null;
		currentNonGoodItemId = "";

		if(Evt_Close != null)
		{
			Evt_Close();
		}

		gameObject.SetActive (false);
	}

	/// <summary>
	/// Purchases the item.
	/// </summary>
	public void PurchaseItem()
	{
		if(Application.internetReachability == NetworkReachability.NotReachable)
		{
			alertControl.ShowAlertWindow(errorKey, noInternetKey);
			return;
		}

		LockButton ();

		try 
		{
			string pId;

			if(currentGood != null)
			{
				//StoreInventory.BuyItem (currentGood.virtualGoodId);

				pId = currentGood.virtualGoodId;
			}
			else
			{
				//StoreInventory.BuyItem (currentNonGoodItemId);

				pId = currentNonGoodItemId;
			}

			//get iap object 
			IAPObject iapObj = IAPManager.GetIAPObject(pId);

			//determine which type of product and do the correspond purchase method
			switch(iapObj.type)
			{
			case IAPType.consumable:

#if UNITY_EDITOR
				//Dot allow to make market purchase in editor
				//show alert window
				alertControl.ShowAlertWindow (null, "Can't purchase market product in editor");

				UnlockButton();
				ClosePurchaseWindow();

#else
				IAPManager.PurchaseConsumableProduct(pId);
#endif
				break;

			case IAPType.nonConsumable:

#if UNITY_EDITOR 
				//Dot allow to make market purchase in editor
				//show alert window
				alertControl.ShowAlertWindow (null, "Can't purchase market product in editor");

				UnlockButton();
				ClosePurchaseWindow();
#else
				IAPManager.PurchaseNonconsumableProduct(pId);
#endif
				break;

			case IAPType.consumableVirtual:

				IAPManager.PurchaseConsumableVirtualProduct(pId);
				break;

			case IAPType.nonConsumableVirtual:

				IAPManager.PurchaseNonconsumableVirtualProduct(pId);
				break;
			}

		}
		catch(UnityException e)
		{
			UnlockButton();
			Debug.LogError("Purchase product throw an exception");
		}

	}

	/// <summary>
	/// Locks the button.
	/// </summary>
	void LockButton()
	{
		buyButton.isEnabled = false;
		closeButton.isEnabled = false;
	}

	/// <summary>
	/// Unlocks the button.
	/// </summary>
	void UnlockButton()
	{
		buyButton.isEnabled = true;
		closeButton.isEnabled = true;
	}

	void FinalizePurchase()
	{
		UnlockButton ();
		
		ClosePurchaseWindow ();
	}

	#region SISDataSync callback
	void OnUploadDataSuccess()
	{
		Debug.Log("Upload client data to server");

		FinalizePurchase ();
	}
	
	void OnUploadDataFail()
	{
		alertControl.ShowAlertWindow (null, syncDataFailKey);

		FinalizePurchase ();
	}
	#endregion SISDataSync callback

	#region SIS callback
	void OnPurchaseSucceed(string pId)
	{
		//increase item amount by 1
		DBManager.IncrementPlayerData(pId, 1);

		//fire purchase event
		if(Evt_ItemPurchased != null)
		{
			Evt_ItemPurchased(this, pId);
		}

		sisDs.UploadData ();
	}

	void OnPurchaseFail(string errorMsg)
	{
		Debug.LogWarning ("Error in store: " + errorMsg);

		UnlockButton ();

		//we splite errorMsg
		//-1 transaction cancel
		string errorStr = errorMsg.Split ("," [0]) [1];

		errorStr = errorStr.Trim(". ".ToCharArray());

		Debug.LogWarning ("Error in store: " + errorStr);

		if(errorStr != "Transaction cancelled")
		{

			if(errorStr == "Insufficient funds")
			{
				//show alert window
				alertControl.ShowAlertWindow (null, noFundsKey);
				
				if(currentGood != null)
				{
					Evt_InsufficientFunds(this, currentGood.virtualGoodId);
				}
				else
				{
					Evt_InsufficientFunds(this, currentNonGoodItemId);
				}
			}
			else
			{
				//show alert window
				alertControl.ShowAlertWindow (null, errorKey);
				
				if(Evt_ErrorOccur != null)
				{
					if(currentGood != null)
					{
						Evt_ErrorOccur(this, currentGood.virtualGoodId, errorMsg);
					}
					else
					{
						Evt_ErrorOccur(this, currentNonGoodItemId, errorMsg);
					}
				}
			}

		}
		else
		{
			if(Evt_ItemPurchaseCancelled != null)
			{
				if(currentGood != null)
				{
					Evt_ErrorOccur(this, currentGood.virtualGoodId, errorMsg);
				}
				else
				{
					Evt_ErrorOccur(this, currentNonGoodItemId, errorMsg);
				}
			}

		}



	}
	#endregion SIS callback

	#region Soomla callback
	/*
	/// <summary>
	/// Handles a market purchase event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	/// <param name="purchaseToken">Purchase token.</param>
	public void onMarketPurchase(PurchasableVirtualItem pvi, string purchaseToken, string payload) 
	{
		
	}

	/// <summary>
	/// Handles an item purchase event. 
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onItemPurchased(PurchasableVirtualItem pvi, string payload) 
	{
		if(Evt_ItemPurchased != null)
		{
			Evt_ItemPurchased(this, pvi.ItemId);
		}

		UnlockButton ();

		ClosePurchaseWindow ();
	}

	/// <summary>
	/// Handles a market purchase started event. 
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onMarketPurchaseStarted(PurchasableVirtualItem pvi) 
	{
		if(Evt_ItemPurchaseStarted != null)
		{
			Evt_ItemPurchaseStarted(this,pvi.ItemId);
		}
	}
	
	/// <summary>
	/// Handles an item purchase started event. 
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onItemPurchaseStarted(PurchasableVirtualItem pvi) 
	{
		if(Evt_ItemPurchaseStarted != null)
		{
			Evt_ItemPurchaseStarted(this, pvi.ItemId);
		}
	}
	
	/// <summary>
	/// Handles an item purchase cancelled event. 
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onMarketPurchaseCancelled(PurchasableVirtualItem pvi) 
	{
		if(Evt_ItemPurchaseCancelled != null)
		{
			Evt_ItemPurchaseCancelled(this, pvi.ItemId);
		}

		UnlockButton ();
	}
	
	/// <summary>
	/// Handles an unexpected error in store event.
	/// </summary>
	/// <param name="message">Error message.</param>
	public void onUnexpectedErrorInStore(string errorMessage) 
	{
		Debug.LogWarning ("Error in store: " + errorMessage);

		//show alert window
		alertControl.ShowAlertWindow (null, errorKey);


		UnlockButton ();

		if(Evt_ErrorOccur != null)
		{
			if(currentGood != null)
			{
				Evt_ErrorOccur(this, currentGood.virtualGoodId, errorMessage);
			}
			else
			{
				Evt_ErrorOccur(this, currentNonGoodItemId, errorMessage);
			}
		}


	}
	*/
	#endregion Soomla callback
}
