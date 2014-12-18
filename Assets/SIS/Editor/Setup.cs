using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SIS
{
    [InitializeOnLoad]
    public class Setup : EditorWindow
    {
        private static Config currentConfig;
        private static string settingsPath = "Assets/SIS/Prefabs/Resources/" + "Plugin_Setup.asset";
        private string packagesPath = "Assets/SIS/Packages/";

        static Setup()
        {
            EditorApplication.hierarchyWindowChanged += EditorUpdate;
            EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
        }


        [MenuItem("Window/Simple IAP System/Plugin Setup")]
        static void Init()
        {
            EditorWindow.GetWindowWithRect(typeof(Setup), new Rect(0, 0, 340, 260), false, "Plugin Setup");
        }


        private static void EditorUpdate()
        {
            if (Setup.Current == null)
                return;

            if (Setup.Current.autoOpen)
                Init();
        }


        private static void PlaymodeStateChanged()
        {
            if (EditorApplication.isPlaying || !EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if (Setup.Current.autoOpen)
                EditorUtility.DisplayDialog("Plugin Setup Required", "You haven't imported any plugin packages yet."
                                            + "\nSimple IAP System won't work without these.", "Ok");
        }


        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Simple IAP System comes with billing plugin dependent");
            EditorGUILayout.LabelField("packages. Which one would you like to use?");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Please read the PDF documentation after importing.");
            EditorGUILayout.LabelField("Other links: Window > Simple IAP System > About.");

            GUILayout.Space(30);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prime31", GUILayout.Width(100));
            if (GUILayout.Button("Import"))
            {
                AssetDatabase.ImportPackage(packagesPath + "Prime31.unitypackage", true);
                DisableAutoOpen();
            }
            if (GUILayout.Button("?", GUILayout.Width(20)))
            {
                Application.OpenURL("https://www.assetstore.unity3d.com/#/publisher/270");
            }
            EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stans Assets", GUILayout.Width(100));
            if (GUILayout.Button("Import"))
            {
                AssetDatabase.ImportPackage(packagesPath + "StansAssets.unitypackage", true);
                DisableAutoOpen();
            }
            if (GUILayout.Button("?", GUILayout.Width(20)))
            {
                Application.OpenURL("https://www.assetstore.unity3d.com/#/publisher/2256");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Unibill", GUILayout.Width(100));
            if (GUILayout.Button("Import"))
            {
                AssetDatabase.ImportPackage(packagesPath + "Unibill.unitypackage", true);
                DisableAutoOpen();
            }
            if (GUILayout.Button("?", GUILayout.Width(20)))
            {
                Application.OpenURL("https://www.assetstore.unity3d.com/#/content/5767");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Neatplug", GUILayout.Width(100));
            if (GUILayout.Button("Import"))
            {
                AssetDatabase.ImportPackage(packagesPath + "Neatplug.unitypackage", true);
                DisableAutoOpen();
            }
            if (GUILayout.Button("?", GUILayout.Width(20)))
            {
                Application.OpenURL("http://neatplug.com/unity3d-plugins-android");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("OpenIAB (free)", GUILayout.Width(100));
            if (GUILayout.Button("Import"))
            {
                AssetDatabase.ImportPackage(packagesPath + "OpenIAB.unitypackage", true);
                DisableAutoOpen();
            }
            if (GUILayout.Button("?", GUILayout.Width(20)))
            {
                Application.OpenURL("https://github.com/onepf/OpenIAB-Unity-Plugin");
            }
            EditorGUILayout.EndHorizontal();
        }


        void DisableAutoOpen()
        {
            Setup.Current.autoOpen = false;
            Setup.Save();
            this.Close();
        }


        public static Config Current
        {
            get
            {
                if (currentConfig == null)
                {
                    currentConfig = Resources.Load("Plugin_Setup", typeof(Config)) as Config;

                    if (currentConfig == null)
                    {
                        string dir = Path.GetDirectoryName(settingsPath);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                            AssetDatabase.ImportAsset(dir);
                        }

                        currentConfig = (Config)ScriptableObject.CreateInstance(typeof(Config));
                        if (currentConfig != null)
                        {
                            AssetDatabase.CreateAsset(currentConfig, settingsPath);
                        }
                    }
                }

                return currentConfig;
            }
            set
            {
                currentConfig = value;
            }
        }

        public static void Save()
        {
            EditorUtility.SetDirty(Setup.Current);
        }
    }
}