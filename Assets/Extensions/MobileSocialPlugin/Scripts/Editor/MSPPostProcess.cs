using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;

public class MSPPostProcess  {

	private const string BUNLDE_KEY = "SA_PP_BUNLDE_KEY";

	[PostProcessBuild(48)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {

		#if UNITY_IPHONE
		string Accounts = "Accounts.framework";
		if(!ISDSettings.Instance.frameworks.Contains(Accounts)) {
			ISDSettings.Instance.frameworks.Add(Accounts);
		}


		string SocialF = "Social.framework";
		if(!ISDSettings.Instance.frameworks.Contains(SocialF)) {
			ISDSettings.Instance.frameworks.Add(SocialF);
		}

		string MessageUI = "MessageUI.framework";
		if(!ISDSettings.Instance.frameworks.Contains(MessageUI)) {
			ISDSettings.Instance.frameworks.Add(MessageUI);
		}

		#endif

		#if UNITY_ANDROID
		string file = PluginsInstalationUtil.ANDROID_DESTANATION_PATH + "AndroidManifest.xml";
		string Manifest = FileStaticAPI.Read(file);
		Manifest = Manifest.Replace("%APP_BUNDLE_ID%", PlayerSettings.bundleIdentifier);
		
		//checking for bundle change
		if(OldBundle != string.Empty) {
			if(OldBundle != PlayerSettings.bundleIdentifier) {
				int result = EditorUtility.DisplayDialogComplex("Mobile Social: bundle id change detected", "Project bundle Identifier changed, do you wnat to replase old bundle: " + OldBundle + "with new one: " + PlayerSettings.bundleIdentifier, "Yes", "No", "Later");
				
				
				switch(result) {
				case 0:
					Manifest = Manifest.Replace(QUOTE +  OldBundle + QUOTE, QUOTE +  PlayerSettings.bundleIdentifier + QUOTE);
					Manifest = Manifest.Replace(QUOTE +  OldBundle + ".fileprovider" + QUOTE, QUOTE +  PlayerSettings.bundleIdentifier + ".fileprovider" + QUOTE);
					OldBundle = PlayerSettings.bundleIdentifier;
					break;
				case 1:
					OldBundle = PlayerSettings.bundleIdentifier;
					break;
					
				}
				
			}
			
			
			
		} else {
			OldBundle = PlayerSettings.bundleIdentifier;
		}
		
		FileStaticAPI.Write(file, Manifest);
		Debug.Log("MSP Post Process Done");
		#endif

	}


	private static string OldBundle {
		get {
			if(EditorPrefs.HasKey(BUNLDE_KEY)) {
				return EditorPrefs.GetString(BUNLDE_KEY);
			} else {
				return string.Empty;
			}
		}
		
		
		set {
			EditorPrefs.SetString(BUNLDE_KEY, value);
		}
	}
	
	private static string QUOTE {
		get {
			return "\"";
		}
	}

}
