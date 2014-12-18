using UnityEngine;
using System.Collections;

/// <summary>
/// UI link to web.
/// 
/// This class handle link to web
/// </summary>
public class UILinkToWeb : MonoBehaviour 
{
	/// <summary>
	/// The URL.
	/// </summary>
	public string url;

	/// <summary>
	/// Opens the web.
	/// </summary>
	public void OpenWeb()
	{
		DebugEx.Debug("open web");
		Application.OpenURL ("http://"+url);
	}
}
