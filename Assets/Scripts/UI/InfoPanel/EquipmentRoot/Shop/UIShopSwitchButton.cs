using UnityEngine;
using System.Collections;

/// <summary>
/// UI shop switch button.
/// 
/// This class represent the button above shop and act as tab
/// Player can tab to switch shop page
/// </summary>
public class UIShopSwitchButton : MonoBehaviour 
{
	/// <summary>
	/// The select indicator.
	/// </summary>
	public GameObject selectIndicator;

	/// <summary>
	/// Select this tab.
	/// </summary>
	public void Select()
	{
		selectIndicator.SetActive (true);
	}

	/// <summary>
	/// Deselect this tab.
	/// </summary>
	public void Deselect()
	{
		selectIndicator.SetActive (false);
	}
}
