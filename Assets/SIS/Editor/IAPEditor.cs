/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SIS
{
    /// <summary>
    /// IAP Settings editor.
    /// The one-stop solution for managing cross-platform IAP data.
    /// Found under Window > Simple IAP System > IAP Settings
    /// </summary>
    public class IAPEditor : EditorWindow
    {
        //shop reference
        [SerializeField]
        ShopManager shop;
        //manager reference
        [SerializeField]
        IAPManager script;
        //prefab object
        [SerializeField]
        private static Object IAPPrefab;
        //window reference
        private static IAPEditor iapEditor;
        //requirement wizard reference
        private static RequirementEditor reqEditor;

        //first toolbar for displaying IAP types
        int toolbar = 0;
        string[] toolbarStrings = new string[] { "In App Purchases", "In Game Content" };
        //available currency names for selection
        string[] currencyNames;
        //currently selected currency index
        int currencyIndex = -1;
        //currently selected platform on Android
        int androidPlatform = 0;
        //available store platforms on Android
        string[] androidPlatformStrings = new string[] { IAPPlatform.GooglePlay.ToString(),
                                                         IAPPlatform.Amazon.ToString() };

        //inspector scrollbar x/y position for each tab
        Vector2 scrollPosIAP;
        Vector2 scrollPosIGC;

        //possible criteria for ordering product ids
        //changing the selected order type will cause a re-order
        private enum OrderType
        {
            none,
            priceAsc,
            priceDesc,
            titleAsc,
            titleDesc,
        }
        private OrderType orderType = OrderType.none;


        //add menu named "IAP Settings" to the window menu
        [MenuItem("Window/Simple IAP System/IAP Settings")]
        static void Init()
        {
            //get existing open window or if none, make a new one
            iapEditor = (IAPEditor)EditorWindow.GetWindowWithRect(typeof(IAPEditor), new Rect(0, 0, 800, 400), false, "IAP Settings");
            //automatically repaint whenever the scene has changed (for caution)
            iapEditor.autoRepaintOnSceneChange = true;
        }


        //when the window gets opened
        void OnEnable()
        {
            //get reference to the shop and cache it
            shop = GameObject.FindObjectOfType(typeof(ShopManager)) as ShopManager;

            script = FindIAPManager();

            //could not get prefab, non-existent?
            if (script == null)
                return;

            if (shop)
                RemoveContainerConnections();

            //set current currency index from -1 to first one,
            //if currencies were specified
            if (script.currency.Count > 0)
                currencyIndex = 0;

            //get current platform variable and set selected index
            string currentPlatform = script.androidPlatform.ToString();
            for (int i = 0; i < androidPlatformStrings.Length; i++)
            {
                if (androidPlatformStrings[i] == currentPlatform)
                {
                    androidPlatform = i;
                    break;
                }
            }
        }


        //locate IAP Manager prefab in the project
        public static IAPManager FindIAPManager()
        {
            GameObject obj = Resources.Load("IAP Manager") as GameObject;

            if (obj != null && PrefabUtility.GetPrefabType(obj) == PrefabType.Prefab)
            {
                //try to get IAP Manager component and return it
                IAPManager iap = obj.GetComponent(typeof(IAPManager)) as IAPManager;
                if (iap != null)
                {
                    IAPPrefab = obj;
                    return iap;
                }
            }

            return null;
        }


        //remove empty IAPGroup references in the scene
        void RemoveContainerConnections()
        {
            //get all container objects from the Shop Manager,
            //then populate a list with all IAPGroups
            List<Container> containers = new List<Container>();
            containers.AddRange(shop.containers);
            List<IAPGroup> allGroups = new List<IAPGroup>();
            allGroups.AddRange(script.IAPs);
            allGroups.AddRange(script.IGCs);

            //loop over lists and compare them
            for (int i = 0; i < containers.Count; i++)
            {
                //if we found an IAPGroup in the Shop Manager component
                //that does not exist anymore, remove it from the scene containers
                IAPGroup g = allGroups.Find(x => x.id == containers[i].id);
                if (g == null)
                {
                    shop.containers.Remove(shop.containers.Find(x => x.id == containers[i].id));
                }
            }
            containers.Clear();
        }


        //close windows and save changes on exit
        void OnDestroy()
        {
            if (reqEditor) reqEditor.Close();
            SavePrefab();
        }


        void OnGUI()
        {
            if (script == null)
            {
                EditorGUILayout.LabelField("Couldn't find an IAP Manager prefab in the project! " +
                                 "Is it located in the Resources folder?");
                return;
            }

            //set the targeted script modified by the GUI for handling undo
            List<Object> objs = new List<Object>() { script };
            if (shop != null) objs.Add(shop);
            Object[] undo = objs.ToArray();
			
            #if UNITY_4_2
                Undo.SetSnapshotTarget(undo, "Changed Settings");
                //save the current state of all objects set with SetSnapshotTarget to internal snapshot
                Undo.CreateSnapshot();
            #else
				Undo.RecordObjects(undo, "ChangedSettings");
            #endif
                
            //display toolbar at the top, followed by a horizontal line
            toolbar = GUILayout.Toolbar(toolbar, toolbarStrings);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

            //handle toolbar selection
            switch (toolbar)
            {
                //first tab selected
                case 0:
                    DrawIAP(script.IAPs);
                    break;
                //second tab selected
                case 1:
                    DrawIGC(script.IGCs);
                    break;
            }

            //track change as well as undo
            TrackChange();
        }


        //draws the in app purchase editor
        //for a specific OS
        void DrawIAP(List<IAPGroup> list)
        {
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.yellow;

            //draw platform selection on Android, re-save prefab if changes occur
            androidPlatform = EditorGUILayout.Popup("Android Platform:", androidPlatform, androidPlatformStrings, GUILayout.Width(395));
            if (script.androidPlatform.ToString() != androidPlatformStrings[androidPlatform])
            {
                script.androidPlatform = (IAPPlatform)System.Enum.Parse(typeof(IAPPlatform), androidPlatformStrings[androidPlatform]);
                SavePrefab();
            }

            //draw yellow button for adding a new IAP group
            if (GUILayout.Button("Add new Group"))
            {
                //create new group, give it a generic name based on
                //the current unix time and add it to the list of groups
                IAPGroup newGroup = new IAPGroup();
                string timestamp = GenerateUnixTime();
                newGroup.name = "Grp " + timestamp;
                newGroup.id = timestamp;
                list.Add(newGroup);
                return;
            }

            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            //begin a scrolling view inside this tab, pass in current Vector2 scroll position 
            scrollPosIAP = EditorGUILayout.BeginScrollView(scrollPosIAP, GUILayout.Height(350));
            GUILayout.Space(20);

            //loop over IAP groups for this OS
            for (int i = 0; i < list.Count; i++)
            {
                //cache group
                IAPGroup group = list[i];
                //version 1.2 backwards compatibility fix (empty IAPGroup ids)
                if (string.IsNullOrEmpty(group.id))
                    group.id = GenerateUnixTime();

                //populate shop container variables if ShopManager is present
                Container shopGroup = null;
                if (shop)
                {
                    shopGroup = shop.GetContainer(group.id);
                    if (shopGroup == null)
                    {
                        shopGroup = new Container();
                        shopGroup.id = group.id;
                        shop.containers.Add(shopGroup);
                    }
                }

                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.yellow;
                //button for adding a new IAPObject (product) to this group
                if (GUILayout.Button("New Object", GUILayout.Width(120)))
                {
                    IAPObject newObj = new IAPObject();
                    //add platform dependent ids to the local list
                    int platforms = System.Enum.GetValues(typeof(IAPPlatform)).Length;
                    for (int j = 0; j < platforms; j++)
                        newObj.localId.Add(new IAPIdentifier());

                    group.items.Add(newObj);
                    break;
                }

                //draw group properties
                GUI.backgroundColor = Color.white;
                EditorGUILayout.LabelField("Group:", GUILayout.Width(45));
                group.name = EditorGUILayout.TextField(group.name, GUILayout.Width(90));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Sort:", GUILayout.Width(35));
                orderType = (OrderType)EditorGUILayout.EnumPopup(orderType, GUILayout.Width(60));
                GUILayout.Space(10);

                if (!shop)
                    EditorGUILayout.LabelField("No ShopManager prefab found in this scene!", GUILayout.Width(300));
                else
                {
                    EditorGUILayout.LabelField("Prefab:", GUILayout.Width(45));
                    shopGroup.prefab = (GameObject)EditorGUILayout.ObjectField(shopGroup.prefab, typeof(GameObject), false, GUILayout.Width(100));
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField("Parent:", GUILayout.Width(45));
                    shopGroup.parent = (Transform)EditorGUILayout.ObjectField(shopGroup.parent, typeof(Transform), true, GUILayout.Width(100));
                }
                
                GUILayout.FlexibleSpace();
                //check for order type and, if it
                //isn't equal to 'none', start ordering
                if (orderType != OrderType.none)
                {
                    group.items = orderProducts(group.items);
                    break;
                }

                //button width for up & down buttons
                //these should always be at the same width, so if there's
                //only one button (e.g. if there's only one group),
                //the width must be extended
                int groupUpWidth = 22;
                int groupDownWidth = 22;
                if (i == 0) groupDownWidth = 48;
                if (i == list.Count - 1) groupUpWidth = 48;

                //draw up & down buttons for re-ordering groups
                //this will simply switch references in the list
                //hotControl and keyboardControl unsets current mouse focus
                if (i > 0 && GUILayout.Button("▲", GUILayout.Width(groupUpWidth)))
                {
                    list[i] = list[i - 1];
                    list[i - 1] = group;
                    EditorGUIUtility.hotControl = 0;
                    EditorGUIUtility.keyboardControl = 0;
                }
                if (i < list.Count - 1 && GUILayout.Button("▼", GUILayout.Width(groupDownWidth)))
                {
                    list[i] = list[i + 1];
                    list[i + 1] = group;
                    EditorGUIUtility.hotControl = 0;
                    EditorGUIUtility.keyboardControl = 0;
                }

                //button for removing a group including items
                GUI.backgroundColor = Color.gray;
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    if(shop) shop.containers.Remove(shopGroup);
                    list.RemoveAt(i);
                    break;
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

                //draw header information for each item property
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(105);
                EditorGUILayout.LabelField("ID:", GUILayout.Width(85));
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Type:", GUILayout.Width(45));
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Fetch:", GUILayout.Width(60));
                GUILayout.Space(20);
                int spaceTitleToDescription = 110;
                if (group.items.Count == 1) spaceTitleToDescription = 155;
                EditorGUILayout.LabelField("Title:", GUILayout.Width(spaceTitleToDescription));
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Description:", GUILayout.Width(105));
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Price:", GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();

                //loop over items in this group
                for (int j = 0; j < group.items.Count; j++)
                {
                    //cache item reference
                    IAPObject obj = group.items[j];
                    EditorGUILayout.BeginHorizontal();

                    //version < 2.1 compatibility (add per-platform ids)
                    int platforms = System.Enum.GetValues(typeof(IAPPlatform)).Length;
                    for (int k = obj.localId.Count; k < platforms; k++)
                        obj.localId.Add(new IAPIdentifier());

                    obj.platformFoldout = EditorGUILayout.Foldout(obj.platformFoldout, "");

                    //draw IAPObject (item/product) properties
                    obj.id = EditorGUILayout.TextField(obj.id, GUILayout.Width(120));
                    obj.type = (IAPType)EditorGUILayout.EnumPopup(obj.type, GUILayout.Width(110));
                    //don't allow virtual IAP types, they must be consumable/non consumable/subscription
                    if (obj.type == IAPType.consumableVirtual) obj.type = IAPType.consumable;
                    else if (obj.type == IAPType.nonConsumableVirtual) obj.type = IAPType.nonConsumable;
                    obj.fetch = EditorGUILayout.Toggle(obj.fetch, GUILayout.Width(20));
                    obj.title = EditorGUILayout.TextField(obj.title);
                    obj.description = EditorGUILayout.TextField(obj.description);
                    obj.realPrice = EditorGUILayout.TextField(obj.realPrice, GUILayout.Width(80));

                    //button for adding a requirement to this item
                    if (!string.IsNullOrEmpty(obj.req.entry))
                        GUI.backgroundColor = Color.yellow;
                    if (GUILayout.Button("R", GUILayout.Width(20)))
                    {
                        reqEditor = (RequirementEditor)EditorWindow.GetWindowWithRect(typeof(RequirementEditor), new Rect(0, 0, 300, 150), false, "Requirement");
                        reqEditor.obj = obj;
                    }

                    GUI.backgroundColor = Color.white;
                    //do the same here as with the group up & down buttons
                    //(see above)
                    int buttonUpWidth = 22;
                    int buttonDownWidth = 22;
                    if (j == 0) buttonDownWidth = 48;
                    if (j == group.items.Count - 1) buttonUpWidth = 48;

                    //draw up & down buttons for re-ordering items in a group
                    //this will simply switch references in the list
                    if (j > 0 && GUILayout.Button("▲", GUILayout.Width(buttonUpWidth)))
                    {
                        group.items[j] = group.items[j - 1];
                        group.items[j - 1] = obj;
                        EditorGUIUtility.hotControl = 0;
                        EditorGUIUtility.keyboardControl = 0;
                    }
                    if (j < group.items.Count - 1 && GUILayout.Button("▼", GUILayout.Width(buttonDownWidth)))
                    {
                        group.items[j] = group.items[j + 1];
                        group.items[j + 1] = obj;
                        EditorGUIUtility.hotControl = 0;
                        EditorGUIUtility.keyboardControl = 0;
                    }

                    //button for removing an item of the group
                    GUI.backgroundColor = Color.gray;
                    if (GUILayout.Button("X"))
                    {
                        group.items.RemoveAt(j);
                        break;
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndHorizontal();

                    //draw platform override foldout
                    if (obj.platformFoldout)
                    {
                        EditorGUILayout.LabelField("Platform Overrides");
                        for (int k = 0; k < obj.localId.Count; k++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(40);
                            obj.localId[k].overridden = EditorGUILayout.BeginToggleGroup("", obj.localId[k].overridden);
                            EditorGUILayout.BeginHorizontal();
                            obj.localId[k].id = EditorGUILayout.TextField(obj.localId[k].id, GUILayout.Width(120));
                            EditorGUILayout.LabelField(((IAPPlatform)k).ToString());
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndToggleGroup();
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
                GUILayout.Space(30);
            }

            //ends the scrollview defined above
            EditorGUILayout.EndScrollView();
        }


        //draws the in game content editor
        void DrawIGC(List<IAPGroup> list)
        {
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.yellow;

            //draw currencies up to a maximum of 7
            //(there is no limitation, but 7 currencies do fit in the window nicely,
            //and there really shouldnt be a reason to have 7+ different currencies)
            if (script.currency.Count < 7)
            {
                //button for adding a new currency
                if (GUILayout.Button("Add Currency"))
                {
                    //switch current currency selection to the first entry
                    currencyIndex = 0;
                    //create new currency, then loop over items
                    //and add a new currency slot for each of them 
                    script.currency.Add(new IAPCurrency());
                    for (int i = 0; i < list.Count; i++)
                        for (int j = 0; j < list[i].items.Count; j++)
                            list[i].items[j].virtualPrice.Add(new IAPCurrency());
                    return;
                }
            }
            else
            {
                //for more than 7 currencies,
                //we show a transparent button with no functionality
                GUI.backgroundColor = new Color(1, 0.9f, 0, 0.4f);
                if (GUILayout.Button("Add Currency"))
                { }
            }

            //draw yellow button for adding a new IAP group
            if (GUILayout.Button("Add new Group"))
            {
                //create new group, give it a generic name based on
                //the current system time and add it to the list of groups
                IAPGroup newGroup = new IAPGroup();
                string timestamp = GenerateUnixTime();
                newGroup.name = "Grp " + timestamp;
                newGroup.id = timestamp;
                list.Add(newGroup);
                return;
            }

            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            //begin a scrolling view inside tab, pass in current Vector2 scroll position 
            scrollPosIGC = EditorGUILayout.BeginScrollView(scrollPosIGC, GUILayout.Height(350));
            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            //only draw a box behind currencies if there are any
            if (script.currency.Count > 0)
                GUI.Box(new Rect(3, 15, 796, 95), "");

            //loop through currencies
            for (int i = 0; i < script.currency.Count; i++)
            {
                EditorGUILayout.BeginVertical();
                //draw currency properties,
                //such as name and amount
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Name", GUILayout.Width(44));
                script.currency[i].name = EditorGUILayout.TextField(script.currency[i].name, GUILayout.Width(54));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default", GUILayout.Width(44));
                script.currency[i].amount = EditorGUILayout.IntField(script.currency[i].amount, GUILayout.Width(54));
                EditorGUILayout.EndHorizontal();

                //button for deleting a currency
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(52);
                GUI.backgroundColor = Color.gray;
                if (GUILayout.Button("X", GUILayout.Width(54)))
                {
                    //ask again before deleting the currency,
                    //as deleting it could cause angry customers!
                    //it's probably better not to remove currencies in production versions
                    if (EditorUtility.DisplayDialog("Delete Currency?",
                        "Existing users might lose their funds associated with this currency when updating.",
                        "Continue", "Abort"))
                    {
                        //loop over items and remove the
                        //associated currency slot for each of them
                        for (int j = 0; j < list.Count; j++)
                            for (int k = 0; k < list[j].items.Count; k++)
                                list[j].items[k].virtualPrice.RemoveAt(i);
                        //then remove the currency
                        script.currency.RemoveAt(i);
                        //reposition current currency index
                        if (script.currency.Count > 0)
                            currencyIndex = 0;
                        else
                            currencyIndex = -1;
                        break;
                    }
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            //draw currency selector, if there are any
            if (script.currency.Count > 0)
            {
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                //get all currency names,
                //then draw a popup list for selecting the desired index
                currencyNames = GetCurrencyNames();
                EditorGUILayout.LabelField("Selected Currency:", GUILayout.Width(120));
                currencyIndex = EditorGUILayout.Popup(currencyIndex, currencyNames, GUILayout.Width(140));
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(20);
            }

            //loop over IAP groups
            for (int i = 0; i < list.Count; i++)
            {
                //cache group
                IAPGroup group = list[i];
                //version 1.2 backwards compatibility fix (empty IAPGroup ids)
                if (string.IsNullOrEmpty(group.id))
                    group.id = GenerateUnixTime();

                Container shopGroup = null;
                if (shop)
                {
                    shopGroup = shop.GetContainer(group.id);
                    if (shopGroup == null)
                    {
                        shopGroup = new Container();
                        shopGroup.id = group.id;
                        shop.containers.Add(shopGroup);
                    }
                }

                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.yellow;
                //button for adding a new IAPObject (product) to this group
                if (GUILayout.Button("New Object", GUILayout.Width(120)))
                {
                    IAPObject newObj = new IAPObject();
                    for (int j = 0; j < script.currency.Count; j++)
                        newObj.virtualPrice.Add(new IAPCurrency());
                    group.items.Add(newObj);
                    break;
                }
                GUI.backgroundColor = Color.white;

                //draw group properties
                EditorGUILayout.LabelField("Group:", GUILayout.Width(45));
                group.name = EditorGUILayout.TextField(group.name, GUILayout.Width(90));
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Sort:", GUILayout.Width(35));
                orderType = (OrderType)EditorGUILayout.EnumPopup(orderType, GUILayout.Width(60));
                GUILayout.Space(10);

                if (!shop)
                    EditorGUILayout.LabelField("No ShopManager prefab found in this scene!", GUILayout.Width(300));
                else
                {
                    EditorGUILayout.LabelField("Prefab:", GUILayout.Width(45));
                    shopGroup.prefab = (GameObject)EditorGUILayout.ObjectField(shopGroup.prefab, typeof(GameObject), false, GUILayout.Width(100));
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField("Parent:", GUILayout.Width(45));
                    shopGroup.parent = (Transform)EditorGUILayout.ObjectField(shopGroup.parent, typeof(Transform), true, GUILayout.Width(100));
                }

                GUILayout.FlexibleSpace();
                if (orderType != OrderType.none)
                {
                    group.items = orderProducts(group.items);
                    break;
                }

                //same as in DrawIAP(),
                //move group up & down buttons
                int groupUpWidth = 22;
                int groupDownWidth = 22;
                if (i == 0) groupDownWidth = 48;
                if (i == list.Count - 1) groupUpWidth = 48;

                if (i > 0 && GUILayout.Button("▲", GUILayout.Width(groupUpWidth)))
                {
                    list[i] = list[i - 1];
                    list[i - 1] = group;
                    EditorGUIUtility.hotControl = 0;
                    EditorGUIUtility.keyboardControl = 0;
                }
                if (i < list.Count - 1 && GUILayout.Button("▼", GUILayout.Width(groupDownWidth)))
                {
                    list[i] = list[i + 1];
                    list[i + 1] = group;
                    EditorGUIUtility.hotControl = 0;
                    EditorGUIUtility.keyboardControl = 0;
                }

                //button for removing a group including items
                GUI.backgroundColor = Color.gray;
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    if(shop) shop.containers.Remove(shopGroup);
                    list.RemoveAt(i);
                    break;
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

                //draw header information for each item property
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ID:", GUILayout.Width(130));
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Type:", GUILayout.Width(110));
                GUILayout.Space(20);
                int spaceTitleToDescription = 110;
                if (group.items.Count == 1) spaceTitleToDescription = 130;
                EditorGUILayout.LabelField("Title:", GUILayout.Width(spaceTitleToDescription));
                GUILayout.Space(20);
                int spaceDescriptionToPrice = 150;
                if (group.items.Count == 1) spaceDescriptionToPrice = 160;
                EditorGUILayout.LabelField("Description:", GUILayout.Width(spaceDescriptionToPrice));
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Price:", GUILayout.Width(40));
                GUILayout.Space(80);
                EditorGUILayout.EndHorizontal();

                //loop over items in this group
                for (int j = 0; j < group.items.Count; j++)
                {
                    //cache item reference
                    IAPObject obj = group.items[j];
                    EditorGUILayout.BeginHorizontal();

                    //draw IAPObject (item/product) properties
                    obj.id = EditorGUILayout.TextField(obj.id, GUILayout.Width(120));
                    obj.type = (IAPType)EditorGUILayout.EnumPopup(obj.type, GUILayout.Width(110));
                    //don't allow consumable/non consumable/subscription IAP types, they must be virtual
                    if (obj.type == IAPType.consumable) obj.type = IAPType.consumableVirtual;
                    else if (obj.type == IAPType.nonConsumable || obj.type == IAPType.subscription)
                        obj.type = IAPType.nonConsumableVirtual;
                    //other item properties
                    obj.title = EditorGUILayout.TextField(obj.title);
                    obj.description = EditorGUILayout.TextField(obj.description);

                    //if a currency has been selected previously,
                    //draw an input field for the selected currency
                    if (currencyIndex > -1)
                    {
                        //version 1.1 compability fix (virtual currency int -> IAPCurrency conversion)
                        if (obj.virtualPrice == null || currencyIndex > obj.virtualPrice.Count - 1)
                        {
                            GUI.backgroundColor = Color.gray;
                            if (GUILayout.Button("X"))
                            {
                                group.items.RemoveAt(j);
                                break;
                            }
                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.EndHorizontal();
                            continue;
                        }

                        EditorGUILayout.BeginHorizontal();
                        IAPCurrency cur = obj.virtualPrice[currencyIndex];
                        cur.name = currencyNames[currencyIndex];
                        EditorGUILayout.LabelField(cur.name, GUILayout.Width(40));
                        cur.amount = EditorGUILayout.IntField(cur.amount, GUILayout.Width(60));
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                        GUILayout.FlexibleSpace();

                    //same as in DrawIAP(), requirement button
                    if (!string.IsNullOrEmpty(obj.req.entry))
                        GUI.backgroundColor = Color.yellow;
                    if (GUILayout.Button("R", GUILayout.Width(20)))
                    {
                        reqEditor = (RequirementEditor)EditorWindow.GetWindowWithRect(typeof(RequirementEditor), new Rect(0, 0, 300, 150), false, "Requirement");
                        reqEditor.obj = obj;
                    }

                    GUI.backgroundColor = Color.white;
                    //same as in DrawIAP(),
                    //move item up & down buttons
                    int buttonUpWidth = 22;
                    int buttonDownWidth = 22;
                    if (j == 0) buttonDownWidth = 48;
                    if (j == group.items.Count - 1) buttonUpWidth = 48;

                    if (j > 0 && GUILayout.Button("▲", GUILayout.Width(buttonUpWidth)))
                    {
                        group.items[j] = group.items[j - 1];
                        group.items[j - 1] = obj;
                        EditorGUIUtility.hotControl = 0;
                        EditorGUIUtility.keyboardControl = 0;
                    }
                    if (j < group.items.Count - 1 && GUILayout.Button("▼", GUILayout.Width(buttonDownWidth)))
                    {
                        group.items[j] = group.items[j + 1];
                        group.items[j + 1] = obj;
                        EditorGUIUtility.hotControl = 0;
                        EditorGUIUtility.keyboardControl = 0;
                    }

                    //button for removing an item of the group
                    GUI.backgroundColor = Color.gray;
                    if (GUILayout.Button("X"))
                    {
                        group.items.RemoveAt(j);
                        break;
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndHorizontal();
                }
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
                GUILayout.Space(30);
            }

            //ends the scrollview defined above
            EditorGUILayout.EndScrollView();
        }


        //orders a list of IAPObjects (products)
        //based on the selected criterion
        List<IAPObject> orderProducts(List<IAPObject> list)
        {
            //create temporary list for sorted IAPObject entries
            //loop over current list and copy reference to the sorted list
            List<IAPObject> sortedList = new List<IAPObject>();
            for (int i = 0; i < list.Count; i++)
                sortedList.Add(list[i]);

            //regular expressions currency string conversion pattern
            //for sorting prices, we only consider numbers, commas and dots
            string pattern = "[^0-9,.]";

            //when ordering prices, first check some requirements
            //for virtual items, there must be a currency to sort on
            //for real money items, check if price values match our pattern
            //and they can actually be converted to decimals
            switch (orderType)
            {
                case OrderType.priceAsc:
                case OrderType.priceDesc:

                    //log warning if price sorting has been selected on virtual items but no currency specified  
                    if (list[0].type == IAPType.consumableVirtual || list[0].type == IAPType.nonConsumableVirtual)
                    {
                        if (script.currency.Count <= 0)
                        {
                            Debug.LogWarning("Cannot sort virtual IAPList: No currency specified to sort on.");
                            orderType = OrderType.none;
                            return list;
                        }
                        else
                            break;
                    }

                    foreach (IAPObject obj in list)
                    {
                        //create temporary decimal value
                        //and try parsing the string
                        decimal value;
                        if (decimal.TryParse(Regex.Replace(obj.realPrice, pattern, ""), out value))
                            continue;
                        else
                        {
                            //log warning if string couldn't be converted to decimal value
                            Debug.LogWarning("Sorting IAPList failed: " + obj.title + "'s price contains no number.");
                            orderType = OrderType.none;
                            return list;
                        }
                    }
                    break;
            }

            //start sorting - differ between order types
            //we use lambda expressions for now, but in some cases
            //the sorting methods below aren't that precise
            switch (orderType)
            {
                //ascending price:
                //for virtual items we use the active price -
                //for real money items first convert input string to decimal -
                //and compare with next entry, then take first
                case OrderType.priceAsc:
                    if (list[0].type == IAPType.consumableVirtual || list[0].type == IAPType.nonConsumableVirtual)
                        sortedList.Sort((a, b) => a.virtualPrice[currencyIndex].amount.CompareTo(b.virtualPrice[currencyIndex].amount));
                    else
                        sortedList.Sort((a, b) => decimal.Parse(Regex.Replace(a.realPrice, pattern, ""))
                                    .CompareTo(decimal.Parse(Regex.Replace(b.realPrice, pattern, ""))));
                    break;
                //descending price:
                //for virtual items we use the active price -
                //for real money items first convert input string to decimal -
                //and compare with next entry, then take second
                case OrderType.priceDesc:
                    if (list[0].type == IAPType.consumableVirtual || list[0].type == IAPType.nonConsumableVirtual)
                        sortedList.Sort((a, b) => -a.virtualPrice[currencyIndex].amount.CompareTo(b.virtualPrice[currencyIndex].amount));
                    else
                        sortedList.Sort((a, b) => -decimal.Parse(Regex.Replace(a.realPrice, pattern, ""))
                                     .CompareTo(decimal.Parse(Regex.Replace(b.realPrice, pattern, ""))));
                    break;
                //ascending title:
                //compare with next entry, then take first
                case OrderType.titleAsc:
                    sortedList.Sort((a, b) => a.title.CompareTo(b.title));
                    break;
                //descending title:
                //compare with next entry, then take second
                case OrderType.titleDesc:
                    sortedList.Sort((a, b) => -a.title.CompareTo(b.title));
                    break;
            }
            //reset order type
            orderType = OrderType.none;
            //return sorted list reference
            return sortedList;
        }


        //returns an array that holds all currency names
        string[] GetCurrencyNames()
        {
            //get list of currencies
            List<IAPCurrency> list = script.currency;
            //create new array with the same size, then loop
            //over currencies and populate array with their names
            string[] curs = new string[list.Count];
            for (int i = 0; i < curs.Length; i++)
                curs[i] = list[i].name;
            //return names array
            return curs;
        }


        string GenerateUnixTime()
        {
            var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            return (System.DateTime.UtcNow - epochStart).TotalSeconds.ToString() + Random.Range(0, 1000);
        }


        private static void SavePrefab()
        {
            if (!IAPPrefab) return;

            GameObject go = PrefabUtility.InstantiatePrefab(IAPPrefab) as GameObject;
            PrefabUtility.ReplacePrefab(go, IAPPrefab);
            DestroyImmediate(go);
        }


        void TrackChange()
        {
            //if we typed in other values in the editor window,
            //we need to repaint it in order to display the new values
            if (GUI.changed)
            {
                //we have to tell Unity that a value of our script has changed
                //http://unity3d.com/support/documentation/ScriptReference/EditorUtility.SetDirty.html
                if(shop) EditorUtility.SetDirty(shop);
                //Register the snapshot state made with CreateSnapshot
                //so the user can later undo back to that state
                #if UNITY_4_2
                    Undo.RegisterSnapshot();
                #endif
                //repaint editor GUI window
                Repaint();
            }
            else
            {
                //clear the snapshot at end of call
                #if UNITY_4_2
                    Undo.ClearSnapshotTarget();
                #endif
            }
        }


        //track project save state and save changes to prefab on project save
        public class IAPModificationProcessor : UnityEditor.AssetModificationProcessor
        {
            public static string[] OnWillSaveAssets(string[] paths)
            {
                if (IAPEditor.iapEditor)
                    IAPEditor.SavePrefab();
                return paths;
            }
        }
    }
}