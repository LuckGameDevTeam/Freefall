using UnityEngine;
using System.Collections;

/// <summary>
/// Cat shield ability.
/// This class is subclass of Ability.
/// This ability allow character to be hit by obstacle for maximum twice
/// </summary>
public class CatShieldAbility :  Ability
{
	/// <summary>
	/// The cat shield clip.
	/// </summary>
	public AudioClip catShieldClip;

	/// <summary>
	/// How many time this shield can block
	/// </summary>
	public int maxShieldBlockCount = 2;

	/// <summary>
	/// The current shield block count.
	/// </summary>
	private int currentShieldBlockCount = 0;

	/// <summary>
	/// Reference to collision
	/// </summary>
	private CircleCollider2D collision;

	protected override void Awake ()
	{
		base.Awake ();

		collision = GetComponent<CircleCollider2D> ();
	}

	protected override void Start ()
	{
		base.Start ();


	}

	protected override void OnEnable()
	{
		base.OnEnable ();

		currentShieldBlockCount = maxShieldBlockCount;
	}

	protected override void OnDisable()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == Tags.obstacle)
		{
			//if collide object is not boss monster
			if((other.gameObject.GetComponent<Obstacle>().monsterType != MonsterTypes.Boss))
			{
				//make monster object dead
				other.gameObject.GetComponent<Obstacle>().isDead = true;

				//decrease block count
				currentShieldBlockCount--;
			}
			else
			{
				//decrease block count
				currentShieldBlockCount--;
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


			collision.radius = maxBound;

			//make character invulnerable
			character.GetComponent<CharacterHealth>().invulnerable = true;
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" can not find character's renderer");
		}

		//play cat shield clip
		if(catShieldClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.sfxClip = catShieldClip;
				soundPlayer.loop = true;
				soundPlayer.PlaySound();
			}
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" unable to play cat shield clip, cat shield clip not assigned");
		}
	}
	
	protected override void RemoveAbility()
	{
		//make character vulnerable
		character.GetComponent<CharacterHealth>().invulnerable = false;

		//stop sound
		if(catShieldClip != null)
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

		//stop sound
		if(catShieldClip != null)
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

		//remove ability if block count is 0
		if(currentShieldBlockCount <= 0)
		{
			RemoveAbility();
		}

	}
}
