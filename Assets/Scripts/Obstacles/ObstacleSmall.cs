using UnityEngine;
using System.Collections;

/// <summary>
/// Obstacle small.
/// 
/// This class is subclass of Obstacle.
/// 
/// This class is designed for small obstacle such as small monster.
/// </summary>
public class ObstacleSmall : Obstacle 
{
	public AudioClip appearClip;

	//on collision
	void OnTriggerEnter2D(Collider2D other)
	{

		if(other.tag == Tags.player)
		{
			Debug.Log("hit player");

			other.gameObject.SendMessageUpwards("TakeDamage", damage);

			isDead = true;
		}
	}

	protected override void BounceObstacle (Vector2 bounceDir)
	{
		base.BounceObstacle (bounceDir);
	}

	public override void InitObstacle()
	{
		if((appearClip != null) && (soundPlayer != null))
		{
			soundPlayer.sfxClip = appearClip;
			soundPlayer.PlaySound();
		}
	}
}
