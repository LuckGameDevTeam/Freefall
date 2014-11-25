using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CSharpCeateWindow : EditorWindow 
{
	//user pick a folder path or not
	bool pathSet = false;

	//user picked folder's path
	string selectPath;

	//user entered class name
	string className = "";

	//the path where file will be save to
	string savedPath;

	//are there duplicate file
	bool fileExist = false;


	[MenuItem ("SLM/CreatePersistantDataClass")]
	public static void  ShowWindow () 
	{
		EditorWindow.GetWindow(typeof(CSharpCeateWindow));
	}
	
	void OnGUI () 
	{

		//get folder path be selection
		selectPath = AssetDatabase.GetAssetPath (Selection.activeObject);

		if((selectPath == null) || (selectPath == "") || (Path.GetExtension(selectPath) != ""))
		{
			pathSet = false;
			selectPath = "Invaild folder path, select project folder path first";
		}
		else
		{
			pathSet = true;
		}


		//path
		EditorGUILayout.BeginHorizontal ();

		if(pathSet)
		{
			EditorGUILayout.LabelField (new GUIContent ("Class file save to: "+selectPath, "Class will be save to this folder path"));

		}
		else
		{
			EditorGUILayout.LabelField (new GUIContent ("Please choose a folder in project panel to begin with"), EditorStyles.boldLabel);
		}

		EditorGUILayout.EndHorizontal ();


		if(pathSet)
		{
			//type class name
			EditorGUILayout.BeginHorizontal ();
			

			EditorGUILayout.LabelField(new GUIContent("Class name:", "Type class name here, do not type space it will be trimed"), GUILayout.Width(70f));

			className = EditorGUILayout.TextField("", className);
			//className = (cn != "")? cn:className;

			//trim space if needed
			if((className != null) && (className != ""))
				className = className.Replace(" ", "");
			
			EditorGUILayout.EndHorizontal ();


			//save path 
			if((className != null) && (className != ""))
			{
				savedPath = selectPath+"/"+className+".cs";
			}

			//check if file exist or not
			if(File.Exists(savedPath))
			{
				fileExist = true;

				EditorGUILayout.BeginHorizontal ();
				
				EditorGUILayout.LabelField("File "+className+".cs"+" already exist, choose other class name", EditorStyles.boldLabel);
				
				EditorGUILayout.EndHorizontal ();

			}
			else
			{
				fileExist = false;
			}
			
			if((fileExist == false) && (className != null) && (className != ""))
			{
				PreviewCode();

				if((!fileExist) && (className != null) && (className != ""))
				{
					GUI.color = Color.green;
					if(GUILayout.Button("Create script"))
					{
						if(CreateCodeFile())
						{
							Debug.Log("Create code file success");

							AssetDatabase.Refresh();

							EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(savedPath, typeof(Object)));

						}
						else
						{
							Debug.LogError("Create code file fail");
						}
					}
					GUI.color = Color.white;
				}

			}
			

		}

	}

	void PreviewCode()
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Script preview:");
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();

		EditorGUILayout.HelpBox (GenerateCode(), MessageType.None, true);

		EditorGUILayout.EndHorizontal ();
	}

	string GenerateCode()
	{
		string code = "";

		code += "using UnityEngine;\n";
		code += "using System.Collections;\n";
		code += "using System;\n";
		code += "using System.Collections.Generic;\n";
		code += "\n";
		code += "\n";
		code += "/// <summary>\n";
		code += "/// "+"This class "+className+" is automatic generated.\n";
		code += "/// \n";
		code += "/// Implement your persistant data calss here\n";
		code += "/// </summary>";
		code += "\n";
		code += "[Serializable]\n";
		code += "public class " + className + " : PersistantMetaData\n";
		code += "{";
		code += "\n";
		code += "\t//Declare your attributes here";
		code += "\n";
		code += "\n";
		code += "\t//Declare your function here";
		code += "\n";
		code += "}";

		return code;
	}

	bool CreateCodeFile()
	{
		if(File.Exists(savedPath))
		{
			Debug.LogError("File exist at path:"+savedPath);

			return false;
		}

		if((savedPath == null) || (savedPath == ""))
		{
			Debug.LogError("no save path");

			return false;
		}

		StreamWriter sw = new StreamWriter (savedPath);

		sw.Write (GenerateCode());

		sw.Close ();

		return true;
	}
}
