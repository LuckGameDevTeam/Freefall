using UnityEngine;
using System.Collections;

public class CatCookieMovable : CatCookie 
{
	/// <summary>
	/// The move speed.
	/// </summary>
	public float moveSpeed = 1.0f;

	/// <summary>
	/// The destination to move to.
	/// </summary>
	[System.NonSerialized]
	public Vector2 dest;

	protected override void Update()
	{
		if(renderer.IsVisibleFromCamera(Camera.main))
		{
			MoveCookie();
		}
		else
		{
			//GameController.sharedGameController.objectPool.RecycleObject(gameObject);
			TrashMan.despawn(gameObject);
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Tags.player)
		{
			//add hp to character 
			GameController.sharedGameController.character.GetComponent<CharacterHealth>().AddHP(increaseHP);
			
			//tell character play cat cookie eaten effect
			GameController.sharedGameController.character.GetComponent<CharacterEffect>().PlayCatCookieEatenEffect();

			//recycle gameobject
			//GameController.sharedGameController.objectPool.RecycleObject(gameObject);
			TrashMan.despawn(gameObject);
		}
	}

	void MoveCookie()
	{
		//calculate direction from obstacle to destination
		Vector2 direction = (dest - transform.ConvertPositionToVector2 ()).normalized;
		
		//calculate amout of movement
		Vector2 amount = direction * (moveSpeed * Time.deltaTime);
		
		//move obstacle
		transform.position = new Vector3 (transform.position.x + amount.x, transform.position.y + amount.y, transform.position.z);
	}
}
