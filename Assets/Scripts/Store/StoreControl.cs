using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

/// <summary>
/// Store control.
/// 
/// Do initialize as early as possible when app launch
/// 
/// Responsible to init Soomla store and any related store setup
/// </summary>
public class StoreControl 
{
	static StoreControl instance = null;

	public delegate void EventStoreInitComplete();
	public EventStoreInitComplete Evt_StoreInitComplete;

	/// <summary>
	/// indicate store initlized or not.
	/// </summary>
	bool isStoreInitlized = false;

	public static StoreControl SharedStoreControl
	{
		get
		{
			if(instance == null)
			{
				instance = new StoreControl();
				
				instance.InitStore();
			}
			
			return instance;
		}
	}

	/// <summary>
	/// Inits the store.
	/// 
	/// If store been initilized, it will not reinitialized
	/// </summary>
	void InitStore()
	{
		if(isStoreInitlized == false)
		{
			StoreEvents.OnSoomlaStoreInitialized += StoreInitialized;

			SoomlaStore.Initialize(new StoreAssets());

			isStoreInitlized = true;
		}
		else
		{
			Debug.LogWarning("Store been initialized");
		}
	}

	/// <summary>
	/// Stores the initialized.
	/// 
	/// Put intital action here when soomla store finished intialization
	/// </summary>
	void StoreInitialized()
	{


		//test
		StoreInventory.GiveItem (StoreAssets.CAT_COIN_CURRENCY_ITEM_ID, 10000);

		int firstTimeLaunch = 0;

		firstTimeLaunch = PlayerPrefs.GetInt("TimeOfLaunch");

		if(firstTimeLaunch == 0)
		{
			StoreInventory.GiveItem(StoreAssets.PLAYER_LIFE_ITEM_ID, 3);

			//give bell cat
			StoreInventory.GiveItem (StoreAssets.CHARACTER_BELL_CAT_ITEM_ID, 1);
			//equip it if need
			if(PlayerCharacter.Load().characterName == "")
			{
				StoreInventory.EquipVirtualGood (StoreAssets.CHARACTER_BELL_CAT_ITEM_ID);
				
				PlayerCharacter pc = new PlayerCharacter();
				pc.characterName = StoreAssets.CHARACTER_BELL_CAT_ITEM_ID;
				
				PlayerCharacter.Save(pc);
			}

		}

		PlayerPrefs.SetInt ("TimeOfLaunch", firstTimeLaunch + 1);



		//unlock level1 for player
		if(!StoreInventory.NonConsumableItemExists(StoreAssets.UNLOCK_LEVEL_1_ITEM_ID))
		{
			StoreInventory.AddNonConsumableItem(StoreAssets.UNLOCK_LEVEL_1_ITEM_ID);
		}

		/*
		//unlock level 1's all sub levels
		for(int i=1; i<=5; i++)
		{
			SubLevelData slData = SubLevelData.Load();

			slData.UnlockSubLevel(1, i);
		}
		*/
		SubLevelData slData = SubLevelData.Load ();
		slData.UnlockSubLevel (1, 1);


		if(Evt_StoreInitComplete != null)
		{
			Evt_StoreInitComplete();
		}
	}

	/// <summary>
	/// Gets all owned items' id.
	/// </summary>
	/// <returns>The all owned items identifier.</returns>
	public List<string> GetAllOwnedItemsId()
	{
		string[] allItemsId = StoreAssets.GetAllItemsId ();

		List<string> ret = new List<string> ();

		for(int i=0; i<allItemsId.Length; i++)
		{
			//if this item's quantity is not 0
			if(StoreInventory.GetItemBalance(allItemsId[i]) > 0)
			{
				ret.Add(allItemsId[i]);
			}
		}

		return ret;
	}

	/// <summary>
	/// Gets all owned characters id.
	/// </summary>
	/// <returns>The all owned characters identifier.</returns>
	public List<string> GetAllOwnedCharactersId()
	{
		string[] charactersId = StoreAssets.GetAllCharactersId ();

		List<string> ret = new List<string> ();

		for(int i=0; i<charactersId.Length; i++)
		{
			if(StoreInventory.GetItemBalance(charactersId[i]) > 0)
			{
				ret.Add(charactersId[i]);
			}
		}

		return ret;
	}
}
