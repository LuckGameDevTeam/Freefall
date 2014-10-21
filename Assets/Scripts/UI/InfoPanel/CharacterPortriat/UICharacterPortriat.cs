using UnityEngine;
using System.Collections;

/// <summary>
/// UI character portriat.
/// 
/// This class control character portrait in info panel
/// </summary>
public class UICharacterPortriat : MonoBehaviour 
{
	/// <summary>
	/// The character portriat.
	/// </summary>
	public UISprite characterPortriat;

	/// <summary>
	/// Reference to info panel.
	/// </summary>
	private UIInfoPanel infoPanel;

	void Awake()
	{
		//find info panel
		infoPanel = NGUITools.FindInParents<UIInfoPanel> (gameObject);

		//register event for character selected
		infoPanel.equipmentControl.GetStorePage.Evt_OnCharacterSelected += OnCharacterSelected;
	}

	// Use this for initialization
	void Start () 
	{
		ConfigurePortriat ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Configures the portriat.
	/// </summary>
	void ConfigurePortriat()
	{
		PlayerCharacter pc = PlayerCharacter.Load ();
		
		string imageName = pc.characterName+"_Head";

		//set portrait sprite name
		characterPortriat.spriteName = imageName;
	}

	/// <summary>
	/// Handle the character selected event.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	void OnCharacterSelected(string itemId)
	{
		ConfigurePortriat ();
	}
}
