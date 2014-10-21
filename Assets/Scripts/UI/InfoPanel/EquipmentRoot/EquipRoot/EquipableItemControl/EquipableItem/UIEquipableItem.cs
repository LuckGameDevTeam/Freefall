using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// UI equipable item.
/// 
/// This class represent equipable item in equip UI page
/// </summary>
public class UIEquipableItem : MonoBehaviour 
{
	public string itemEquipableKey = "ItemEquipable";
	public string itemEquippedKey = "ItemEquipped";
	public string buyKey = "Buy";
	public string purchasedKey = "Purchased";

	/// <summary>
	/// The name of the virtual good.
	/// </summary>
	public string equipableItemNameKey;
	
	/// <summary>
	/// The virtual good identifier.
	/// 
	/// This is as same as id in soomla
	/// </summary>
	public string equipableItemId;

	/// <summary>
	/// The equipable item desc key.
	/// </summary>
	public string equipableItemDescKey;

	/// <summary>
	/// The item price.
	/// </summary>
	public int itemPrice = 0;

	/// <summary>
	/// The price label.
	/// </summary>
	public UILabel priceLabel;

	/// <summary>
	/// The coin mark.
	/// </summary>
	public GameObject CoinMark;

	/// <summary>
	/// The button buy.
	/// </summary>
	public GameObject buyButton;

	/// <summary>
	/// The quantity label.
	/// </summary>
	public UILabel quantityLabel;

	/// <summary>
	/// Reference to UIEquipableItemControl
	/// </summary>
	private UIEquipableItemControl equipControl;

	/// <summary>
	/// Reference to UIEquipRoot.
	/// </summary>
	private UIEquipRoot equipRoot;

	protected virtual void Awake()
	{
		//find UIEquipRoot
		equipRoot = NGUITools.FindInParents<UIEquipRoot> (gameObject);

		//find UIEquipableItemControl 
		equipControl = NGUITools.FindInParents<UIEquipableItemControl> (gameObject);

		//register event
		equipControl.Evt_OnItemUnEquip += OnItemUnEquip;

		//register event for item purchased event from puchase window
		equipRoot.purchaseControl.Evt_ItemPurchased += OnItemPurchased;

		//register event for item balance change
		StoreEvents.OnGoodBalanceChanged += OnItemBalanceChange;
	}

	// Use this for initialization
	protected virtual void Start () 
	{
		ConfigureEquipableItem();
	}

	protected virtual void OnEnable()
	{
		ConfigureEquipableItem();
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
	
	}

	/// <summary>
	/// Configures the equipable item.
	/// </summary>
	protected virtual void ConfigureEquipableItem()
	{
		if(gameObject.activeInHierarchy)
		{
			//get item balance
			int itemBalance = StoreInventory.GetItemBalance (equipableItemId);

			//item balance > 0
			if(itemBalance > 0)
			{
				//chnage price label to purchased
				priceLabel.text = Localization.Localize(purchasedKey);

				//don't show coin mark
				CoinMark.SetActive(false);

				//set quantity label to item balance
				quantityLabel.text = itemBalance.ToString();
				
				//check if item equiped or not
				if(equipControl.IsItemEquiped(equipableItemId))
				{
					//set buy button fucntion name to none
					buyButton.GetComponent<UIButtonMessage>().functionName = "";

					//change buy button localized key
					buyButton.GetComponentInChildren<UILocalize>().key = itemEquippedKey;

					//change buy button label
					buyButton.GetComponentInChildren<UILabel>().text = Localization.Localize(itemEquippedKey);

					//set buy button to not enable 
					buyButton.GetComponent<UIImageButton>().isEnabled = false;
				}
				else
				{
					//set buy button function name to EquipItem
					buyButton.GetComponent<UIButtonMessage>().functionName = "EquipItem";

					//change buy button localized key
					buyButton.GetComponentInChildren<UILocalize>().key = itemEquipableKey;

					//change buy button label
					buyButton.GetComponentInChildren<UILabel>().text = Localization.Localize(itemEquipableKey);

					//set buy button to enable
					buyButton.GetComponent<UIImageButton>().isEnabled = true;
				}
				
			}
			else//item balance <= 0
			{
				//set price label to item price
				priceLabel.text = itemPrice.ToString();

				//set coin mark active
				CoinMark.SetActive(true);

				//set quantity label to item balance
				quantityLabel.text = itemBalance.ToString();

				//set buy button function name to Purchase
				buyButton.GetComponent<UIButtonMessage>().functionName = "Purchase";

				//change buy button localize key
				buyButton.GetComponentInChildren<UILocalize>().key = buyKey;

				//change buy button label
				buyButton.GetComponentInChildren<UILabel>().text = Localization.Localize(buyKey);

				//set buy button to enable
				buyButton.GetComponent<UIImageButton>().isEnabled = true;
			}
		}
	}

	/// <summary>
	/// Equip item.
	/// </summary>
	public virtual void EquipItem()
	{
		//tell equipControl this item should be equipped
		if(equipControl.EquipItem(equipableItemId))
		{
			ConfigureEquipableItem();
		}
	}

	/// <summary>
	/// Purchase this item.
	/// </summary>
	public virtual void Purchase()
	{
		//check if this item's balance if it is 0
		if(StoreInventory.GetItemBalance(equipableItemId) == 0)
		{
			//show purchase window
			equipRoot.purchaseControl.ShowPurchaseWindow(equipableItemId, equipableItemNameKey, equipableItemDescKey);
		}
	}

	/// <summary>
	/// Handle the item unequip event.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	protected virtual void OnItemUnEquip(string itemId)
	{
		if(itemId != equipableItemId)
		{
			return;
		}

		if(gameObject.activeInHierarchy)
		{
			//change buy button function name to EquipItem
			buyButton.GetComponent<UIButtonMessage>().functionName = "EquipItem";

			//change buy button localize key
			buyButton.GetComponentInChildren<UILocalize>().key = itemEquipableKey;

			//change buy button label
			buyButton.GetComponentInChildren<UILabel>().text = Localization.Localize(itemEquipableKey);

			//change buy button to enable
			buyButton.GetComponent<UIImageButton>().isEnabled = true;
		}

	}

	/// <summary>
	/// Handle the item purchased event.
	/// </summary>
	/// <param name="control">Control.</param>
	/// <param name="itemId">Item identifier.</param>
	protected virtual void OnItemPurchased(UIPurchaseControl control, string itemId)
	{
		if(itemId != equipableItemId)
		{
			return;
		}

		ConfigureEquipableItem ();

	}

	/// <summary>
	/// Handle the item balance change event.
	/// </summary>
	/// <param name="good">Good.</param>
	/// <param name="balance">Balance.</param>
	/// <param name="amountAdded">Amount added.</param>
	void OnItemBalanceChange(VirtualGood good, int balance, int amountAdded)
	{
		if(good.ItemId == equipableItemId)
		{
			//change quantity label to item balance
			quantityLabel.text = balance.ToString();
		}
	}

}
