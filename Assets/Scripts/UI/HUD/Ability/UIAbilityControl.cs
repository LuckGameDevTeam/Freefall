using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SIS;
using System;

/// <summary>
/// UI ability control.
/// 
/// This class control each ability button in game
/// </summary>
public class UIAbilityControl : MonoBehaviour 
{
	public delegate void EventAbilityButtonPress(string itemId);
	/// <summary>
	/// Event when ability button press.
	/// As this event fired the item will take away from
	/// player immediately
	/// </summary>
	public EventAbilityButtonPress Evt_AbilityButtonPress;

	/// <summary>
	/// Ability buttons.
	/// </summary>
	public UIAbilityButton[] buttons;


	void OnEnable()
	{
		AdControl control = GameObject.FindObjectOfType (typeof(AdControl)) as AdControl;
		control.Evt_OnAdLoaded += OnAdLoaded;
		control.Evt_OnAdFailToLoad += OnAdFailToLoad;
		control.Evt_OnNoAdToLoad += OnNoAdToLoad;
	}

	void OnDisable()
	{
		AdControl control = GameObject.FindObjectOfType (typeof(AdControl)) as AdControl;

		if(control)
		{
			control.Evt_OnAdLoaded -= OnAdLoaded;
			control.Evt_OnAdFailToLoad -= OnAdFailToLoad;
			control.Evt_OnNoAdToLoad -= OnNoAdToLoad;
		}

	}


	// Use this for initialization
	void Start () 
	{
		InitAbility ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Inits the ability.
	/// </summary>
	void InitAbility()
	{
		//load player equipped item data
		PlayerEquippedItems pei = PlayerEquippedItems.Load ();

		List<string> equippedItems = pei.GetAllEquippedItems ();

		//setup eatch ability button
		for(int i=0; i<buttons.Length; i++)
		{
			if(i<equippedItems.Count)
			{
				buttons[i].ItemId = equippedItems[i];
				buttons[i].Evt_OnButtonPress += OnAbilityButtonPress;
			}
			else
			{
				buttons[i].ItemId = null;
			}
		}

		LockAbitliyPanel ();
	}

	/// <summary>
	/// Locks the abitliy panel so player can not interact with ability button.
	/// </summary>
	public void LockAbitliyPanel()
	{
		for(int i=0; i<buttons.Length; i++)
		{
			buttons[i].LockButton();
		}
	}

	/// <summary>
	/// UnLock abitliy panel so player can interact with ability button.
	/// </summary>
	public void UnLockAbitliyPanel()
	{
		for(int i=0; i<buttons.Length; i++)
		{
			buttons[i].UnLockButton();
		}
	}

	/// <summary>
	/// Reset ability control.
	/// 
	/// Initalize control again
	/// </summary>
	public void Reset()
	{
		for(int i=0; i<buttons.Length; i++)
		{
			buttons[i].Evt_OnButtonPress -= OnAbilityButtonPress;
		}

		InitAbility ();
	}

	/// <summary>
	/// Handle the ability button press event.
	/// </summary>
	/// <param name="button">Button.</param>
	/// <param name="itemId">Item identifier.</param>
	void OnAbilityButtonPress(UIAbilityButton button, string itemId)
	{
		if(Evt_AbilityButtonPress != null)
		{
			Evt_AbilityButtonPress(itemId);
		}

		//take one of this item away from player
		//StoreInventory.TakeItem (itemId, 1);
		DBManager.SetPlayerData (itemId, new SimpleJSON.JSONData (DBManager.GetPlayerData (itemId).AsInt - 1));
	}

	void OnAdLoaded(AdControl control, object sender, EventArgs args)
	{
		transform.localPosition = new Vector3 (transform.localPosition.x, 90f, transform.localPosition.z);
	}

	void OnAdFailToLoad(AdControl control, object sender, string message)
	{
		transform.localPosition = new Vector3 (transform.localPosition.x, 0f, transform.localPosition.z);
	}

	void OnNoAdToLoad(AdControl control)
	{
		transform.localPosition = new Vector3 (transform.localPosition.x, 0f, transform.localPosition.z);
	}
}
