using UnityEngine;
using System.Collections;

public enum PageType
{
	Store,
	Equip,
	Default
}

/// <summary>
/// UI info panel.
/// 
/// This class display info panel
/// </summary>
public class UIInfoPanel : MonoBehaviour 
{
	/// <summary>
	/// The equipment button.
	/// </summary>
	public UIButton equipmentButton;

	/// <summary>
	/// The equipment control.
	/// </summary>
	public UIEquipmentControl equipmentControl;

	/// <summary>
	/// The backdrop.
	/// </summary>
	public GameObject backdrop;

	void Awake()
	{
		//register event
		equipmentControl.Evt_EquipmentClose += EquipmentClose;
	}

	// Use this for initialization
	void Start () 
	{
		backdrop.SetActive (false);

		//Close equipment UI 
		equipmentControl.CloseEquipment ();

#if TestMode
		//lock equipment button in test mode
		equipmentButton.isEnabled = false;
#endif
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Opens the equipment alternative.
	/// 
	/// </summary>
	/// <param name="pType">P type.</param>
	public void OpenEquipmentAlternative(PageType pType = PageType.Default)
	{
		equipmentButton.isEnabled = false;

		switch(pType)
		{
		case PageType.Store:

			equipmentControl.ShowEquipment (EquipmentPage.CharacterSelection);

			break;

		case PageType.Equip:

			equipmentControl.ShowEquipment (EquipmentPage.Equip);

			break;

		case PageType.Default:

			equipmentControl.ShowEquipment ();

			break;
		}
	}

	/// <summary>
	/// Opens the equipment as default
	/// </summary>
	public void OpenEquipment()
	{
		backdrop.SetActive (true);

		equipmentButton.isEnabled = false;

		equipmentControl.ShowEquipment ();
	}

	/// <summary>
	/// Equipment close.
	/// </summary>
	void EquipmentClose()
	{
		backdrop.SetActive (false);

		equipmentButton.isEnabled = true;
	}
}
