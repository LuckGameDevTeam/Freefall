using UnityEngine;
using System.Collections;

public enum EquipmentPage
{
	Menu,
	CharacterSelection,
	Equip
}

/// <summary>
/// UI equipment control.
/// 
/// This class control menu page, shop, equip.
/// This class can switch between menu page, shop, equip.
/// </summary>
public class UIEquipmentControl : MonoBehaviour 
{
	public delegate void EventEquipmentClose();
	public EventEquipmentClose Evt_EquipmentClose;

	/// <summary>	
	/// The current page.
	/// </summary>
	EquipmentPage currentPage = EquipmentPage.Menu;

	/// <summary>
	/// The menu page.
	/// </summary>
	public UIEquipmentRoot MenuPage;

	/// <summary>
	/// Gets the menu page.
	/// </summary>
	/// <value>The equipment menu.</value>
	public UIMenuRoot GetMenuPage{get{return (UIMenuRoot)MenuPage;}}

	/// <summary>
	/// The character selection page.
	/// </summary>
	public UIEquipmentRoot characterSelectionPage;

	/// <summary>
	/// Gets the get store page.
	/// </summary>
	/// <value>The get store.</value>
	public UIStoreRoot GetStorePage{get{return (UIStoreRoot)characterSelectionPage;}}

	/// <summary>
	/// The equip page.
	/// </summary>
	public UIEquipmentRoot equipPage;

	/// <summary>
	/// Gets the get equip page.
	/// </summary>
	/// <value>The get equip page.</value>
	public UIEquipRoot GetEquipPage{get{return (UIEquipRoot)equipPage;}}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void DirectToPage(EquipmentPage page)
	{
		switch(page)
		{
		case EquipmentPage.Menu:

			characterSelectionPage.Close();
			equipPage.Close();
			MenuPage.Open();

			break;

		case EquipmentPage.CharacterSelection:

			equipPage.Close();
			MenuPage.Close();
			characterSelectionPage.Open();

			break;

		case EquipmentPage.Equip:

			MenuPage.Close();
			characterSelectionPage.Close();
			equipPage.Open();

			break;
		}
	}

	public void ShowEquipment(EquipmentPage page = EquipmentPage.Menu)
	{
		gameObject.SetActive (true);

		DirectToPage (page);
	}

	public void CloseEquipment()
	{
		DirectToPage (EquipmentPage.Menu);

		MenuPage.Close();
		characterSelectionPage.Close ();
		equipPage.Close();

		gameObject.SetActive (false);

		if(Evt_EquipmentClose != null)
		{
			Evt_EquipmentClose();
		}
	}

	/// <summary>
	/// Changes to menu.
	/// </summary>
	public void ChangeToMenu()
	{
		DirectToPage (EquipmentPage.Menu);
	}

	/// <summary>
	/// Changes to character selection.
	/// </summary>
	public void ChangeToCharacterSelection()
	{
		DirectToPage (EquipmentPage.CharacterSelection);
	}

	/// <summary>
	/// Changes to equip.
	/// </summary>
	public void ChangeToEquip()
	{
		DirectToPage (EquipmentPage.Equip);
	}
}
