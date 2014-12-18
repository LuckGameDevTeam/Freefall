using UnityEngine;
using System.Collections;

public class UITutorialSlideClick : MonoBehaviour 
{
	private UITutorialControl control;

	void Awake()
	{
		control = NGUITools.FindInParents<UITutorialControl> (gameObject);
	}

	void OnClick()
	{
		control.ShowNextSlide ();
	}

}
