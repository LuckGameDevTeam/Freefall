using UnityEngine;
using System.Collections;
using Soomla.Store;

/// <summary>
/// Store assets.
/// This class contain all item's data in store
/// </summary>
public class StoreAssets : IStoreAssets 
{

	/// <summary>
	/// see parent.
	/// </summary>
	public int GetVersion() 
	{
		return 0;
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrency[] GetCurrencies() 
	{
		return new VirtualCurrency[]{CAT_COIN_CURRENCY};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualGood[] GetGoods() 
	{
		return new VirtualGood[] {PLAYER_LIFE ,CAT_ENERGY, CAT_COOKIE_BIG, CAT_CROWN, CAT_MAGNET, CAT_SHIELD, CAT_SWORD, CAT_STICK, ITEM_BOX,
			CHARACTER_BELL_CAT, CHARACTER_TARZAN_CAT, CHARACTER_NINJA_CAT, CHARACTER_CANDY_CAT, CHARACTER_PUMPKIN_CAT, CHARACTER_GHOST_CAT, CHARACTER_IRON_CAT, CHARACTER_HULK_CAT};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrencyPack[] GetCurrencyPacks() 
	{
		return new VirtualCurrencyPack[] {COIN_PACK_1, COIN_PACK_2, COIN_PACK_3, COIN_BOX};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCategory[] GetCategories() 
	{
		return new VirtualCategory[]{};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public NonConsumableItem[] GetNonConsumableItems() 
	{
		return new NonConsumableItem[]{
			UNLOCK_ALL_LEVEL_NO_AD, UNLOCK_LEVEL_1, UNLOCK_LEVEL_2, UNLOCK_LEVEL_3, 
			UNLOCK_LEVEL_4, UNLOCK_LEVEL_5
		};
	}

	///////////////////////////////////////////////////////Helper function//////////////////////////////////////////////////////////

	/// <summary>
	/// 
	/// Return all items id that can be used in game
	/// </summary>
	public static string[] GetAllItemsId()
	{
		string[] items = {
			CAT_COOKIE_BIG_ITEM_ID, 
			CAT_CROWN_ITEM_ID, 
			CAT_MAGNET_ITEM_ID, 
			CAT_SHIELD_ITEM_ID, 
			CAT_SWORD_ITEM_ID, 
			CAT_STICK_ITEM_ID, 
			CAT_DOLL_ITEM_ID
		};

		return items;
	}

	/// <summary>
	/// 
	/// Return all characters id that can be used in game
	/// </summary>
	public static string[] GetAllCharactersId()
	{
		string[] characters = {
			CHARACTER_BELL_CAT_ITEM_ID,
			CHARACTER_TARZAN_CAT_ITEM_ID,
			CHARACTER_NINJA_CAT_ITEM_ID,
			CHARACTER_CANDY_CAT_ITEM_ID,
			CHARACTER_PUMPKIN_CAT_ITEM_ID,
			CHARACTER_GHOST_CAT_ITEM_ID,
			CHARACTER_IRON_CAT_ITEM_ID,
			CHARACTER_HULK_CAT_ITEM_ID
		};

		return characters;
	}

	///////////////////////////////////////////////////////Helper function//////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////Assets definition///////////////////////////////////////////////////////////

	/// <summary>
	/// Cat coin
	/// 
	/// The in game currency
	/// </summary>
	public const string CAT_COIN_CURRENCY_ITEM_ID = "CatCoin";

	/// <summary>
	/// Player life
	/// 
	/// You can't play game if life is 0, but life will regenrate
	/// </summary>
	public const string PLAYER_LIFE_ITEM_ID = "PlayerLife";

	/// <summary>
	/// Cat energy
	/// 
	/// Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CAT_ENERGY_ITEM_ID = "CatEnergy";

	/// <summary>
	/// Cat cookie big 
	/// 
	/// Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CAT_COOKIE_BIG_ITEM_ID = "CatCookieBig";

	/// <summary>
	/// Cat crown
	/// 
	/// Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CAT_CROWN_ITEM_ID = "CatCrown";

	/// <summary>
	/// Cat magnet
	/// 
	/// Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CAT_MAGNET_ITEM_ID = "CatMagnet";

	/// <summary>
	/// Cat shield
	/// 
	/// Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CAT_SHIELD_ITEM_ID = "CatShield";

	/// <summary>
	/// Cat sword
	/// 
	/// Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CAT_SWORD_ITEM_ID = "CatSword";
	
	/// <summary>
	/// Cat stick
	/// 
	/// Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CAT_STICK_ITEM_ID = "CatStick";

	/// <summary>
	/// Cat doll
	/// 
	/// Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CAT_DOLL_ITEM_ID = "CatDoll";

	/// <summary>
	/// Item box
	/// 
	/// Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string ITEM_BOX_ITEM_ID = "ItemBox";

	/// <summary>
	/// Character bell cat
	/// 
	/// Non-Consumable
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CHARACTER_BELL_CAT_ITEM_ID = "BellCat";

	/// <summary>
	/// Character ninja cat
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CHARACTER_NINJA_CAT_ITEM_ID = "NinjaCat";

	/// <summary>
	/// Character tarzan cat
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CHARACTER_TARZAN_CAT_ITEM_ID = "TarzanCat";

	/// <summary>
	/// Character candy cat
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CHARACTER_CANDY_CAT_ITEM_ID = "CandyCat";

	/// <summary>
	/// Character pumpkin cat
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CHARACTER_PUMPKIN_CAT_ITEM_ID = "PumpkinCat";

	/// <summary>
	/// Character iron cat
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CHARACTER_IRON_CAT_ITEM_ID = "IronCat";

	/// <summary>
	/// Character hulk cat
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CHARACTER_HULK_CAT_ITEM_ID = "HulkCat";

	/// <summary>
	/// Character ghost cat
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string CHARACTER_GHOST_CAT_ITEM_ID = "GhostCat";

	/// <summary>
	/// 1 coin pack
	/// 
	/// Consumable
	/// 
	/// Buy in market with real money
	/// </summary>
	public const string COIN_PACK_1_PRODUCT_ID = "OneCoinPack";

	/// <summary>
	/// 2 coin pack
	/// 
	/// Consumable
	/// 
	/// Buy in market with real money
	/// </summary>
	public const string COIN_PACK_2_PRODUCT_ID = "TwoCoinPack";

	/// <summary>
	/// 3 coin pack
	/// 
	/// Consumable
	/// 
	/// Buy in market with real money
	/// </summary>
	public const string COIN_PACK_3_PRODUCT_ID = "ThreeCoinPack";

	/// <summary>
	/// Coin box
	/// 
	/// Consumable
	/// 
	/// Buy in market with real money
	/// </summary>
	public const string COIN_BOX_PRODUCT_ID = "CoinBox";

	/// <summary>
	/// Unlock all levels and remove ad from game
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string UNLOCK_ALL_LEVEL_NO_AD_ITEM_ID = "BuyFullGame";

	/// <summary>
	/// Unlock all levels and remove ad from game
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string UNLOCK_ALL_LEVEL_NO_AD_PRODUCT_ID = "BuyFullGame";

	/// <summary>
	/// Unlock level1
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string UNLOCK_LEVEL_1_ITEM_ID = "UnlockLevel1";

	/// <summary>
	/// Unlock level2
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string UNLOCK_LEVEL_2_ITEM_ID = "UnlockLevel2";

	/// <summary>
	/// Unlock level3
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string UNLOCK_LEVEL_3_ITEM_ID = "UnlockLevel3";

	/// <summary>
	/// Unlock level4
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string UNLOCK_LEVEL_4_ITEM_ID = "UnlockLevel4";

	/// <summary>
	/// Unlock level5
	/// 
	/// Non-Consumable
	/// 
	/// Virtual item can only buy with cat coin
	/// </summary>
	public const string UNLOCK_LEVEL_5_ITEM_ID = "UnlockLevel5";

	///////////////////////////////////////////////////////Assets definition///////////////////////////////////////////////////////////


	public static VirtualCurrency CAT_COIN_CURRENCY = new VirtualCurrency (

		"CatCoineCurrency",
		"Buy virtual item in game",
		CAT_COIN_CURRENCY_ITEM_ID
	);

	public static VirtualGood PLAYER_LIFE = new SingleUseVG (
		
		"Playerlife",
		"Player life that use to play game",
		PLAYER_LIFE_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 0)
		);

	public static VirtualGood CAT_ENERGY = new SingleUseVG (

		"CatEnergyItem",
		"Refill player's energy",
		CAT_ENERGY_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 5000)
	);

	public static VirtualGood CAT_COOKIE_BIG = new SingleUseVG (

		"CatCookieBigItem",
		"Increase player hp by 3",
		CAT_COOKIE_BIG_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 300)
	);

	public static VirtualGood CAT_CROWN = new SingleUseVG (

		"CatCrownItem",
		"",
		CAT_CROWN_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 1000)
	);

	public static VirtualGood CAT_MAGNET = new SingleUseVG (
		
		"CatMagnetItem",
		"",
		CAT_MAGNET_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 500)
	);

	public static VirtualGood CAT_SHIELD = new SingleUseVG (
		
		"CatShieldItem",
		"",
		CAT_SHIELD_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 500)
	);

	public static VirtualGood CAT_SWORD = new SingleUseVG (
		
		"CatSwordItem",
		"",
		CAT_SWORD_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 500)
	);

	public static VirtualGood CAT_STICK = new SingleUseVG (
		
		"CatStickItem",
		"",
		CAT_STICK_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 500)
	);

	public static VirtualGood CAT_DOLL = new SingleUseVG (

		"CatDollItem",
		"",
		CAT_DOLL_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 1000)
	);

	public static VirtualGood ITEM_BOX = new SingleUseVG (
		
		"ItemBox",
		"Get 5 per item",
		ITEM_BOX_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 5000)
	);

	public static VirtualCurrencyPack COIN_PACK_1 = new VirtualCurrencyPack (

		"10000CatCoin",
		"Get 10000 cat coins",
		COIN_PACK_1_PRODUCT_ID,
		10000,
		CAT_COIN_CURRENCY_ITEM_ID,
		new PurchaseWithMarket(COIN_PACK_1_PRODUCT_ID, 0.99) 
	);

	public static VirtualCurrencyPack COIN_PACK_2 = new VirtualCurrencyPack (
		
		"20000CatCoin",
		"Get 20000 cat coins",
		COIN_PACK_2_PRODUCT_ID,
		20000,
		CAT_COIN_CURRENCY_ITEM_ID,
		new PurchaseWithMarket(COIN_PACK_2_PRODUCT_ID, 1.99) 
	);

	public static VirtualCurrencyPack COIN_PACK_3 = new VirtualCurrencyPack (
		
		"30500CatCoin",
		"Get 30500 cat coins",
		COIN_PACK_3_PRODUCT_ID,
		30500,
		CAT_COIN_CURRENCY_ITEM_ID,
		new PurchaseWithMarket(COIN_PACK_3_PRODUCT_ID, 2.99) 
	);

	public static VirtualCurrencyPack COIN_BOX = new VirtualCurrencyPack (
		
		"51000CatCoin",
		"Get 51000 cat coins",
		COIN_BOX_PRODUCT_ID,
		51000,
		CAT_COIN_CURRENCY_ITEM_ID,
		new PurchaseWithMarket(COIN_BOX_PRODUCT_ID, 4.99) 
	);

	public static EquippableVG CHARACTER_BELL_CAT  = new EquippableVG (
		EquippableVG.EquippingModel.GLOBAL,
		"CharacterBellCat",
		"",
		CHARACTER_BELL_CAT_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 0)
	);

	public static EquippableVG CHARACTER_TARZAN_CAT  = new EquippableVG (
		EquippableVG.EquippingModel.GLOBAL,
		"CharacterTarzanCat",
		"",
		CHARACTER_TARZAN_CAT_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 500)
	);

	public static EquippableVG CHARACTER_NINJA_CAT  = new EquippableVG (
		EquippableVG.EquippingModel.GLOBAL,
		"CharacterNinjaCat",
		"",
		CHARACTER_NINJA_CAT_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 1000)
	);

	public static EquippableVG CHARACTER_CANDY_CAT  = new EquippableVG (
		EquippableVG.EquippingModel.GLOBAL,
		"CharacterCandyCat",
		"",
		CHARACTER_CANDY_CAT_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 1500)
	);

	public static EquippableVG CHARACTER_PUMPKIN_CAT  = new EquippableVG (
		EquippableVG.EquippingModel.GLOBAL,
		"CharacterPumpkinCat",
		"",
		CHARACTER_PUMPKIN_CAT_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 2000)
	);

	public static EquippableVG CHARACTER_GHOST_CAT  = new EquippableVG (
		EquippableVG.EquippingModel.GLOBAL,
		"CharacterGhostCat",
		"",
		CHARACTER_GHOST_CAT_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 5000)
	);

	public static EquippableVG CHARACTER_IRON_CAT  = new EquippableVG (
		EquippableVG.EquippingModel.GLOBAL,
		"CharacterIronCat",
		"",
		CHARACTER_IRON_CAT_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 5000)
	);

	public static EquippableVG CHARACTER_HULK_CAT  = new EquippableVG (
		EquippableVG.EquippingModel.GLOBAL,
		"CharacterHulkCat",
		"",
		CHARACTER_HULK_CAT_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 5000)
	);

	public static NonConsumableItem UNLOCK_ALL_LEVEL_NO_AD  = new NonConsumableItem (
		"UnlockAllLevelNoAd",
		"",
		UNLOCK_ALL_LEVEL_NO_AD_ITEM_ID,
		new PurchaseWithMarket(UNLOCK_ALL_LEVEL_NO_AD_PRODUCT_ID, 0.99)
	);

	public static NonConsumableItem UNLOCK_LEVEL_1  = new NonConsumableItem (
		"UnlockLevel1",
		"",
		UNLOCK_LEVEL_1_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 0)
	);

	public static NonConsumableItem UNLOCK_LEVEL_2  = new NonConsumableItem (
		"UnlockLevel2",
		"",
		UNLOCK_LEVEL_2_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 500)
	);

	public static NonConsumableItem UNLOCK_LEVEL_3  = new NonConsumableItem (
		"UnlockLevel3",
		"",
		UNLOCK_LEVEL_3_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 1000)
	);

	public static NonConsumableItem UNLOCK_LEVEL_4  = new NonConsumableItem (
		"UnlockLevel4",
		"",
		UNLOCK_LEVEL_4_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 1500)
	);

	public static NonConsumableItem UNLOCK_LEVEL_5  = new NonConsumableItem (
		"UnlockLevel5",
		"",
		UNLOCK_LEVEL_5_ITEM_ID,
		new PurchaseWithVirtualItem(CAT_COIN_CURRENCY_ITEM_ID, 2000)
	);
}
