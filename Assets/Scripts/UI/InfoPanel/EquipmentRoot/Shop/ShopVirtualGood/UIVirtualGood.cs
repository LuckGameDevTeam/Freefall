using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PurchaseType
{
	VirtualCoin,//in game currency
	Market //real money
}

/// <summary>
/// Ui virtual good.
/// 
/// This is generic class for all kind of good, such character,
/// item and coin pack.
/// </summary>
public class UIVirtualGood : MonoBehaviour 
{

	/// <summary>
	/// The name of the virtual good.
	/// </summary>
	public string virtualGoodName;

	/// <summary>
	/// The virtual good identifier.
	/// 
	/// This is as same as id in soomla
	/// </summary>
	public string virtualGoodId;

	/// <summary>
	/// The description tag.
	/// 
	/// Tag in localization
	/// </summary>
	public string descriptionTag;

	/// <summary>
	/// The type of the purchase.
	/// 
	/// </summary>
	public PurchaseType purchaseType;

	/// <summary>
	/// How much to spend to purchase
	/// </summary>
	public float price = 0;

	/// <summary>
	/// The name of the portrait image.
	/// 
	/// Icon
	/// </summary>
	public string portraitImageName;

	/// <summary>
	/// Reference to UIStoreControl
	/// </summary>
	protected UIStoreRoot uiStoreRoot;

	protected virtual void Awake()
	{

		//find UIStoreControl in parents
		uiStoreRoot = NGUITools.FindInParents<UIStoreRoot> (gameObject);

		//register event for purchase window
		uiStoreRoot.purchaseControl.Evt_Close += PurchaseWindowClose;
		uiStoreRoot.purchaseControl.Evt_ErrorOccur += PurchaseWindowErrorOccur;
		uiStoreRoot.purchaseControl.Evt_InsufficientFunds += PurchaseWindowInsufficientFunds;
		uiStoreRoot.purchaseControl.Evt_ItemPurchaseCancelled += PurchaseWindowCancel;
		uiStoreRoot.purchaseControl.Evt_ItemPurchased += PurchaseWindowItemPurchased;
		uiStoreRoot.purchaseControl.Evt_ItemPurchaseStarted += PurchaseWindowStartPurchase;

	}

	protected virtual void Start()
	{
		InitVirtualGood ();
	}

	/// <summary>
	/// Init virtual good.
	/// </summary>
	protected virtual void InitVirtualGood()
	{

	}

	/// <summary>
	/// Starts the purchase.
	/// 
	/// Pop up purchase window
	/// 
	/// Call this when buy button press
	/// </summary>
	public virtual void StartPurchase()
	{

	}

	/// <summary>
	/// Event handler for purchase window close
	/// </summary>
	protected virtual void PurchaseWindowClose()
	{

	}

	/// <summary>
	/// Event handler for purchase window occur an error
	/// </summary>
	/// <param name="control">Control.</param>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="errorMessage">Error message.</param>
	protected virtual void PurchaseWindowErrorOccur(UIPurchaseControl control, string itemId, string errorMessage)
	{
	}

	/// <summary>
	/// Event handler for purchase window find out no insufficient funds
	/// </summary>
	/// <param name="control">Control.</param>
	/// <param name="itemId">Item identifier.</param>
	protected virtual void PurchaseWindowInsufficientFunds(UIPurchaseControl control, string itemId)
	{
	}

	/// <summary>
	/// Event handler for purchase window cancel pruchase process
	/// </summary>
	/// <param name="control">Control.</param>
	/// <param name="itemId">Item identifier.</param>
	protected virtual void PurchaseWindowCancel(UIPurchaseControl control, string itemId)
	{
	}

	/// <summary>
	/// Event handler for purchase window purchase an item
	/// </summary>
	/// <param name="control">Control.</param>
	/// <param name="itemId">Item identifier.</param>
	protected virtual void PurchaseWindowItemPurchased(UIPurchaseControl control, string itemId)
	{
	}

	/// <summary>
	/// Event handler for purchase window start an item purchase process
	/// </summary>
	/// <param name="control">Control.</param>
	/// <param name="itemId">Item identifier.</param>
	protected virtual void PurchaseWindowStartPurchase(UIPurchaseControl control, string itemId)
	{
	}

	/// <summary>
	/// Helper fucntion
	/// Get all same level items
	/// Those items and you must be in a child of UIGrid
	/// </summary>
	/// <returns>The items with type or null it there is an error.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	protected List<T> GetItems<T>() where T : UnityEngine.Component
	{
		//Make sure we are item in UIGrid
		if(transform.parent.GetComponent<UIGrid>() != null)
		{
			List<T> retItems = new List<T>();

			for(int i=0; i<transform.parent.childCount; i++)
			{
				Transform child = transform.parent.GetChild(i);

				T item = child.GetComponent<T>();

				if(item != null)
				{
					retItems.Add(item);
				}
			}

			return retItems;
		}

		Debug.LogError("You request same level of item but you are not under gameobject with UIGride");

		return null;
	}
}
