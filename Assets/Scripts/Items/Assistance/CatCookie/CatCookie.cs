using UnityEngine;
using System.Collections;

/// <summary>
/// Cat cookie.
/// 
/// This class is subclass of AssistantItem.
/// 
/// This is generic class for such Big Cat Cookie and Small Cat Cookie
/// </summary>
public class CatCookie : AssistantItem 
{

	/// <summary>
	/// How much hp can character recover
	/// </summary>
	public int increaseHP = 1;

	protected override void Awake()
	{
		base.Awake ();
	}
	
	protected override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D (other);
	}
}
