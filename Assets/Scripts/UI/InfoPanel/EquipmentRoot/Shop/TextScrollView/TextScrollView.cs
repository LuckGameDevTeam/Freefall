using UnityEngine;
using System.Collections;

/// <summary>
/// Text scroll view.
/// </summary>
public class TextScrollView : MonoBehaviour 
{

	public UIDraggablePanel dragableContentPanel;

	public UILocalize descLocalization;

	public void SetDescriptionKey(string key)
	{
		descLocalization.key = key;

		Invoke ("Refresh", 0.1f);
	}

	void Refresh()
	{
		dragableContentPanel.ResetPosition ();
	}
}
