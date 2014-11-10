using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UnityCustomMenu : MonoBehaviour
{

	#if UNITY_EDITOR
	/// <summary>
	/// Easy way to create InGameUI gameobject on scene
	/// </summary>
	[MenuItem("GameMenu/InGame/AddInGameUI")]
	static void CreateInGameUI()
	{

		string path = "Assets/Prefabs/UI/InGameUI/UI Root (2D).prefab";
		
		//check if scene has already one...if so then don't create
		if(GameObject.FindObjectOfType(typeof(UIRoot)) != null)
		{
			Debug.LogError("You can not create another one InGameUI gameobject, delete current one and try again");
			
			return;
		}
		
		//load prefab
		UnityEngine.Object uiObejct = AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));

		if(uiObejct != null)
		{
			//create prefab on scene
			GameObject go = Instantiate (uiObejct, Vector3.zero, Quaternion.identity) as GameObject;
			
			//rename
			go.name = uiObejct.name;
		}
		else
		{
			Debug.LogError("Fail to create InGameUI to scene, can not find prefabs at "+ path); 
		}
		

		
		
	}

	
	/// <summary>
	/// Easy way to create AdController gameobject on scene
	/// </summary>
	[MenuItem("GameMenu/Ad/CreateAdControl")]
	static void CreateAdControl()
	{
		string path = "Assets/Prefabs/AdController/AdController.prefab";
		
		//check if scene has already one...if so then don't create
		if(GameObject.FindObjectOfType(typeof(AdControl)) != null)
		{
			Debug.LogError("You can not create another one AdController gameobject, delete current one and try again");
			
			return;
		}
		
		//load prefab
		UnityEngine.Object adControllerObject = AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));

		if(adControllerObject != null)
		{
			//create prefab on scene
			GameObject go = Instantiate (adControllerObject, Vector3.zero, Quaternion.identity) as GameObject;
			
			//rename
			go.name = adControllerObject.name;
		}
		else
		{
			Debug.LogError("Fail to create AdControl to scene, can not find prefabs at "+ path); 
		}

	}
	#endif
}
