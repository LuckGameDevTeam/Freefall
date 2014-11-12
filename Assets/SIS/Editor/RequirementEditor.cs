/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SIS
{
    /// <summary>
    /// Requirement editor. Embedded in the IAP Settings editor,
    /// for defining requirements for locked IAP items
    /// </summary>
    public class RequirementEditor : EditorWindow
    {
        //IAP object to modify, set by IAPEditor
        public IAPObject obj;


        void OnGUI()
        {
            if (obj == null)
                return;

            //draw label for selected IAP name
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("IAP:", GUILayout.Width(40));
            EditorGUILayout.LabelField(obj.id, EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            //draw fields for database entry id and its target to reach
            //when an entry has been entered in its field, color them yellow (active) 
            if (!string.IsNullOrEmpty(obj.req.entry))
                GUI.backgroundColor = Color.yellow;
            obj.req.entry = EditorGUILayout.TextField(new GUIContent("DB Entry:", "Entry in DBManager which stores the required " +
                                                                     "value in your game, e.g. 'level', 'score', etc."), obj.req.entry);
            obj.req.target = EditorGUILayout.IntField(new GUIContent("Target:", "Value to reach of the entry defined above"), obj.req.target);
            EditorGUILayout.Space();

            //draw field for defining an "what to do to unlock" text
            GUI.backgroundColor = Color.white;
            EditorGUILayout.LabelField("optional", GUILayout.Width(60));
            obj.req.labelText = EditorGUILayout.TextField(new GUIContent("Label Text:", "Text that describes what the player has to do to fulfill " +
                                                                         "the requirement. Displayed in the prefab's 'lockedLabel' UILabel, if set"), obj.req.labelText);

            //button for closing this window
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            if (GUILayout.Button("Close"))
            {
                this.Close();
            }
        }
    }
}
