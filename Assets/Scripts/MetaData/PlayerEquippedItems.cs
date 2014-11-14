using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SIS;

/// <summary>
/// Player equipped items.
/// 
/// This class used to remember equipments that player equipped
/// </summary>

[Serializable]
public class PlayerEquippedItems : PersistantMetaData 
{
	/// <summary>
	/// The equipped item identifiers.
	/// </summary>
	List<string> equippedItemIds = new List<string>();

	/// <summary>
	/// Load data.
	/// </summary>
	/// <param name="sync">If set to <c>true</c> sync.</param>
	public static PlayerEquippedItems Load(bool sync = true)
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<PlayerEquippedItems>())
		{
			//create new one
			PlayerEquippedItems newFile = new PlayerEquippedItems();
			
			SaveLoadManager.SharedManager.Save(newFile);
			
			return newFile;
		}

		PlayerEquippedItems ret = SaveLoadManager.SharedManager.Load<PlayerEquippedItems> ();

		if(sync)
		{
			ret.SyncWithStore ();
		}

		return ret;
	}

	/// <summary>
	/// Gets all equipped items.
	/// </summary>
	/// <returns>The all equipped items.</returns>
	/// <param name="sync">If set to <c>true</c> sync.</param>
	public List<string> GetAllEquippedItems(bool sync = true)
	{
		if(sync)
		{
			SyncWithStore();
		}

		return equippedItemIds;
	}

	/// <summary>
	/// Equips the item.
	/// </summary>
	/// <returns><c>true</c>, if item was equiped, <c>false</c> otherwise.</returns>
	/// <param name="sync">If set to <c>true</c> sync.</param>
	/// <param name="itemId">Item identifier.</param>
	public bool EquipItem(string itemId, bool sync = true)
	{
		if(sync)
		{
			SyncWithStore();
		}

		if(!equippedItemIds.Contains(itemId))
		{
			equippedItemIds.Add(itemId);
		}

		return save (this);
	}

	/// <summary>
	/// UnEquip item.
	/// </summary>
	/// <returns><c>true</c>, if equip item was uned, <c>false</c> otherwise.</returns>
	/// <param name="sync">If set to <c>true</c> sync.</param>
	/// <param name="itemId">Item identifier.</param>
	public bool UnEquipItem(string itemId, bool sync = true)
	{
		if(sync)
		{
			SyncWithStore();
		}

		if(equippedItemIds.Contains(itemId))
		{
			equippedItemIds.Remove(itemId);
		}

		return save (this);
	}

	/// <summary>
	/// Unequip all items.
	/// </summary>
	/// <returns><c>true</c>, if equip all items was uned, <c>false</c> otherwise.</returns>
	/// <param name="sync">If set to <c>true</c> sync.</param>
	public bool UnEquipAllItems(bool sync = true)
	{
		if(sync)
		{
			SyncWithStore();
		}

		equippedItemIds.Clear ();

		return save (this);
	}

	/// <summary>
	/// Determines whether sepcific item is equipped.
	/// </summary>
	/// <returns><c>true</c> if this instance is item equipped the specified itemId sync; otherwise, <c>false</c>.</returns>
	/// <param name="itemId">Item identifier.</param>
	/// <param name="sync">If set to <c>true</c> sync.</param>
	public bool IsItemEquipped(string itemId, bool sync = true)
	{
		if(sync)
		{
			SyncWithStore();
		}

		return equippedItemIds.Contains (itemId);
	}

	/// <summary>
	/// Sync this sync with store.
	/// </summary>
	public void Sync()
	{
		SyncWithStore ();

		save (this);
	}

	/// <summary>
	/// Syncs data the with store.
	/// </summary>
	private void SyncWithStore()
	{
		List<string> removedIds = new List<string>();
		
		//remove any item that balance is 0 but still equpped
		for(int i=0; i<equippedItemIds.Count; i++)
		{
			//if(StoreInventory.GetItemBalance(equippedItemIds[i]) <= 0)
			if(DBManager.GetPlayerData(equippedItemIds[i]).AsInt <= 0)
			{
				removedIds.Add(equippedItemIds[i]);
			}
		}
		
		for(int j=0; j<removedIds.Count; j++)
		{
			equippedItemIds.Remove (removedIds[j]);
		}
	}

	/// <summary>
	/// Save data.
	/// </summary>
	/// <param name="data">Data.</param>
	private bool save(PlayerEquippedItems data)
	{
		return SaveLoadManager.SharedManager.Save (data);
	}
}
