using UnityEngine;
using System.Collections;
using SIS;

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
		//priceLabel.text =  ((int)price).ToString();
		priceLabel.text = IAPManager.GetIAPObject (virtualGoodId).virtualPrice [0].amount.ToString ();

		//set image
		portrait.spriteName = portraitImageName;

		ConfigureButton ();
	}

	void ConfigureButton()
	{
		if(gameObject.activeInHierarchy)
		{
			//if(StoreInventory.GetItemBalance(virtualGoodId) >= maxItems)
			if(DBManager.GetPlayerData(virtualGoodId).AsInt >= maxItems)
			{
				buyButton.GetComponentInChildren<UILabel>().text = Localization.Get(maxReachKey);
				buyButton.GetComponentInChildren<UILocalize>().key = maxReachKey;
				
				buyButton.GetComponent<UIButton>().isEnabled = false;
			}
			else
			{
				buyButton.GetComponentInChildren<UILabel>().text = Localization.Get(buyKey);
				buyButton.GetComponentInChildren<UILocalize>().key = buyKey;
				
				buyButton.GetComponent<UIButton>().isEnabled = true;
			}
		}

	}

	public override void StartPurchase()
	{
		base.StartPurchase ();

		//show purchase control window
		//uiStoreRoot.purchaseControl.ShowPurchaseWindow (this);
		uiStoreRoot.purchaseControl.ShowPurchaseWindow(virtualGoodId, virtualGoodName, descriptionTag);
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
			
		//Debug.Log (itemId + " " + StoreInventory.GetItemBalance (itemId));
		Debug.Log (itemId + " " + DBManager.GetPlayerData(itemId).AsInt);
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
