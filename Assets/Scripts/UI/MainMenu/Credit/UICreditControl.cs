using UnityEngine;
using System.Collections;

public class UICreditControl : MonoBehaviour 
{
	public delegate void EventOnCreditClose(UICreditControl control);
	public EventOnCreditClose Evt_OnCreditClose;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ShowCredit()
	{
		gameObject.SetActive (true);
	}
	
	public void CloseCredit()
	{
		gameObject.SetActive (false);
		
		if(Evt_OnCreditClose != null)
		{
			Evt_OnCreditClose(this);
		}
	}
}
