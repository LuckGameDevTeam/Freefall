using UnityEngine;
using System.Collections;
using Soomla.Store;
using SIS;

/// <summary>
/// UI level item.
/// 
/// This class display main level item 
/// </summary>
public class UILevelItem : MonoBehaviour 
{
	public string confirmKey = "Confirm";
	/// <summary>
	/// The level.
	/// </summary>
	public int level = 1;

	/// <summary>
	/// The number of sub level.
	/// </summary>
	public int numberOfSubLevel = 5;

	public string levelTitleKey;

	public string levelDescKey;

	/// <summary>
	/// The level item identifier.
	/// </summary>
	public string levelItemId;

	/// <summary>
	/// The unlock price.
	/// </summary>
	public int unlockPrice = 1000;

	/// <summary>
	/// The coin mark.
	/// </summary>
	public GameObject coinMark;

	/// <summary>
	/// The lock indicator.
	/// </summary>
	public GameObject lockIndicator;

	/// <summary>
	/// The confirm button.
	/// </summary>
	public GameObject confirmButton;

	/// <summary>
	/// Reference to level selection control.
	/// </summary>
	private UILevelSelectionControl levelSelectionControl;

	void Awake()
	{
		//find UILevelSelectionControl
		levelSelectionControl = NGUITools.FindInParents<UILevelSelectionControl> (gameObject);


		//register event for purchase window
		levelSelectionControl.purchaseControl.Evt_Close += LevelPurchaseWindowClose;
		levelSelectionControl.purchaseControl.Evt_ErrorOccur += LevelPurchaseWindowErrorOccur;
		levelSelectionControl.purchaseControl.Evt_InsufficientFunds += LevelPurchaseWindowInsufficientFunds;
		levelSelectionControl.purchaseControl.Evt_ItemPurchased += LevelPurchaseWindowItemPurchased;
		levelSelectionControl.purchaseControl.Evt_ItemPurchaseStarted += LevelPurchaseWindowPurchaseStarted;

	}

	void OnEnable()
	{
		ConfigureLevelItem ();
	}

	// Use this for initialization
	void Start () 
	{
		ConfigureLevelItem ();
	}

	/// <summary>
	/// Configures the level item.
	/// </summary>
	void ConfigureLevelItem()
	{
		if(gameObject.activeInHierarchy)
		{
			//if(StoreInventory.NonConsumableItemExists(levelItemId) || StoreInventory.NonConsumableItemExists(StoreAssets.UNLOCK_ALL_LEVEL_NO_AD_ITEM_ID))
			if(DBManager.isPurchased(levelItemId) || DBManager.isPurchased("BuyFullGame"))
			{
				//hide locker
				lockIndicator.SetActive(false);
				
				//change button to confirm
				//confirmButton.GetComponentInChildren<UILocalize>().key = confirmKey;
				confirmButton.GetComponentInChildren<UILabel>().text = Localization.Get(confirmKey);

				coinMark.SetActive(false);
			}
			else
			{
				//show locker
				lockIndicator.SetActive(true);

				//show unlock price
				//confirmButton.GetComponentInChildren<UILocalize>().key = unlockPrice.ToString();
				//confirmButton.GetComponentInChildren<UILabel>().text = unlockPrice.ToString();
				confirmButton.GetComponentInChildren<UILabel>().text = IAPManager.GetIAPObject(levelItemId).virtualPrice[0].amount.ToString();

				coinMark.SetActive(true);
			}
		}

	}

	/// <summary>
	///	level select event.
	/// </summary>
	public void OnLevelSelect()
	{
#if TestMode
		if(GameObject.FindObjectOfType(typeof(LevelLoadManager)))
		{
			(GameObject.FindObjectOfType(typeof(LevelLoadManager)) as LevelLoadManager).LoadLevel ("TestField");
		}
		else
		{
			Application.LoadLevel("TestField");
		}
		
#else
		//if(StoreInventory.NonConsumableItemExists(levelItemId) || StoreInventory.NonConsumableItemExists(StoreAssets.UNLOCK_ALL_LEVEL_NO_AD_ITEM_ID))
		if(DBManager.isPurchased(levelItemId) || DBManager.isPurchased("BuyFullGame"))
		{
			//enter sub level selection
			levelSelectionControl.ShowSubLevelSelection (level);
		}
		else
		{
			//buy this level
			levelSelectionControl.purchaseControl.ShowPurchaseWindow(levelItemId, levelTitleKey, levelDescKey) ;
		}
#endif
	}

	protected virtual void LevelPurchaseWindowClose()
	{
	}

	protected virtual void LevelPurchaseWindowErrorOccur(UIPurchaseControl control, string itemId, string errorMessage)
	{

	}

	protected virtual void LevelPurchaseWindowInsufficientFunds(UIPurchaseControl control, string itemId)
	{
	}

	protected virtual void LevelPurchaseWindowItemPurchased(UIPurchaseControl control, string itemId)
	{

		if((itemId == levelItemId) || (itemId == StoreAssets.UNLOCK_ALL_LEVEL_NO_AD_ITEM_ID))
		{
			/*
			if(level > 1)
			{
				bool unlockFirstSubLevel = true;
				
				SubLevelData slData = SubLevelData.Load();
				
				//check if previous all sub level are unlocked
				for(int i=1; i<=numberOfSubLevel; i++)
				{
					if(!slData.IsSubLevelUnlocked(level-1, i))
					{
						unlockFirstSubLevel = false;
						break;
					}
				}
				
				if(unlockFirstSubLevel)
				{
					//unlock first sub level of this level
					slData.UnlockSubLevel(level, 1);
				}
				
			}
			else
			{
				Debug.LogError("You can not have level less or equal then 1");
			}
			*/

			LevelManager.SharedLevelManager.SyncWithMainLevel(level);
			
			ConfigureLevelItem ();
		}

	}

	protected virtual void LevelPurchaseWindowPurchaseStarted(UIPurchaseControl control, string itemId)
	{
	}
}
