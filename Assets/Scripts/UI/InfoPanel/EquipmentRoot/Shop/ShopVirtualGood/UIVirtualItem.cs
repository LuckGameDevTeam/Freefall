using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// UI virtual item.
/// 
/// This class is for item
/// </summary>
public class UIVirtualItem : UIVirtualGood 
{
	public string buyKey = "Buy";
	public string maxReachKey = "MaxReach";

	/// <summary>
	/// Reference to UI price label.
	/// </summary>
	public UILabel priceLabel;

	/// <summary>
	/// Reference to UI protrait sprite 
	/// </summary>
	public UISprite portrait;

	/// <summary>
	/// The button.
	/// </summary>
	public GameObject buyButton;

	/// <summary>
	/// The max number player can get.
	/// </summary>
	public int maxItems = 9;

	protected override void Awake()
	{
		base.Awake ();
	}
	
	protected override void Start()
	{
		base.Start ();
	}

	protected override void InitVirtualGood()
	{
		base.InitVirtualGood ();

		//set price label
		priceLabel.text =  ((int)price).ToString();

		//set image
		portrait.spriteName = portraitImageName;

		ConfigureButton ();
	}

	void ConfigureButton()
	{
		if(gameObject.activeInHierarchy)
		{
			if(StoreInventory.GetItemBalance(virtualGoodId) >= maxItems)
			{
				buyButton.GetComponentInChildren<UILabel>().text = Localization.Localize(maxReachKey);
				buyButton.GetComponentInChildren<UILocalize>().key = maxReachKey;
				
				buyButton.GetComponent<UIImageButton>().isEnabled = false;
			}
			else
			{
				buyButton.GetComponentInChildren<UILabel>().text = Localization.Localize(buyKey);
				buyButton.GetComponentInChildren<UILocalize>().key = buyKey;
				
				buyButton.GetComponent<UIImageButton>().isEnabled = true;
			}
		}

	}

	public override void StartPurchase()
	{
		base.StartPurchase ();

		//show purchase control window
		uiStoreRoot.purchaseControl.ShowPurchaseWindow (this);

	}

	protected override void PurchaseWindowItemPurchased(UIPurchaseControl control, string itemId)
	{
		if(itemId != virtualGoodId)
		{
			return;
		}



		base.PurchaseWindowItemPurchased (control, itemId);

		//perform extra action
		PurchasedAction (control, itemId);
			
		ConfigureButton ();
			
		Debug.Log (itemId + " " + StoreInventory.GetItemBalance (itemId));


	}

	/// <summary>
	/// Purchased the action.
	/// 
	/// subclass can override this and do any action after purchase success
	/// </summary>
	/// <param name="control">Control.</param>
	/// <param name="itemId">Item identifier.</param>
	protected virtual void PurchasedAction(UIPurchaseControl control, string itemId)
	{

	}
}
