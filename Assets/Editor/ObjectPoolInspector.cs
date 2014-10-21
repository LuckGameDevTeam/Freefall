using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ObjectPool))]
public class ObjectPoolInspector : Editor 
{
	bool expend = true;

	public override void OnInspectorGUI ()
	{
		/*
		ObjectPool op = (ObjectPool)target;

		EditorGUILayout.BeginHorizontal ();
		{
			EditorGUILayout.PrefixLabel(serializedObject.FindProperty("autoLoadPrefab").name);
			op.autoLoadPrefab = EditorGUILayout.Toggle(op.autoLoadPrefab?true:false);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.HelpBox ("Enable to auto load prefab from resources folder make sure you fill folder name", MessageType.Warning);



		if(op.autoLoadPrefab)
		{
			expend = EditorGUILayout.Foldout(expend, "Folder to load");

			if(expend)
			{
				EditorGUI.indentLevel = 1;
				for(int i=0; i<op.resourceFolders.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.PrefixLabel("Folder");
						EditorGUILayout.TextField(op.resourceFolders[i]);

						GUI.color = Color.red;
						if(GUILayout.Button("X"))
						{
							Debug.Log("delete folder at: "+i);
						}
						GUI.color = Color.white;
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUI.indentLevel = 0;

				GUI.color = Color.green;
				if(GUILayout.Button("Add new folder"))
				{
					Debug.Log("add folder");
				}
				GUI.color = Color.white;
			}

		}
		else
		{
			expend = EditorGUILayout.Foldout(expend, "Prefabs");

			if(expend)
			{

				EditorGUI.indentLevel = 1;
				for(int i=0; i<op.objectPrefabs.Count; i++)
				{

					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField("Prefab");
						op.objectPrefabs[i] = (GameObject)EditorGUILayout.ObjectField(op.objectPrefabs[i], typeof(GameObject), false);

						GUI.color = Color.red;
						if(GUILayout.Button("X"))
						{
							Debug.Log("delete prefab at: "+i);

							op.objectPrefabs.RemoveAt(i);
						}
						GUI.color = Color.white;
					}
					EditorGUILayout.EndHorizontal();

					if(i < op.objectPrefabs.Count)
					{
						if(op.objectPrefabs[i] == null)
						{
							EditorGUILayout.HelpBox("You must assign an object", MessageType.Error);
						}
					}

				}
				EditorGUI.indentLevel = 0;

				GUI.color = Color.green;
				if(GUILayout.Button("Add new prefab"))
				{
					Debug.Log("add prefab");

					op.objectPrefabs.Add(null);
				}
				GUI.color = Color.white;
			}
		}

		EditorGUILayout.BeginHorizontal ();
		{
			EditorGUILayout.PrefixLabel(serializedObject.FindProperty("initialQuantity").name);
			op.initialQuantity = EditorGUILayout.IntField(op.initialQuantity);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.HelpBox ("How many time should instantiate per prefab", MessageType.Warning);
		*/

		DrawDefaultInspector ();
	}
}
