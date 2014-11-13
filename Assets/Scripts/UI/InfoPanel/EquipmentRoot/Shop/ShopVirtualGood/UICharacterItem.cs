using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;
using SIS;

/// <summary>
/// UI character item.
/// 
/// This class is for character item
/// </summary>
public class UICharacterItem : UIVirtualGood 
{
	/// <summary>
	/// Localization key
	/// This should match key in localization file
	/// </summary>
	public string selectKey = "Select";
	public string selectedKey = "Selected";
	public string buyKey = "Buy";
	public string purchasedKey = "Purchased";

	/// <summary>
	/// Reference to UI price label.
	/// </summary>
	public UILabel priceLabel;
	
	/// <summary>
	/// Reference to UI protrait sprite 
	/// </summary>
	public UISprite portrait;

	/// <summary>
	/// The protrait border.
	/// </summary>
	public UISprite protraitBorder;

	/// <summary>
	/// The button.
	/// </summary>
	public GameObject buyButton;

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
		priceLabel.text = ((int)price).ToString();
		
		//set image
		portrait.spriteName = portraitImageName;

		//check if character is equipped
		//if(StoreInventory.IsVirtualGoodEquipped(virtualGoodId))
		if(DBManager.GetAllSelected().ContainsKey(virtualGoodId))
		{
			//turn on protrait border
			protraitBorder.alpha = 1f;
		}
		else
		{
			//turn off protrait border
			protraitBorder.alpha = 0f;
		}

		ConfigureButton ();
		ConfigurePriceLabel ();

	}
	
	public override void StartPurchase()
	{
		base.StartPurchase ();

		//if character not purchased
		//if(!StoreInventory.NonConsumableItemExists(virtualGoodId))
		//if(StoreInventory.GetItemBalance(virtualGoodId) <= 0)
		if(!DBManager.isPurchased(virtualGoodId))
		{
			//show purchase control window
			uiStoreRoot.purchaseControl.ShowPurchaseWindow (virtualGoodId, virtualGoodName, descriptionTag);
		}

		
	}

	protected override void PurchaseWindowItemPurchased(UIPurchaseControl control, string itemId)
	{
		if(itemId != virtualGoodId)
		{
			return;
		}

		base.PurchaseWindowItemPurchased (control, itemId);
		
		
		ConfigureButton ();
		ConfigurePriceLabel ();

	}

	void ConfigureButton()
	{

		if(gameObject.activeInHierarchy)
		{
			//if character has been bought
			//if(StoreInventory.GetItemBalance(virtualGoodId) > 0)
			if(DBManager.isPurchased(virtualGoodId))
			{

				if(DBManager.GetAllSelected().Count <= 0)
				{
					Debug.LogError(gameObject.name+" No character was selected");
					return;
				}
				
				//if character is selected
				//if(StoreInventory.IsVirtualGoodEquipped(virtualGoodId))
				if((DBManager.GetAllSelected().ContainsKey(IAPManager.GetIAPObjectGroupName(virtualGoodId))) && 
				   (DBManager.GetAllSelected()[IAPManager.GetIAPObjectGroupName(virtualGoodId)].Contains(virtualGoodId)))
				{
					//disable button
					buyButton.GetComponent<UIButton>().isEnabled = false;
					
					//change button function name to non
					//NGUI 2.7
					//UIButtonMessage btnMsg = buyButton.GetComponent<UIButtonMessage>();
					//btnMsg.functionName = "";

					//NGUI 3.x.x
					UIButton btn = buyButton.GetComponent<UIButton>();
					EventDelegate.Remove(btn.onClick, Select);
					EventDelegate.Remove(btn.onClick, StartPurchase);
					
					//change localize key to selected
					buyButton.GetComponentInChildren<UILocalize>().key = selectedKey;
					
					//change label to selected
					buyButton.GetComponentInChildren<UILabel>().text = Localization.Get(selectedKey);
				}
				else
				{
					//enable button
					buyButton.GetComponent<UIButton>().isEnabled = true;
					
					//change button function name to Select
					//NGUI 2.7
					//UIButtonMessage btnMsg = buyButton.GetComponent<UIButtonMessage>();
					//btnMsg.functionName = "Select";

					//NGUI 3.x.x
					UIButton btn = buyButton.GetComponent<UIButton>();
					EventDelegate.Set(btn.onClick, Select);
					
					//change localize key to select
					buyButton.GetComponentInChildren<UILocalize>().key = selectKey;
					
					//change label to select
					buyButton.GetComponentInChildren<UILabel>().text = Localization.Get(selectKey);
				}
				
				
			}
			else
			{
				//change button function name to StartPurchase
				//NGUI 2.7
				//UIButtonMessage btnMsg = buyButton.GetComponent<UIButtonMessage>();
				//btnMsg.functionName = "StartPurchase";

				//NGUI 3.x.x
				UIButton btn = buyButton.GetComponent<UIButton>();
				EventDelegate.Set (btn.onClick, StartPurchase);
				
				//change localize key to select
				buyButton.GetComponentInChildren<UILocalize>().key = buyKey;
				
				//change label to select
				buyButton.GetComponentInChildren<UILabel>().text = Localization.Get(buyKey);
			}
		}

	}

	void ConfigurePriceLabel()
	{
		if(gameObject.activeInHierarchy)
		{
			//if character has been bought...change price label to purchased
			//if(StoreInventory.GetItemBalance(virtualGoodId) > 0)
			if(DBManager.isPurchased(virtualGoodId))
			{
				priceLabel.text = Localization.Get(purchasedKey);
			}
			else
			{
				//set price
				priceLabel.text = ((int)price).ToString();
			}
		}

	}

	/// <summary>
	/// Select this character
	/// </summary>
	public void Select()
	{
		List<UICharacterItem> items = GetItems<UICharacterItem> ();

		//select character
		uiStoreRoot.SelectCharacter (virtualGoodId);


		//turn on protrait border
		protraitBorder.alpha = 1f;

		ConfigureButton ();

		//Deselect all characters
		for(int i=0; i<items.Count; i++)
		{
			if(items[i].virtualGoodId != virtualGoodId)
			{
				items[i].Deselect();
			}
			
		}
	}

	/// <summary>
	/// Deselect this character.
	/// </summary>
	public void Deselect()
	{

		//deselect character
		uiStoreRoot.DeselectCharacter (virtualGoodId);

		protraitBorder.alpha = 0f;

		ConfigureButton ();
	}
}
