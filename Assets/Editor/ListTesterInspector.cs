using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ListTester)), CanEditMultipleObjects]
public class ListTesterInspector : Editor 
{

	public override void OnInspectorGUI () 
	{
		serializedObject.Update();
		EditorList.Show(serializedObject.FindProperty("integers"), EditorListOption.ListSize);
		EditorList.Show(serializedObject.FindProperty("vectors"));
		EditorList.Show(serializedObject.FindProperty("colorPoints"),  EditorListOption.Buttons);
		EditorList.Show(serializedObject.FindProperty("objects"), EditorListOption.ListLabel | EditorListOption.Buttons);
		EditorList.Show(serializedObject.FindProperty("notAList"));
		EditorList.Show(serializedObject.FindProperty("aList"), EditorListOption.ListLabel | EditorListOption.Buttons);
		serializedObject.ApplyModifiedProperties();
	}
}
