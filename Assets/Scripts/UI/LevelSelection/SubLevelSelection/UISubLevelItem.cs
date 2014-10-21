using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UI sub level item.
/// 
/// This class display each sub level item
/// </summary>
public class UISubLevelItem : MonoBehaviour 
{
	/// <summary>
	/// The sub level index start with 1.
	/// </summary>
	public int subLevel = 1;

	/// <summary>
	/// The lock indicator.
	/// </summary>
	public GameObject lockIndicator;

	/// <summary>
	/// The icon sprite.
	/// </summary>
	public UISprite iconSprite;

	/// <summary>
	/// The click button.
	/// </summary>
	public UIButton clickButton;

	/// <summary>
	/// The click button background.
	/// </summary>
	public UISprite clickButtonBackground;

	/// <summary>
	/// The star control.
	/// </summary>
	public UIStarControl starControl;

	/// <summary>
	/// The title label.
	/// </summary>
	public UILabel titleLabel;

	/// <summary>
	/// The main level this sub level belong to.
	/// </summary>
	private int mainLevel = 1;

	/// <summary>
	/// The sub level string.
	/// 
	/// will be like "Level1-3";
	/// </summary>
	private string subLevelString;

	/// <summary>
	/// Reference to UISubLevelSelectionControl
	/// </summary>
	private UISubLevelSelectionControl subLevelControl;

	void Awake()
	{
		//find UISubLevelSelectionControl
		subLevelControl = NGUITools.FindInParents<UISubLevelSelectionControl> (gameObject);

		//register event
		subLevelControl.Evt_UpdateUIWithMainLevel += UpdateSubLevelItem;
	}

	void OnDisable()
	{
		Deselect ();
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Select.
	/// </summary>
	public void Select()
	{
		//deselect all other sub levels
		List<UISubLevelItem> subLevelItems = GetItems<UISubLevelItem> ();

		for(int i=0; i<subLevelItems.Count; i++)
		{
			if(subLevelItems[i].subLevel != subLevel)
			{
				subLevelItems[i].Deselect();
			}

		}

		subLevelControl.selectedSubLevel = subLevel;

		SetIconSelect ();
	}

	/// <summary>
	/// Deselect.
	/// </summary>
	public void Deselect()
	{
		SetIconDeSelect ();
	}

	/// <summary>
	/// Sets the icon select.
	/// </summary>
	private void SetIconSelect()
	{
		string sName = SubLevelData.levelPrefix+mainLevel+"-sub-select";

		iconSprite.spriteName = sName;

		clickButtonBackground.spriteName = sName;

	}

	/// <summary>
	/// Sets the icon de select.
	/// </summary>
	private void SetIconDeSelect()
	{
		string sName = SubLevelData.levelPrefix+mainLevel+"-sub-unselect";

		iconSprite.spriteName = sName;

		clickButtonBackground.spriteName = sName;
	}

	/// <summary>
	/// Helper fucntion
	/// Get all same level items
	/// </summary>
	/// <returns>The items with type or null it there is an error.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	protected List<T> GetItems<T>() where T : UnityEngine.Component
	{
		List<T> retItems = new List<T>();
		
		for(int i=0; i<transform.parent.childCount; i++)
		{
			Transform child = transform.parent.GetChild(i);
			
			T item = child.GetComponent<T>();
			
			if(item != null)
			{
				retItems.Add(item);
			}
		}
		
		return retItems;
	}
	
	/// <summary>
	/// Event updates the sub level item.
	/// </summary>
	/// <param name="control">Control.</param>
	/// <param name="currentMainLevel">Current main level.</param>
	void UpdateSubLevelItem(UISubLevelSelectionControl control, int currentMainLevel)
	{
		//set main level this sub level belong to
		mainLevel = currentMainLevel;

		//update sub level string
		subLevelString = SubLevelData.GetLevelTitle (mainLevel, subLevel);

		//update title label
		titleLabel.text = mainLevel + "-" + subLevel;

		//load sub level data
		SubLevelData slData = SubLevelData.Load ();

		//check if sub level unlocked 
		if(slData.IsSubLevelUnlocked(mainLevel, subLevel))
		{
			lockIndicator.SetActive(false);

			//enable button icon
			clickButton.isEnabled = true;
			SetIconDeSelect();

			starControl.UpdateStarsWithScroe(slData.GetSubLevelScore(currentMainLevel, subLevel));
		}
		else//not unlocked
		{
			lockIndicator.SetActive(true);

			//disable button icon
			clickButton.isEnabled = false;
			SetIconDeSelect();

			starControl.UpdateStarsWithScroe(0);
		}
	}
}
