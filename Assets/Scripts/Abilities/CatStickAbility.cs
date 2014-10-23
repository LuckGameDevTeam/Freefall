using UnityEngine;
using System.Collections;

/// <summary>
/// Cat stick ability.
/// This class is subclass of Ability
/// This ability make all obstacles that are hitting character
/// bounce away from character for 5 seconds except boss obstacle.
/// </summary>
public class CatStickAbility : Ability 
{
	/// <summary>
	/// The cat stick clip.
	/// </summary>
	public AudioClip catStickClip;

	/// <summary>
	/// How much bounce force should apply to contact obstacle
	/// </summary>
	public float bounceForce = 5f;

	/// <summary>
	/// reference to collision.
	/// </summary>
	private CircleCollider2D collision;

	protected override void Awake ()
	{
		base.Awake ();

		collision = GetComponent<CircleCollider2D> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == Tags.obstacle)
		{
			Obstacle o = other.gameObject.GetComponent<Obstacle>();

			//if object is not boss monster
			if(o.monsterType != MonsterTypes.Boss)
			{
				CharacterControl cc = character.GetComponent<CharacterControl>();

				//make it bounce away from character if character moving velocity is not zero
				if(cc.MovingVelocity != Vector2.zero)
				{
					//set bounce force
					o.BounceForce = bounceForce;

					//use character's moving velocity as bounce direction
					o.bounceDirection = cc.MovingVelocity.normalized;
				}
				else
				{
					//set bounce force
					o.BounceForce = bounceForce;

					//use monster object it self moving velocity as direction but opposite of that direction
					o.bounceDirection = o.MovingVelocity.normalized * -1f;
				}
			}

		}
	}

	public override void ActiveAbility(GameObject owner)
	{
		base.ActiveAbility (owner);

		Renderer cRenderer = character.renderer;

		//if character has renderer... set the collision radius to max of renderer's bound
		//and a bit extend
		if(cRenderer != null)
		{
			float maxBound = 0f;
			
			if(cRenderer.bounds.extents.x >= cRenderer.bounds.extents.y)
			{
				maxBound = cRenderer.bounds.extents.x + (cRenderer.bounds.extents.x/3);
			}
			else
			{
				maxBound = cRenderer.bounds.extents.y + (cRenderer.bounds.extents.y/3);
			}
			
			collision.radius = maxBound;
			
			//make character invulnerable
			character.GetComponent<CharacterHealth>().invulnerable = true;
		}
		else
		{
			Debug.LogError(gameObject.name+" can not find character's renderer");
		}

		//play cat stick clip
		if(catStickClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.sfxClip = catStickClip;
				soundPlayer.loop = true;
				soundPlayer.PlaySound();
			}
		}
		else
		{
			Debug.LogError(gameObject.name+" unable to play cat stick clip, cat stick clip not assigned");
		}
	}
	
	protected override void RemoveAbility()
	{
		//make character vulnerable
		character.GetComponent<CharacterHealth>().invulnerable = false;

		//make all obstacles destroyable when contact with player
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (Tags.obstacle);
		
		for(int i=0; i<obstacles.Length; i++)
		{
			//set all monster object can be destroyed
			obstacles[i].GetComponent<Obstacle>().canDestroy = true;
		}

		//stop sound
		if(catStickClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}

		base.RemoveAbility ();
		
	}

	public override void RemoveAbilityImmediately ()
	{
		//make character vulnerable
		character.GetComponent<CharacterHealth>().invulnerable = false;
		
		//make all obstacles destroyable when contact with player
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (Tags.obstacle);
		
		for(int i=0; i<obstacles.Length; i++)
		{
			//set all monster object can be destroyed
			obstacles[i].GetComponent<Obstacle>().canDestroy = true;
		}

		//stop sound
		if(catStickClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.StopSound();
			}
		}
		
		base.RemoveAbilityImmediately ();
	}
	
	protected override void ProcessAbility()
	{
		base.ProcessAbility ();

		//make all obstacles not destroyable when contact with player
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag (Tags.obstacle);

		if(obstacles.Length > 0)
		{
			for(int i=0; i<obstacles.Length; i++)
			{
				//set all monster object can not be destroyed
				obstacles[i].GetComponent<Obstacle>().canDestroy = false;
			}
		}


	}
}
