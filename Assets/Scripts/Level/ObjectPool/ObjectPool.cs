using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Object pool class can manage gameobject by recycle them, 
/// this can prevent cpu spike frequently.
/// 
/// gameobject that was instantiate by pool manager
/// are able to be reused as long as it is not currently using.
/// 
/// If pool manager find out there is no avaliable gameobject when
/// you requesting a certain gameobject pool manager will instantiate new one.
/// 
/// Object pool manager provide two ways to instantiate gameobject:
/// 
/// 1.Give pool manager any gameobject prefabs that you will used in level.
/// 
/// 2.Put any gameobject prefabs into a certain folder which under Resources folder(Unity reuqired),
/// then give pool manager the folder's name(Not Resources), as well as, enabled autoLoadPrefab option. This 
/// way pool manager will auto load all gameobject prefabs from that folder.
/// 
/// Give quantity to pool manager to indicate how many gameobject is going to be instantiated for each gameobject prefabes
/// when scene start.
/// Adjust this value to conserve memory usage. 
/// </summary>

//Replace by TrashMan
public class ObjectPool : MonoBehaviour 
{

	/// <summary>
	/// Auto load prefab from folder require resource folder name to
	/// be fill.
	/// </summary>
	public bool autoLoadPrefab = false;

	/// <summary>
	/// Name of resource folders.
	/// Pool will automatic create every prefabs under designate folders.
	/// The designate folder must be the parent folder under Resources folder.
	/// Do not assign duplicate folder name
	/// </summary>
	public string[] resourceFolders;

	/// <summary>
	/// The object prefabs that will
	/// be used to instantiate
	/// </summary>
	public List<GameObject> objectPrefabs;

	/// <summary>
	/// How many gameobject will be instantiate for 
	/// each object prefabs
	/// </summary>
	public int initialQuantity = 1;

	/// <summary>
	/// Object pool.
	/// It store prefab's name and number of 
	/// instantiated gameobject by key,value
	/// </summary>
	private Dictionary<string, List<GameObject>> pool;

	/// <summary>
	/// Hold bunch of instantiated gameobject
	/// </summary>
	private GameObject objectHolder;

	void Awake()
	{


	}

	void OnEnable()
	{
		//create a empty gameobject in scene
		//that will hold bunch of instantiated gameobject
		objectHolder = new GameObject ();
		objectHolder.name = "ObjectPool";
		
		//create bunch of gameobject
		InitPool ();
	}

	/// <summary>
	/// Gets the object from pool by name.
	/// </summary>
	/// <returns>The object from pool.</returns>
	/// <param name="name">Name.</param>
	/// <param name="position">Position.</param>
	/// <param name="rotation">Rotation.</param>
	public GameObject GetObjectFromPool(string name, Vector3 position, Quaternion rotation)
	{
		if((pool.Count > 0) && (pool.ContainsKey(name)))
		{
			List<GameObject> listObjects = pool[name];

			for(int i=0; i<listObjects.Count; i++)
			{
				GameObject retObject = listObjects[i];

				if(retObject.activeInHierarchy)
				{
					continue;
				}
				else
				{

					//active child
					for(int j=0; j<retObject.transform.childCount; j++)
					{
						GameObject rChlid = retObject.transform.GetChild(j).gameObject;
						rChlid.SetActive(true);
					}

					//set position and rotation
					retObject.transform.position = position;
					retObject.transform.rotation = rotation;

					//active object
					retObject.SetActive(true);

					return retObject;
				}
			}

			//no object avaliable create new one
			GameObject go = CreateObject(name);

			//set position and rotation
			go.transform.position = position;
			go.transform.rotation = rotation;

			return go;
		}

		Debug.LogError ("Pool does not contain this object: " + name);
		return null;
	}

	/// <summary>
	/// Gets the object from pool by prefab.
	/// </summary>
	/// <returns>The object from pool.</returns>
	/// <param name="prefab">Prefab.</param>
	/// <param name="position">Position.</param>
	/// <param name="rotation">Rotation.</param>
	public GameObject GetObjectFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return GetObjectFromPool (prefab.name, position, rotation);
	}

	/// <summary>
	/// Recycles the object.
	/// </summary>
	/// <param name="theObject">The object.</param>
	public void RecycleObject(GameObject theObject)
	{
		//if this object was in pool deactive it...otherwise log error
		if(pool.ContainsKey(theObject.name))
		{
			theObject.transform.parent = objectHolder.transform;

			//deactive object
			theObject.SetActive (false);


			//deactive child
			for(int j=0; j<theObject.transform.childCount; j++)
			{
				GameObject rChlid = theObject.transform.GetChild(j).gameObject;
				rChlid.SetActive(false);
			}

		}
		else
		{
			Debug.LogError("pool can not recycle this object: "+theObject.name+" this object was not in pool");
		}

	}

	/// <summary>
	/// Recycles all objects.
	/// </summary>
	public void RecycleAllObjects()
	{
		foreach(string key in pool.Keys)
		{
			List<GameObject> listOfObject = pool[key];

			for(int j=0; j<listOfObject.Count; j++)
			{
				RecycleObject(listOfObject[j]);
			}
		}
	}
	
	/// <summary>
	/// Init pool.
	/// </summary>
	void InitPool()
	{
		//if auto load enabled
		if(autoLoadPrefab)
		{
			//if resource folder was given
			if(resourceFolders.Length > 0)
			{
				for(int i=0; i<resourceFolders.Length; i++)
				{
					//load all prefab assets
					Object[] prefabAssets = Resources.LoadAll(resourceFolders[i], typeof(GameObject));

					//if has found prefab assets 
					if(prefabAssets.Length > 0)
					{
						if(objectPrefabs == null)
						{
							objectPrefabs = new List<GameObject>();
						}
						
						//add prefab asset to list
						for(int j=0; j<prefabAssets.Length; j++)
						{
							GameObject go = (GameObject)prefabAssets[j];
							objectPrefabs.Add(go);
						}
					}
					else
					{
						Debug.LogError("Auto load prefab enabled but no prefab under folder: "+resourceFolders[i]);
						
						return;
					}
				}

			}
			else
			{
				Debug.LogError("Auto load prefab enabled but no folder name was specified");

				return;
			}
		}

		//if has type of prefab
		if(objectPrefabs.Count > 0)
		{
			//create pool
			pool = new Dictionary<string, List<GameObject>>();
			
			for(int i=0; i<objectPrefabs.Count; i++)
			{
				GameObject prefab = objectPrefabs[i];
				
				//if prefab had been instantiated...user has given same prefab more than once...continue
				if(pool.ContainsKey(prefab.name))
				{
					Debug.LogError("You have given same prefab more than once");
					
					continue;
				}
				
				List<GameObject> temp = new List<GameObject>();

				//create number of object instants
				for(int j=0; j<initialQuantity; j++)
				{
					//instantiate gameobject
					GameObject obj = Instantiate(prefab) as GameObject;
					
					//remove name clone
					obj.name = prefab.name;
					
					//add gameobject
					temp.Add(obj);
					
					//make it child
					obj.transform.parent = objectHolder.transform;
					
					//disable gameobject
					obj.SetActive(false);
				}
				
				//add key and value
				pool.Add(prefab.name, temp);
			}
			
		}
		
	}

	/// <summary>
	/// Creates an object.
	/// It auto add new gameobject into list
	/// </summary>
	/// <returns>The GameObject.</returns>
	/// <param name="name">Name of prefab.</param>
	GameObject CreateObject(string name)
	{
		for(int i=0; i<objectPrefabs.Count; i++)
		{
			//if given name match any prefab name
			if(objectPrefabs[i].name == name)
			{
				//instantiate gameobject
				GameObject obj = Instantiate(objectPrefabs[i]) as GameObject;

				//remove name clone
				obj.name = objectPrefabs[i].name;

				//add to list
				List<GameObject> listObjects = pool[objectPrefabs[i].name];
				listObjects.Add(obj);

				//make it child
				obj.transform.parent = objectHolder.transform;
				
				return obj;
			}
		}

		return null;
	}

	//return this pool's name
	//public string PoolName{ get{ return objectHolder.name; }}
}
