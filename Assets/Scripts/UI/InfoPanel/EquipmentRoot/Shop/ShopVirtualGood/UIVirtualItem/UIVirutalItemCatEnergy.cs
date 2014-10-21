using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// UI virutal item cat energy.
/// 
/// This class is special designed for cat energy item
/// Once cat energy purchased, it will be convert to player life 
/// </summary>
public class UIVirutalItemCatEnergy : UIVirtualItem 
{
	/// <summary>
	/// The give life.
	/// </summary>
	public int giveLife = 3;

	protected override void PurchasedAction(UIPurchaseControl control, string itemId)
	{
		base.PurchasedAction (control, itemId);

		//give player amount of life
		StoreInventory.GiveItem (StoreAssets.PLAYER_LIFE_ITEM_ID, giveLife);

		//take cat energy away from player
		StoreInventory.TakeItem (itemId, 1);
	}
}
