using UnityEngine;
using System.Collections;

/// <summary>
/// UI equip root.
/// 
/// This class is root of equip page
/// </summary>
public class UIEquipRoot : UIEquipmentRoot 
{
	/// <summary>
	/// Reference to purchase control.
	/// </summary>
	public UIPurchaseControl purchaseControl;

	/// <summary>
	/// Reference to  alert control.
	/// </summary>
	public UIAlertControl alertControl;

	void Awake()
	{
		if(purchaseControl == null)
		{
			Debug.LogError("You must assigned UIPurchaseControl to "+gameObject.name);
		}

		if(alertControl == null)
		{
			Debug.LogError("You must assigned UIAlertControl to "+gameObject.name);
		}
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
