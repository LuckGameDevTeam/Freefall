using UnityEngine;
using System.Collections;

/// <summary>
/// UI equipable item control.
/// 
/// This class control all equipable item
/// </summary>
public class UIEquipableItemControl : MonoBehaviour 
{
	public string itemFullKey = "ItemFull";
	public string itemFullDescKey = "ItemFullDesc";

	public delegate void EventOnItemUnEquip(string itemId);
	/// <summary>
	/// Event for item unequip.
	/// </summary>
	public EventOnItemUnEquip Evt_OnItemUnEquip;

	/// <summary>
	/// Reference to UIEquippedItemControl.
	/// </summary>
	public UIEquippedItemControl equippedControl;

	/// <summary>
	/// Reference to UIEquipRoot.
	/// </summary>
	private UIEquipRoot equipRoot;

	void Awake()
	{
		//find UIEquipRoot
		equipRoot = NGUITools.FindInParents<UIEquipRoot> (gameObject);

		if(equippedControl == null)
		{
			Debug.LogError("Require UIEquippedItemControl to work with ");
		}

		//register event for item unequip
		equippedControl.Evt_OnItemUnEquip += OnItemUnEquipped;
	}

	void OnEnable()
	{
		//synce player equipped item data
		PlayerEquippedItems pei = PlayerEquippedItems.Load ();
		pei.Sync ();
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
	/// Equips specific item.
	/// </summary>
	/// <returns><c>true</c>, if item was equiped, <c>false</c> otherwise.</returns>
	/// <param name="itemId">Item identifier.</param>
	public bool EquipItem(string itemId)
	{
		//check if can equip item
		if(equippedControl.CanEquipItem())
		{
			//equip item
			return equippedControl.EquipItem(itemId);
		}
		else//if can't equip item mean there is no more room for item to equip
		{
			//show alert
			equipRoot.alertControl.ShowAlertWindow(itemFullKey, itemFullDescKey);

			return false;
		}

	}

	/// <summary>
	/// Determines whether this item is equipped or not.
	/// </summary>
	/// <returns><c>true</c> if this instance is item equiped the specified itemId; otherwise, <c>false</c>.</returns>
	/// <param name="itemId">Item identifier.</param>
	public bool IsItemEquiped(string itemId)
	{
		PlayerEquippedItems data = PlayerEquippedItems.Load ();

		return data.IsItemEquipped (itemId);
	}

	/// <summary>
	/// Handle the item unequipped event.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	void OnItemUnEquipped(string itemId)
	{
		if(Evt_OnItemUnEquip != null)
		{
			Evt_OnItemUnEquip(itemId);
		}
	}
	
}
