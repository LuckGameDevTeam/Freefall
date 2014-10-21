using UnityEngine;
using System.Collections;
using Soomla.Store;

public class UIItemBox : UIVirtualGood 
{
	/// <summary>
	/// The items that will give to player
	/// if purchase.
	/// </summary>
	public string[] gaveItemsId;

	/// <summary>
	/// How many quantity give to player per item
	/// </summary>
	public int gaveQuantity = 1;

	/// <summary>
	/// Reference to UI price label.
	/// </summary>
	public UILabel priceLabel;

	/// <summary>
	/// Reference to UI protrait sprite 
	/// </summary>
	public UISprite portrait;

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
		priceLabel.text = "C " + price;
		
		//set image
		portrait.spriteName = portraitImageName;
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

		//gave player bunch of items
		if(gaveItemsId.Length > 0)
		{
			for(int i=0; i<gaveItemsId.Length; i++)
			{
				StoreInventory.GiveItem(gaveItemsId[i], gaveQuantity);
			}
		}

		//take 1 item box away from player
		if(StoreInventory.GetItemBalance(virtualGoodId) > 0)
		{
			StoreInventory.TakeItem(virtualGoodId, 1);
		}
	}
}
