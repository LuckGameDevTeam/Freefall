using UnityEngine;
using System.Collections;
using SIS;

public class UIUpdateGold : MonoBehaviour 
{
	public UILabel goldLabel;

	/// <summary>
	/// time for animating the current start to end value
	/// </summary>
	public float duration = 2f;

	private int curValue = 0;

	void OnEnable()
	{
		SetGold ();

		//register currency balance change event
		DBManager.updatedDataEvent += ConfigureGold;
	}
	
	void OnDisable()
	{

		//unregister currency balance change event
		DBManager.updatedDataEvent -= ConfigureGold;
	}

	// Use this for initialization
	void Start () 
	{
		SetGold ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void SetGold()
	{
		curValue = DBManager.GetFunds (IAPManager.GetCurrency () [0].name);

		goldLabel.text = curValue.ToString ();
	}

	void ConfigureGold()
	{
		StopCoroutine("CountTo");

		if(gameObject.activeInHierarchy)
		{
			StartCoroutine("CountTo", DBManager.GetFunds(IAPManager.GetCurrency()[0].name));
		}

	}

	IEnumerator CountTo(int target)
	{
		//remember current value as starting position
		int start = curValue;
		
		//over the duration defined, lerp value from start to target value
		//and set the UILabel text to this value
		for (float timer = 0; timer < duration; timer += Time.deltaTime)
		{
			float progress = timer / duration;
			curValue = (int)Mathf.Lerp(start, target, progress);
			goldLabel.text = curValue + "";
			yield return null;
		}
		
		//once the duration is over, directly set the value and text
		//to the targeted value to avoid rounding issues or inconsistency
		curValue = target;
		goldLabel.text = curValue + "";
	}
}
