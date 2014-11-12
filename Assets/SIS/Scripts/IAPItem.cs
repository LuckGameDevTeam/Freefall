/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System.Collections;

namespace SIS
{
    /// <summary>
    /// shop item properties, this class basically stores all
    /// necessary variables for visualizing a product in your shop GUI 
    /// </summary>
    public class IAPItem : MonoBehaviour
    {
        /// <summary>
        /// ID of the product
        /// </summary>
        [HideInInspector]
        public string productId;

        /// <summary>
        /// product name
        /// </summary>
        public UILabel title;

        /// <summary>
        /// product description
        /// </summary>
        public UILabel description;

        /// <summary>
        /// array of price labels, as there could be more
        /// than one currency for virtual game purchases
        /// </summary>
        public UILabel[] price;

        /// <summary>
        /// icon sprite variable
        /// </summary>
        public UISprite icon;

        /// <summary>
        /// boolean for setting all labels to uppercase
        /// </summary>
        public bool uppercase = false;

        /// <summary>
        /// buy button that invokes the actual purchase
        /// </summary>
        public GameObject buyButton;

        /// <summary>
        /// buy trigger, used for making the buy button visible
        /// (optional - could be used for 'double tap to purchase')
        /// </summary>
        public GameObject buyTrigger;

        /// <summary>
        /// label that displays text while this item is locked
        /// </summary>
        public UILabel lockedLabel;

        /// <summary>
        /// widgets that will be de-activated when unlocking this item
        /// </summary>
        public GameObject hideOnUnlock;

        /// <summary>
        /// widgets that will be activated when unlocking this item
        /// </summary>
        public GameObject[] showOnUnlock;

        /// <summary>
        /// additional widgets that will be activated on sold items
        /// </summary>
        public GameObject sold;

        /// <summary>
        /// additional widgets that will be activated on selected items
        /// </summary>
        public GameObject selected;

        /// <summary>
        /// button for selecting this item
        /// </summary>
        public GameObject selButton;

        /// <summary>
        /// button for deselecting this item
        /// </summary>
        public GameObject deselButton;

        //selection checkbox, cached for triggering other checkboxes
        //in the same group on selection/deselection
        private UIToggle selCheck;

        /// <summary>
        /// type of in app purchase for this item
        /// </summary>
        [HideInInspector]
        public IAPType type = IAPType.consumable;


        //set up delegates and selection checkboxes
        void Start()
        {
            //forward buyButton click event to Purchase()
            UIEventListener.Get(buyButton).onClick += Purchase;

            //if a selection of this item is possible
            if (selButton)
            {
                //get checkbox component
                selCheck = selButton.GetComponent<UIToggle>();
                //if it has one and if it has a deselect button
                if (selCheck && deselButton)
                {
                    //forward deselButton click event to Deselect(),
                    //set up checkbox for multi selections
                    UIEventListener.Get(deselButton).onClick += Deselect;
                    selCheck.optionCanBeNone = true;
                }
            }
        }


        //if we have a possible purchase confirmation set up or pending,
        //hide the buy button when disabling this item to reset it
        void OnDisable()
        {
            if (buyTrigger)
                ConfirmPurchase(false);
        }


        /// <summary>
        /// initialize virtual or real item properties
        /// based on IAPObject info set in IAP Settings editor.
        /// Called by ShopManager
        /// </summary>
        public void Init(IAPObject prod)
        {
            //cache
            type = prod.type;
            string name = prod.title;
            string descr = prod.description.Replace("\\n", "\n");
            string lockText = prod.req.labelText;

            //store the item id for later purposes
            productId = prod.id;
            //set icon to the matching sprite in the atlas used
            if (icon) icon.spriteName = name;

            //when 'uppercase' has been checked,
            //convert title and description text to uppercase,
            //otherwise just keep and set them as they are
            if (uppercase)
            {
                name = name.ToUpper();
                descr = descr.ToUpper();
                lockText = lockText.ToUpper();
            }

            if (title) title.text = name;
            if (description) description.text = descr;

            if (type == IAPType.consumable || type == IAPType.nonConsumable
                || type == IAPType.subscription)
            {
                //purchases for real money have only one price value,
                //so we just use the first entry of the price label array
                if (price.Length > 0) price[0].text = prod.realPrice;
            }
            else if (prod.virtualPrice.Count > 0)
            {
                //purchases for virtual currency could have more than one price value,
                //so we loop over all entries and set the corresponding price label
                for (int i = 0; i < price.Length; i++)
                    if (price[i]) price[i].text = prod.virtualPrice[i].amount.ToString();
            }

            //set locked label text in case a requirement has been set
            if (lockedLabel && !string.IsNullOrEmpty(prod.req.entry)
                && !string.IsNullOrEmpty(prod.req.labelText))
                lockedLabel.text = lockText;
        }


        /// <summary>
        /// overwrite real money item properties with online data
        /// from Google or Apple, based on our IAPArticle wrapper
        /// Called by ShopManager
        /// </summary>
        public void Init(IAPArticle prod)
        {
            //cache
            string name = prod.title;
            string descr = prod.description.Replace("\\n", "\n");
            //normally, the online item name received from Google or Apple
            //has the application name attached, so we exclude that here
            int cap = name.IndexOf("(");
            if (cap > 0)
                name = name.Substring(0, cap - 1);

            //when 'uppercase' has been checked,
            //convert title and description text to uppercase,
            //otherwise just keep and set them as they are
            if (uppercase)
            {
                name = name.ToUpper();
                descr = descr.ToUpper();
            }

            if (title) title.text = name;
            if (description) description.text = descr;

            //purchases for real money have only one price value,
            //so we just use the first entry of the price label array
            if (price.Length > 0) price[0].text = prod.price;
        }


        /// <summary>
        /// unlocks this item by hiding the 'locked' gameobject
        /// and setting up the default state. Called by ShopManager
        /// </summary>
        public void Unlock()
        {
            if (!hideOnUnlock || !hideOnUnlock.activeSelf)
                return;

            NGUITools.SetActive(hideOnUnlock, false);
            if(lockedLabel) NGUITools.SetActive(lockedLabel.gameObject, false);

            for (int i = 0; i < showOnUnlock.Length; i++)
                NGUITools.SetActive(showOnUnlock[i], true);
        }


        /// <summary>
        /// show the buy button based on the bool passed in.
        /// This simulates 'double tap to purchase' behavior,
        /// and only works when setting a buyTrigger button
        /// </summary>
        public void ConfirmPurchase(bool selected)
        {
            if (!selected)
                NGUITools.SetActive(buyButton, false);
        }


        //when the buy button has been clicked, here we try to purchase this item
        //maps to the corresponding purchase methods of IAPManager
        //only works on an actual mobile device
        void Purchase(GameObject button)
        {
            #if UNITY_EDITOR
            if (type == IAPType.consumable || type == IAPType.nonConsumable
               || type == IAPType.subscription)
            {
                Debug.Log("Calling purchase: " + this.productId
                          + ".\nYou are not on a mobile device, nothing will happen.");
            }
            #endif

            //differ between IAP type
            switch (type)
            {
                case IAPType.consumable:
                    IAPManager.PurchaseConsumableProduct(this.productId);
                    break;
                case IAPType.nonConsumable:
                    IAPManager.PurchaseNonconsumableProduct(this.productId);
                    break;
                case IAPType.subscription:
                    IAPManager.PurchaseSubscription(this.productId);
                    break;
                case IAPType.consumableVirtual:
                    IAPManager.PurchaseConsumableVirtualProduct(this.productId);
                    break;
                case IAPType.nonConsumableVirtual:
                    IAPManager.PurchaseNonconsumableVirtualProduct(this.productId);
                    break;
            }

            //hide buy button once a purchase was made
            //only when an additional buy trigger was set
            if (buyTrigger)
                ConfirmPurchase(false);
        }


        /// <summary>
        /// set this item to 'purchased' state (true),
        /// or unpurchased state (false) for fake purchases
        /// </summary>
        public void Purchased(bool state)
        {
            //in case we restored an old purchase on a
            //locked item, we have to unlock it first
            Unlock();

            //back to unpurchased state, deselect
            if (!state) Deselect(null);

            //activate the sold gameobject
            if (sold)
                NGUITools.SetActive(sold, state);
            //activate the select button
            if (selButton)
                NGUITools.SetActive(selButton, state);

            //hide both buy trigger and buy button,
            //for ignoring further purchase clicks.
            //but don't do that for subscriptions,
            //so that the user could easily renew it
            if (type == IAPType.subscription) return;

            if (buyTrigger)
                NGUITools.SetActive(buyTrigger, !state);
            NGUITools.SetActive(buyButton, !state);
        }


        /// <summary>
        /// handles selection state for this item, but this method
        /// gets called on other radio buttons within the same group too.
        /// Called by selButton's UIToggle component
        /// </summary>
        public void IsSelected(bool thisSelect)
        {
            //if this object has been selected
            if (thisSelect)
            {
                //tell our ShopManager to change the database entry
                ShopManager.SetToSelected(this);

                //if we have a deselect button or
                //a 'selected' gameobject, show them
                //and hide the select button for ignoring further selections
                if (deselButton)
                {
                    NGUITools.SetActive(selButton, false);
                    NGUITools.SetActive(deselButton, true);
                }
                else
                {
                    UIWidget[] widgets = selButton.GetComponentsInChildren<UIWidget>(true);
                    for (int i = 0; i < widgets.Length; i++)
                        widgets[i].enabled = false;
                }

                if (selected)
                    NGUITools.SetActive(selected, true);
            }
            else
            {
                //if an other object has been selected, show the
                //select button for this item and hide the 'selected' state
                if (!deselButton)
                {
                    UIWidget[] widgets = selButton.GetComponentsInChildren<UIWidget>(true);
                    for (int i = 0; i < widgets.Length; i++)
                        widgets[i].enabled = true;
                }

                if (selected)
                    NGUITools.SetActive(selected, false);
            }
        }


        //called when deselecting this item via deselButton
        void Deselect(GameObject button)
        {
            //hide the deselect button and 'selected' state
            if (deselButton)
                NGUITools.SetActive(deselButton, false);
            if (selected)
                NGUITools.SetActive(selected, false);

            //tell our checkbox component that this object isn't checked
            if (selCheck) selCheck.value = false;
            //tell our ShopManager to change the database entry accordingly
            ShopManager.SetToDeselected(this);
            //re-show the select button
            if (selButton)
                NGUITools.SetActive(selButton, true);
        }
    }
}
