using UnityEngine;
using System.Collections;
using Soomla.Store;
using SIS;

/// <summary>
/// UI ability button.
/// 
/// This class handle each ability button in game
/// </summary>
public class UIAbilityButton : MonoBehaviour 
{
	public delegate void EventOnButtonPress(UIAbilityButton button, string itemId);
	public EventOnButtonPress Evt_OnButtonPress;

	/// <summary>
	/// The item id this button bound to.
	/// </summary>
	private string itemId;

	/// <summary>
	/// The background sprite.
	/// </summary>
	public UISprite backgroundSprite;

	/// <summary>
	/// The button.
	/// </summary>
	public UIButton button;

	/// <summary>
	/// The quantity label.
	/// </summary>
	public UILabel quantityLabel;

	void Awake()
	{
		gameObject.SetActive (false);

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
	/// Raises the button press event.
	/// </summary>
	public void OnButtonPress()
	{
		if(Evt_OnButtonPress != null)
		{
			Evt_OnButtonPress(this, itemId);
		}
	}

	/// <summary>
	/// Locks the button so player can not interact with.
	/// </summary>
	public void LockButton()
	{
		button.isEnabled = false;
	}

	/// <summary>
	/// Uns the lock button so player can interact with.
	/// </summary>
	public void UnLockButton()
	{
		button.isEnabled = true;
	}

	/// <summary>
	/// Handle event when item balance changed
	/// </summary>
	/// <param name="good">Good.</param>
	/// <param name="balance">Balance.</param>
	/// <param name="amountAdded">Amount added.</param>
	void OnItemBalanceChange(VirtualGood good, int balance, int amountAdded)
	{
		if(good.ItemId != itemId)
		{
			return;
		}

		ConfigureButton (balance);
	}

	/// <summary>
	/// Configures the button.
	/// </summary>
	/// <param name="balance">Balance.</param>
	void ConfigureButton(int balance)
	{	

		if(gameObject.activeInHierarchy)
		{
			if(balance != 0)
			{
				//set quantity
				quantityLabel.text = balance.ToString();
				
				button.isEnabled = true;
				
				backgroundSprite.spriteName = itemId+"_Icon";;
				
				button.normalSprite = itemId+"_Icon";
				button.hoverSprite = itemId+"_Icon";
				button.pressedSprite = itemId+"_Icon_Pressed";
				button.disabledSprite = itemId+"_Icon";
			}
			else
			{
				quantityLabel.text = "0";
				
				backgroundSprite.spriteName = itemId+"_Icon";
				
				button.normalSprite = itemId+"_Icon";
				button.hoverSprite = itemId+"_Icon";
				button.pressedSprite = itemId+"_Icon_Pressed";
				button.disabledSprite = itemId+"_Icon";
				
				button.isEnabled = false;
			}
		}

	}
	
	/// <summary>
	/// Gets or sets the item identifier.
	/// 
	/// give null to make button not appear
	/// </summary>
	/// <value>The item identifier.</value>
	public string ItemId
	{
		get
		{
			return itemId;
		}

		set
		{


			if(value == null)
			{
				itemId = null;

				gameObject.SetActive(false);
			}
			else
			{
				itemId = value;

				//get item balance
				//int balance = StoreInventory.GetItemBalance(itemId);
				int balance = DBManager.GetPlayerData(itemId).AsInt;

				//set button active
				gameObject.SetActive(true);

				ConfigureButton(balance);
			}

		}

	}
}
