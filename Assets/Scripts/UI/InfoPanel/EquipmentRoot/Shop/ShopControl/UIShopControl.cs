using UnityEngine;
using System.Collections;
using Soomla.Store;

public enum ShopType
{
	Item,
	CoinPacks,
	Characters
}

/// <summary>
/// UI shop control.
/// 
/// This class is to control which page in store should be present
/// </summary>
public class UIShopControl : MonoBehaviour 
{
	/// <summary>
	/// Current shop type
	/// </summary>
	public ShopType shopType = ShopType.Item;

	/// <summary>
	/// The initial type of the shop.
	/// </summary>
	public ShopType initialShopType = ShopType.Item;

	/// <summary>
	/// Reference to item shop root gameobject.
	/// </summary>
	public GameObject itemShop;

	/// <summary>
	/// Reference to coin packs shop root gameobject
	/// </summary>
	public GameObject coinPacksShop;

	/// <summary>
	/// Reference to characeters shop root gameobject
	/// </summary>
	public GameObject charactersShop;

	/// <summary>
	/// Reference to item shop button gameobject
	/// </summary>
	public GameObject itemShopBtn;

	/// <summary>
	/// Reference to coin packs shop button gameobject
	/// </summary>
	public GameObject coinPacksShopBtn;

	/// <summary>
	/// Reference to characters shop button gameobject
	/// </summary>
	public GameObject charactersShopBtn;


	void Awake()
	{

	}

	void OnEnable()
	{
		//change shop type to initial shop type
		ChangeShopType (initialShopType);
	}

	// Use this for initialization
	void Start () 
	{
		//change shop type to initial shop type
		ChangeShopType (initialShopType);

	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Show item shop.
	/// </summary>
	/// <param name="btn">Button.</param>
	public void ShowItemShop(GameObject btn)
	{
		//change shop type to item shop
		ChangeShopType (ShopType.Item);
	}

	/// <summary>
	/// Show coin packs shop.
	/// </summary>
	/// <param name="btn">Button.</param>
	public void ShowCoinPacksShop(GameObject btn)
	{
		//change shop type to coin packs shop
		ChangeShopType (ShopType.CoinPacks);
	}

	/// <summary>
	/// Show characters shop.
	/// </summary>
	/// <param name="btn">Button.</param>
	public void ShowCharactersShop(GameObject btn)
	{
		//change shop type to characters shop
		ChangeShopType (ShopType.Characters);
	}

	/// <summary>
	/// Changes the type of the shop.
	/// </summary>
	/// <param name="type">Type.</param>
	public void ChangeShopType(ShopType type)
	{
		//set current shop type
		shopType = type;

		switch (shopType) 
		{
			//enable item shop
			case ShopType.Item:

			//deal with shop button
			itemShopBtn.GetComponent<UIImageButton>().isEnabled = false;
			itemShopBtn.GetComponent<UIShopSwitchButton>().Select();

			coinPacksShopBtn.GetComponent<UIImageButton>().isEnabled = true;
			coinPacksShopBtn.GetComponent<UIShopSwitchButton>().Deselect();

			charactersShopBtn.GetComponent<UIImageButton>().isEnabled = true;
			charactersShopBtn.GetComponent<UIShopSwitchButton>().Deselect();

			itemShop.SetActive(true);
			coinPacksShop.SetActive(false);
			charactersShop.SetActive(false);

			break;
		
			//enable coin packs shop
			case ShopType.CoinPacks:

			//deal with shop button
			itemShopBtn.GetComponent<UIImageButton>().isEnabled = true;
			itemShopBtn.GetComponent<UIShopSwitchButton>().Deselect();

			coinPacksShopBtn.GetComponent<UIImageButton>().isEnabled = false;
			coinPacksShopBtn.GetComponent<UIShopSwitchButton>().Select();

			charactersShopBtn.GetComponent<UIImageButton>().isEnabled = true;
			charactersShopBtn.GetComponent<UIShopSwitchButton>().Deselect();
			
			itemShop.SetActive(false);
			coinPacksShop.SetActive(true);
			charactersShop.SetActive(false);

			break;

			//enable characters shop
			case ShopType.Characters:

			//deal with shop button
			itemShopBtn.GetComponent<UIImageButton>().isEnabled = true;
			itemShopBtn.GetComponent<UIShopSwitchButton>().Deselect();

			coinPacksShopBtn.GetComponent<UIImageButton>().isEnabled = true;
			coinPacksShopBtn.GetComponent<UIShopSwitchButton>().Deselect();

			charactersShopBtn.GetComponent<UIImageButton>().isEnabled = false;
			charactersShopBtn.GetComponent<UIShopSwitchButton>().Select();
			
			itemShop.SetActive(false);
			coinPacksShop.SetActive(false);
			charactersShop.SetActive(true);

			break;
		}
	}
}
