using UnityEngine;
using System.Collections;
using Soomla.Store;
using SIS;

/// <summary>
/// UI coin pack.
/// 
/// This class is for coin pack item
/// </summary>
public class UICoinPack : UIVirtualGood 
{
	/// <summary>
	/// How many coin to give to player if purchase.
	/// No longer used in new IAP system
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
		//priceLabel.text = "USD " + price;
		priceLabel.text = "USD " + IAPManager.GetIAPObject(virtualGoodId).realPrice;
		
		//set image
		portrait.spriteName = portraitImageName;
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

		//retrieve product info
		IAPObject iapObj = IAPManager.GetIAPObject (virtualGoodId);

		//increase funds for player
		//look into SIS setting, in description of coin pack has explicit amount of coin that will give to player if
		//player bought this product.
		//We here slipt description and retrieve the amount and turn it into integer
		DBManager.IncreaseFunds (IAPManager.GetCurrency () [0].name, int.Parse (iapObj.description.Split ("," [0]) [1]));

		base.PurchaseWindowItemPurchased (control, itemId);

	}
}
