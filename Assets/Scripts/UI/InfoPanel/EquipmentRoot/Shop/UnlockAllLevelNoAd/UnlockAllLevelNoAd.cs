using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// Unlock all level no ad.
/// </summary>
public class UnlockAllLevelNoAd : MonoBehaviour 
{
	/// <summary>
	/// The item identifier.
	/// </summary>
	public string buyItemId;

	/// <summary>
	/// The item title key.
	/// </summary>
	public string itemTitleKey = "BuyFullGame";

	/// <summary>
	/// The item desc key.
	/// </summary>
	public string itemDescKey = "BuyFullGameDesc";

	/// <summary>
	/// The button.
	/// </summary>
	public UIImageButton button;

	/// <summary>
	/// Reference to UIStoreControl
	/// </summary>
	private UIStoreRoot storeRoot;

	void Awake()
	{
		//find store control
		storeRoot = NGUITools.FindInParents<UIStoreRoot> (gameObject);

		//register event
		storeRoot.purchaseControl.Evt_ItemPurchased += PurchaseWindowItemPurchased;
	}

	// Use this for initialization
	void Start () 
	{
		ConfigureButton ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Purchase.
	/// </summary>
	public void Purchase()
	{
		if(!StoreInventory.NonConsumableItemExists(buyItemId))
		{
			storeRoot.purchaseControl.ShowPurchaseWindow (buyItemId, itemTitleKey, itemDescKey);
		}
	}

	/// <summary>
	/// Configures the button.
	/// </summary>
	void ConfigureButton()
	{
		if(StoreInventory.NonConsumableItemExists(buyItemId))
		{
			//not enable button if it is bought
			button.isEnabled = false;
		}
		else
		{
			//enable button if it is not bought
			button.isEnabled = true;
		}
	}

	/// <summary>
	/// Handle item purchased event.
	/// </summary>
	/// <param name="control">Control.</param>
	/// <param name="itemId">Item identifier.</param>
	void PurchaseWindowItemPurchased(UIPurchaseControl control, string itemId)
	{
		if(itemId != buyItemId)
		{
			return;
		}

		ConfigureButton ();
	}
}
