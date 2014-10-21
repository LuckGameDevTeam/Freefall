using UnityEngine;
using System.Collections;

/// <summary>
/// UI HP display.
/// 
/// This class control each health block 
/// </summary>
public class UIHPDisplay : MonoBehaviour 
{
	/// <summary>
	/// The fill sprite.
	/// </summary>
	public UISprite fillSprite;

	/// <summary>
	/// The cover sprte.
	/// </summary>
	public UISprite coverSprte;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Updates the HP.
	/// </summary>
	/// <param name="hp">Hp.</param>
	public void UpdateHP(float hp)
	{
		fillSprite.fillAmount = hp;
	}
}
