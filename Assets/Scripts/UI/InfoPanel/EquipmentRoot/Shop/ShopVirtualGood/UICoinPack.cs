using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// UI coin pack.
/// 
/// This class is for coin pack item
/// </summary>
public class UICoinPack : UIVirtualGood 
{
	/// <summary>
	/// How many coin to give to player if purchase.
	/// </summary>
	public int coinGave = 0;

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
		priceLabel.text = "USD " + price;
		
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

	}
}
