/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SIS
{
    /// <summary>
    /// instantiates shop items in the scene,
    /// unlocks/locks and selects/deselects shop items based on previous purchases/selections.
    /// Called after database initialization
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        /// <summary>
        /// whether this script should print debug messages
        /// </summary>
        public bool debug;

        //static reference of this script
        private static ShopManager instance;
		
		//static indicator for allowing multi touch
		private static bool allowMultiTouch = false;

        /// <summary>
        /// window for showing feedback on IAPListener events to the user
        /// </summary>
        public GameObject errorWindow;
        /// <summary>
        /// text component of the errorWindow gameobject
        /// </summary>
        public UILabel message;

        /// <summary>
        /// store the relation between an IAP Group set in the IAP Settings Editor and its
        /// "parent" transform. This is because IAP Manager is a prefab that exists during
        /// scene changes, thus can't keep scene-specific data like transforms. 
        /// </summary>
        [HideInInspector]
        public List<Container> containers = new List<Container>();

        /// <summary>
        /// instantiated shop items, ordered by their ID
        /// </summary>
        public Dictionary<string, IAPItem> IAPItems = new Dictionary<string, IAPItem>();

        /// <summary>
        /// fired when selecting a shop item
        /// </summary>
        public static event Action<string> itemSelectedEvent;

        /// <summary>
        /// fired when deselecting a shop item
        /// </summary>
        public static event Action<string> itemDeselectedEvent;


        /// <summary>
        /// initialization called by IAP Manager in Awake()
        /// </summary>
        public void Init()
        {
            instance = this;
            Input.multiTouchEnabled = false;

            InitShop();
            SetItemState();
            UnlockItems();
        }


        /// <summary>
        /// if there is no IAP Manager in the scene,
        /// Shop Manager will try to instantiate the prefab
        /// </summary>
        void Awake()
        {
			allowMultiTouch = Input.multiTouchEnabled;
		
            if (!IAPManager.GetInstance())
            {
                if (debug) Debug.LogWarning("Could not find IAP Manager prefab. " +
                           "Have you placed it in the first scene of your app? Instantiating copy.");
                GameObject obj = Instantiate(Resources.Load("IAP Manager", typeof(GameObject))) as GameObject;
                //remove clone tag from its name. not necessary, but nice to have
                obj.name = obj.name.Replace("(Clone)", "");
            }
        }

		
		//revert multiTouch state
		void OnDestroy()
		{
			Input.multiTouchEnabled = allowMultiTouch;
		}
		

        /// <summary>
        /// returns a static reference to this script
        /// </summary>
        public static ShopManager GetInstance()
        {
            return instance;
        }


        //instantiates shop item prefabs
        void InitShop()
        {
            //reset
            IAPItems.Clear();

            //get list of all shop groups from IAPManager
            List<IAPGroup> list = IAPManager.GetIAPs();
            int index = 0;

            //loop over groups
            for (int i = 0; i < list.Count; i++)
            {
                //cache current group
                IAPGroup group = list[i];
                Container container = GetContainer(group.id);

                //skip group if prefab or parent wasn't set
                if (container == null || container.prefab == null || container.parent == null)
                {
                    if (debug)
                        Debug.LogWarning("Setting up Shop, but prefab or parent of Group: '"
                                         + group.name + "' isn't set. Skipping group.");
                    continue;
                }

                //loop over items
                for (int j = 0; j < group.items.Count; j++)
                {
                    //cache item
                    IAPObject obj = group.items[j];
                    //instantiate shop item in the scene and attach it to the defined parent transform
                    GameObject newItem = NGUITools.AddChild(container.parent.gameObject, container.prefab);
                    //rename item to force ordering as set in the IAP Settings editor
                    newItem.name = "IAPItem " + string.Format("{0:000}", index + j);
                    //get IAPItem component of the instantiated item
                    IAPItem item = newItem.GetComponent<IAPItem>();
                    if (item == null) continue;

                    //initialize and set up item properties based on the associated IAPObject
                    //they could get overwritten by online data later
                    item.Init(obj);

                    //add IAPItem to dictionary for later lookup
                    IAPItems.Add(obj.id, item);
                }

                index += group.items.Count;
                //reposition shop items correctly,
                //either in a table or grid layout
                UITable table = container.parent.GetComponent<UITable>();
                if (table) table.Reposition();
                UIGrid grid = container.parent.GetComponent<UIGrid>();
                if (grid) grid.Reposition();
            }
        }


        /// <summary>
        /// sets up shop items based on previous purchases, meaning we
        /// set them to 'purchased' thus not purchasable in the GUI.
        /// Also select the items that were selected by the player before
        /// </summary>
        public static void SetItemState()
        {
            //this method is based on data from the database,
            //so if we don't have a DBManager instance don't continue
            if (!DBManager.GetInstance()) return;

            //get array of purchased item ids, look them up in our
            //shop item dictionary and set them to purchased
            List<string> purchasedItems = DBManager.GetAllPurchased();
            for (int i = 0; i < purchasedItems.Count; i++)
                if (instance.IAPItems.ContainsKey(purchasedItems[i]))
                    instance.IAPItems[purchasedItems[i]].Purchased(true);

            //get dictionary of selected item ids, look them up in our
            //shop item dictionary and set the checkbox component to selected
            Dictionary<string, List<string>> selectedItems = DBManager.GetAllSelected();
            foreach (string key in selectedItems.Keys)
            {
                for (int i = 0; i < selectedItems[key].Count; i++)
                    if (instance.IAPItems.ContainsKey(selectedItems[key][i]))
                        instance.IAPItems[selectedItems[key][i]].selButton.GetComponent<UIToggle>().startsActive = true;
            }
        }


        /// <summary>
        /// unlocks items if the requirement for them has been met. You can
        /// call this method at runtime whenever the player made some
        /// progress, to ensure your shop items reflect the current state
        /// </summary>
        public static void UnlockItems()
        {
            //this method is based on data from the database,
            //so if we don't have a DBManager instance don't continue
            if (!DBManager.GetInstance()) return;

            //get list of all shop groups from IAPManager
            List<IAPGroup> list = IAPManager.GetIAPs();

            //loop over groups
            for (int i = 0; i < list.Count; i++)
            {
                //cache current group
                IAPGroup group = list[i];

                //loop over items
                for (int j = 0; j < group.items.Count; j++)
                {
                    //cache IAP object
                    IAPObject obj = group.items[j];
                    if (obj.req == null) continue;
                    //cache reference to IAP item instance
                    IAPItem item = GetIAPItem(obj.id);
                    if (item == null) continue;
                    //check if a requirement is set up for this item,
                    //then unlock if the requirement has been met
                    if (!string.IsNullOrEmpty(obj.req.entry) &&
                        DBManager.isRequirementMet(obj.req))
                    {
                        if (instance.debug) Debug.Log("requirement met for: " + obj.id);
                        item.Unlock();
                    }
                }
            }
        }


        /// <summary>
        /// method for overwriting shop item's properties with online IAP data
        /// from Google's or Apple's servers (those in the developer console).
        /// When we receive the online item list of IAPProducts from IAPManager,
        /// we loop over our products and check if 'fetch' was checked in the
        /// IAP Settings editor, then simply reinitialize the items with new info
        /// </summary>
        public static void OverwriteWithFetch(List<IAPArticle> products)
        {
            for (int i = 0; i < products.Count; i++)
            {
                string id = IAPManager.GetIAPIdentifier(products[i].id);
                IAPObject item = IAPManager.GetIAPObject(id);
                if (item != null && item.fetch && instance.IAPItems.ContainsKey(id))
                    instance.IAPItems[id].Init(products[i]);
            }
        }


        /// <summary>
        /// sets an item to 'selected' in the database
        /// </summary>
        public static void SetToSelected(IAPItem item)
        {
            //check if the item allows for single or multi selection,
            //this depends on whether the item has a deselect button
            bool single = item.deselButton ? false : true;
            //pass arguments to DBManager and invoke select event
            bool changed = DBManager.SetToSelected(item.productId, single);
            if(changed && itemSelectedEvent != null)
                itemSelectedEvent(item.productId);
        }


        /// <summary>
        /// sets an item to 'deselected' in the database
        /// </summary>
        public static void SetToDeselected(IAPItem item)
        {
            //pass argument to DBManager and invoke deselect event
            DBManager.SetToDeselected(item.productId);
            itemDeselectedEvent(item.productId);
        }


        //show feedback/error window with text received through an event:
        //this gets called in IAPListener's HandleSuccessfulPurchase method with some feedback,
        //or automatically with the error when a purchase failed 
        public static void ShowMessage(string text)
        {
			if (!instance.errorWindow) return;

            if(instance.message) instance.message.text = text;
            NGUITools.SetActive(instance.errorWindow, true);
        }


        /// <summary>
        /// returns IAPItem shop item reference
        /// </summary>
        public static IAPItem GetIAPItem(string id)
        {
            if (instance.IAPItems.ContainsKey(id))
                return instance.IAPItems[id];
            else
                return null;
        }


        /// <summary>
        /// returns container for a specific group id
        /// </summary>
        public Container GetContainer(string id)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                if (containers[i].id.Equals(id))
                    return containers[i];
            }
            return null;
        }
    }


    /// <summary>
    /// correlation between IAP group
    /// and scene-specific properties
    /// </summary>
    [System.Serializable]
    public class Container
    {
        public string id;
        public GameObject prefab;
        public Transform parent;
    }
}