using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UI equipped item control.
/// 
/// This class control all equipped item
/// </summary>
public class UIEquippedItemControl : MonoBehaviour 
{
	public delegate void EventOnItemEquipped(string itemId);
	/// <summary>
	/// Event for item equipped.
	/// </summary>
	public EventOnItemEquipped Evt_OnItemEquipped;

	public delegate void EventOnItemUnEquip(string itemId);
	/// <summary>
	/// Event for item unequip.
	/// </summary>
	public EventOnItemUnEquip Evt_OnItemUnEquip;

	/// <summary>
	/// The max items you can equip.
	/// </summary>
	public int maxItems = 5;

	/// <summary>
	/// The equipped item prefab.
	/// </summary>
	public GameObject equippedItemPrefab;

	/// <summary>
	/// The grid.
	/// 
	/// This hold bunch of item pic that is equiped
	/// </summary>
	public UIGrid grid;

	/// <summary>
	/// The equiped item identifiers.
	/// </summary>
	private List<string> equippedItemIds = new List<string>();

	void OnEnable()
	{

		LoadEquippedItem ();
	}

	void OnDisable()
	{
		//remove all equipped item
		equippedItemIds.Clear ();

		//remove from display
		//RemoveAllEquippedItemDisplay ();
	}

	void Awake()
	{
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
	/// Removes all equipped item display.
	/// </summary>
	void RemoveAllEquippedItemDisplay()
	{
		GameObject[] childs = new GameObject[grid.transform.childCount];

		for(int j=0; j<grid.transform.childCount; j++)
		{
			childs[j] = grid.transform.GetChild(j).gameObject;
		}

		for(int i=0; i<childs.Length; i++)
		{
			childs[i].transform.parent = null;
			NGUITools.Destroy(childs[i]);
		}

		/*
		for(int i=0; i<grid.transform.childCount; i++)
		{
			GameObject child = grid.transform.GetChild(i).gameObject;

			child.transform.parent = null;

			NGUITools.Destroy(child);
		}
		*/

		grid.Reposition ();
	}

	/// <summary>
	/// Loads the equipped item.
	/// 
	/// This will remove all current display and reload again
	/// </summary>
	void LoadEquippedItem()
	{
		PlayerEquippedItems data = PlayerEquippedItems.Load ();

		//clear list
		equippedItemIds.Clear ();

		//remove all display
		RemoveAllEquippedItemDisplay ();

		//get all equipped item id
		List<string> allEquippedItemIds = data.GetAllEquippedItems ();
		
		//sync up with data
		for(int i=0; i<allEquippedItemIds.Count; i++)
		{
			string addItemId = allEquippedItemIds[i];

			//add to lise
			equippedItemIds.Add(addItemId);
			
			//setup dispay
			if(equippedItemPrefab != null)
			{
				GameObject child = NGUITools.AddChild(grid.gameObject, equippedItemPrefab);
				
				grid.Reposition();
				
				//set item id
				child.GetComponent<UIEquippedItemDisplay>().ItemId = addItemId;
			}
			else
			{
				DebugEx.DebugError("Can not load item because prefab assigned can not create equipped item display");
			}
		}


	}

	/// <summary>
	/// Unequips the item.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="objectToDestroy">Object to destroy.</param>
	public void UnequipItem(string itemId, GameObject objectToDestroy)
	{
		PlayerEquippedItems data = PlayerEquippedItems.Load ();

		//remove from data
		if(data.UnEquipItem(itemId))
		{
			//remove from list
			equippedItemIds.Remove(itemId);

			//destroy object UI
			if(objectToDestroy.GetComponent<UIEquippedItemDisplay>() != null)
			{
				//NGUI say to remove from grid should first unparent it then destroy
				objectToDestroy.transform.parent = null;

				NGUITools.DestroyImmediate(objectToDestroy);

				//reposition
				grid.Reposition();

				if(Evt_OnItemUnEquip != null)
				{
					Evt_OnItemUnEquip(itemId);
				}
			}
			else
			{
				DebugEx.DebugError("Can't destroy item display it was not in the set: "+itemId);
			}
		}
		else
		{
			DebugEx.DebugError("There is an error while unequip an item: "+itemId);
		}
	}

	/// <summary>
	/// Equips the item.
	/// </summary>
	/// <returns><c>true</c>, if item was equiped, <c>false</c> otherwise.</returns>
	/// <param name="itemId">Item identifier.</param>
	public bool EquipItem(string itemId)
	{
		if(!CanEquipItem())
		{
			return false;
		}

		PlayerEquippedItems data = PlayerEquippedItems.Load ();

		//add to data
		if(data.EquipItem(itemId))
		{
			//add to list
			equippedItemIds.Add(itemId);

			//find item object UI
			GameObject equippedItem = GetEquippedItemDisplay(itemId);

			if(equippedItem == null)
			{
				if(equippedItemPrefab != null)
				{
					GameObject child = NGUITools.AddChild(grid.gameObject, equippedItemPrefab);

					grid.Reposition();

					//set item id
					child.GetComponent<UIEquippedItemDisplay>().ItemId = itemId;

					if(Evt_OnItemEquipped != null)
					{
						Evt_OnItemEquipped(itemId);
					}

					DebugEx.Debug("Equipped item: "+itemId);

					return true;
				}
				else
				{
					DebugEx.DebugError("No prefab assigned can not create equipped item display");
				}
			}
			else
			{
				DebugEx.DebugError("Item display is already in the set: "+itemId);
			}
		}
		else
		{
			DebugEx.DebugError("There is an error while equip an item: "+itemId);
		}

		return false;
	}

	/// <summary>
	/// Determines whether itemId is equipped.
	/// </summary>
	/// <returns><c>true</c> if this instance is item equipped the specified itemId; otherwise, <c>false</c>.</returns>
	/// <param name="itemId">Item identifier.</param>
	public bool IsItemEquipped(string itemId)
	{
		return false;
	}

	/// <summary>
	/// Determines whether container can accept more item.
	/// </summary>
	/// <returns><c>true</c> if this instance can equip item; otherwise, <c>false</c>.</returns>
	public bool CanEquipItem()
	{
		if(equippedItemIds.Count < maxItems)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Gets the equipped item display.
	/// </summary>
	/// <returns>The equipped item display.</returns>
	/// <param name="matchItemId">Match item identifier.</param>
	private GameObject GetEquippedItemDisplay(string matchItemId)
	{
		if(grid.transform.childCount > 0)
		{
			for(int i =0; i< grid.transform.childCount; i++)
			{
				GameObject child = grid.transform.GetChild(i).gameObject;

				if(child.GetComponent<UIEquippedItemDisplay>().ItemId == matchItemId)
				{
					return child;
				}
			}
		}

		return null;
	}
}
