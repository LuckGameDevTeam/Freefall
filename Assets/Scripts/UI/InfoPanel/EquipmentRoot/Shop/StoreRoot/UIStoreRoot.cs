using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// Store root.
/// 
/// All store related gameobject must under this one
/// Responsible to switch iphone or ipad UI
/// </summary>
public class UIStoreRoot : UIEquipmentRoot 
{

	public delegate void EventOnCharacterSelected(string itemId);
	/// <summary>
	/// Event for character selected.
	/// </summary>
	public EventOnCharacterSelected Evt_OnCharacterSelected;

	public delegate void EventOnCharacterDeselected(string itemId);
	/// <summary>
	/// Event for character deselected.
	/// </summary>
	public EventOnCharacterDeselected Evt_OnCharacterDeselected;

	/// <summary>
	/// Reference to StoreControl
	/// </summary>
	StoreControl sc;

	/// <summary>
	/// The purchase control.
	/// </summary>
	public UIPurchaseControl purchaseControl;

	void Awake()
	{
		//test
		//init store
		sc = StoreControl.SharedStoreControl;

		if(purchaseControl == null)
		{
			Debug.LogError("You must assign UIPurchaseControl to "+gameObject.name);
		}

	}
	
	// Use this for initialization
	void Start () 
	{
		//force to close purchase control window
		purchaseControl.ClosePurchaseWindow ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Selects the character.
	/// 
	/// It also equip character
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void SelectCharacter(string itemId)
	{
		//equip character
		StoreInventory.EquipVirtualGood (itemId);

		//save character name as itemid
		PlayerCharacter pc = new PlayerCharacter ();
		pc.characterName = itemId;
		
		PlayerCharacter.Save (pc);

		if(Evt_OnCharacterSelected != null)
		{
			Evt_OnCharacterSelected(itemId);
		}
	}

	/// <summary>
	/// Deselects the character.
	/// 
	/// It also unequip character
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void DeselectCharacter(string itemId)
	{
		//unequip character
		StoreInventory.UnEquipVirtualGood (itemId);

		//remove character name from save data
		PlayerCharacter pc = new PlayerCharacter ();
		pc.characterName = "";
		
		PlayerCharacter.Save (pc);

		if(Evt_OnCharacterDeselected != null)
		{
			Evt_OnCharacterDeselected(itemId);
		}
	}
}
