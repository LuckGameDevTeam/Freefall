using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// UI equipped item display.
/// 
/// This class represent each item that is equipped in UI
/// </summary>
public class UIEquippedItemDisplay : MonoBehaviour 
{
	/// <summary>
	/// The quantity label.
	/// </summary>
	public UILabel quantityLabel;

	/// <summary>
	/// The button.
	/// </summary>
	public GameObject button;

	/// <summary>
	/// The item identifier.
	/// 
	/// Must be assigned when create one
	/// </summary>
	private string itemId;

	void Awake()
	{
		//register event for item balance changed
		StoreEvents.OnGoodBalanceChanged += OnItemBalanceChange;
	}

	void OnDisable()
	{
		StoreEvents.OnGoodBalanceChanged -= OnItemBalanceChange;
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Unequip.
	/// </summary>
	public void UnEquip()
	{
		//find UIEquippedItemControl
		UIEquippedItemControl control = NGUITools.FindInParents<UIEquippedItemControl> (gameObject);

		//unequip item
		control.UnequipItem (itemId, gameObject);
	}

	/// <summary>
	/// Handle the item balance change event.
	/// </summary>
	/// <param name="good">Good.</param>
	/// <param name="balance">Balance.</param>
	/// <param name="amountAdded">Amount added.</param>
	void OnItemBalanceChange(VirtualGood good, int balance, int amountAdded)
	{
		if(good.ItemId == itemId)
		{
			//change quantity label
			quantityLabel.text = balance.ToString();
		}
	}

	public string ItemId
	{
		get
		{
			return itemId;
		}

		set
		{
			itemId = value;

			Debug.Log ("itemId assigned");

			if(gameObject.activeInHierarchy)
			{
				Debug.Log("Configure button");

				string iconName = itemId+"_Icon";

				//change button sprtie name
				button.GetComponentInChildren<UISprite>().spriteName = iconName;

				//change image of button for each state
				UIButton btn = button.GetComponent<UIButton>();
				btn.normalSprite = iconName;
				btn.hoverSprite = iconName;
				btn.pressedSprite = iconName;
				btn.disabledSprite = iconName;

				//set button enable
				btn.isEnabled = true;

				//set quantity label to item balance
				quantityLabel.text = StoreInventory.GetItemBalance(itemId).ToString();
			}

		}
	}
}
