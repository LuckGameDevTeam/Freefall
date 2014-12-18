using UnityEngine;
using System.Collections;

public class UITutorialControl : MonoBehaviour 
{

	public delegate void EventOnTutorialClose(UITutorialControl control);
	public EventOnTutorialClose Evt_OnTutorialClose;

	/// <summary>
	/// The tutorial slides.
	/// </summary>
	public GameObject[] tutorialSlides;

	/// <summary>
	/// The current slide.
	/// </summary>
	private int currentSlide;
	
	// Use this for initialization
	void Start () 
	{
	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowTutorial()
	{
		foreach(GameObject obj in tutorialSlides)
		{
			obj.SetActive(false);
		}

		gameObject.SetActive (true);

		//reset current slide index
		currentSlide = 0;
		
		if(tutorialSlides.Length > 0)
		{
			tutorialSlides[currentSlide].SetActive(true);
		}
		else
		{
			DebugEx.DebugError("No tutorial slide was assigned");
		}
	}
	
	public void CloseTutorial()
	{
		foreach(GameObject obj in tutorialSlides)
		{
			obj.SetActive(false);
		}

		gameObject.SetActive (false);
		
		if(Evt_OnTutorialClose != null)
		{
			Evt_OnTutorialClose(this);
		}
	}
	
	public void ShowNextSlide()
	{
		//hide current slide
		tutorialSlides [currentSlide].SetActive (false);

		//increase slide index
		currentSlide++;

		//if slide index greater then total slide close tutorial
		if(currentSlide < tutorialSlides.Length)
		{
			tutorialSlides[currentSlide].SetActive(true);
		}
		else
		{
			CloseTutorial();
		}
	}
}
