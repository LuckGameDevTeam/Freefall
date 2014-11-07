using UnityEngine;
using System.Collections;
using Soomla.Store;

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

	void Awake()
	{
		if(alertControl == null)
		{
			Debug.LogError("Purchase Window require alert control to be assigned");
		}
	}

	void OnEnable()
	{
		StoreEvents.OnMarketPurchase += onMarketPurchase;
		StoreEvents.OnItemPurchased += onItemPurchased;
		StoreEvents.OnMarketPurchaseStarted += onMarketPurchaseStarted;
		StoreEvents.OnItemPurchaseStarted += onItemPurchaseStarted;
		StoreEvents.OnUnexpectedErrorInStore += onUnexpectedErrorInStore;
		StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;

	}

	void OnDisable()
	{
		StoreEvents.OnMarketPurchase -= onMarketPurchase;
		StoreEvents.OnItemPurchased -= onItemPurchased;
		StoreEvents.OnMarketPurchaseStarted -= onMarketPurchaseStarted;
		StoreEvents.OnItemPurchaseStarted -= onItemPurchaseStarted;
		StoreEvents.OnUnexpectedErrorInStore -= onUnexpectedErrorInStore;
		StoreEvents.OnMarketPurchaseCancelled -= onMarketPurchaseCancelled;
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

		itemDescLocalize.key = Localization.Localize(good.descriptionTag);

		gameObject.SetActive (true);
	}

	/// <summary>
	/// Shows the purchase window.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="itemTitleKey">Item title key.</param>
	/// <param name="itemDescKey">Item desc key.</param>
	public void ShowPurchaseWindow(string itemId, string itemTitleKey, string itemDescKey)
	{
		currentNonGoodItemId = itemId;
		currentGood = null;

		//title
		itemTitleLocalize.key = itemTitleKey;
		
		itemDescLocalize.key = Localization.Localize(itemDescKey);
		
		gameObject.SetActive (true);
	}

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
		LockButton ();

		try 
		{
			if(currentGood != null)
			{
				StoreInventory.BuyItem (currentGood.virtualGoodId);
			}
			else
			{
				StoreInventory.BuyItem (currentNonGoodItemId);
			}

		}
		catch(InsufficientFundsException e)
		{
			//show alert window
			alertControl.ShowAlertWindow(null, noFundsKey);

			if(Evt_InsufficientFunds != null)
			{
				if(currentGood != null)
				{
					Evt_InsufficientFunds(this, currentGood.virtualGoodId);
				}
				else
				{
					Evt_InsufficientFunds(this, currentNonGoodItemId);
				}

			}

			UnlockButton ();
		}
		catch(UnityException e)
		{
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
}
