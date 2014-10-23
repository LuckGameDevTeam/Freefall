using UnityEngine;
using System.Collections;

/// <summary>
/// Ability.
/// 
/// This is generic class for all abilities.
/// 
/// This class responsible for ability effect.
/// 
/// </summary>
public class Ability : MonoBehaviour 
{
	/// <summary>
	/// The event indicate ability already start
	/// </summary>
	/// <returns>The ability started.</returns>
	public delegate void EventAbilityStart(Ability ability);
	public EventAbilityStart Evt_AbilityStart;

	/// <summary>
	/// The event when ability remove from it's owner
	/// 
	/// Also indicate this ability is end
	/// </summary>
	public delegate void EventAbilityRemoved(Ability ability);
	public EventAbilityRemoved Evt_AbilityRemoved;

	/// <summary>
	/// The ability effect prefab.
	/// Assign prefab of effect this ability is going to play
	/// </summary>
	public GameObject abilityEffectPrefab;

	/// <summary>
	/// True then ability will not auto remove when time is reached
	/// you are responsible to call RemoveAbility(). Meanwhile this
	/// ability will keep processing until you call RemoveAbility() or
	/// new ability interrupt this one 
	/// 
	/// False then ability will auto remove when time is reached
	/// </summary>
	public bool hasAbilityTime = true;

	/// <summary>
	/// How long this ability live before
	/// destroyed.
	/// If value is 0 then ability will not have time to process before
	/// removed
	/// </summary>
	public float abilityTime = 3f;

	/// <summary>
	/// The character who own this ability.
	/// </summary>
	protected GameObject character;

	/// <summary>
	/// Indicate ability is active or not
	/// </summary>
	protected bool isAbilityActive = false;

	/// <summary>
	/// The ability effect.
	/// Hold the ability effect
	/// </summary>
	protected GameObject abilityEffect;

	/// <summary>
	/// The time ability about to be removed
	/// </summary>
	protected float removeTime = 0f;

	/// <summary>
	/// Reference to GameController
	/// </summary>
	//private GameController gameController;

	/// <summary>
	/// The sound player.
	/// </summary>
	protected SFXPlayer soundPlayer;


	protected virtual void Awake()
	{
		//find GameController
		//gameController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<GameController> ();

		//find SFXPlayer
		soundPlayer = GetComponent<SFXPlayer> ();

		if(soundPlayer == null)
		{
			soundPlayer = gameObject.AddComponent<SFXPlayer>();
		}
	}


	protected virtual void Start()
	{


	}

	protected virtual void OnEnable()
	{
		//set remove time
		removeTime = Time.time + abilityTime;

		//if ability was not created
		if(abilityEffect == null)
		{
			//create ability effect
			CreateAbilityEffect ();
		}
		else
		{
			//register animation finish event, effect can be reused
			abilityEffect.GetComponent<EffectAnimation>().Evt_AnimationFinished += EffectEnd;
		}
	}

	protected virtual void OnDisable()
	{
		removeTime = 0f;
	}

	protected virtual void Update()
	{
		//check if ability is active 
		if(isAbilityActive)
		{
			//check if this ability has time, not instance removeable
			if(hasAbilityTime)
			{
				//remove ability if time is reached...otherwise process ability
				if(Time.time >= removeTime)
				{
					RemoveAbility();
				}
				else
				{
					ProcessAbility();
				}
			}
			else
			{
				ProcessAbility();
			}
			
		}
	}

	/// <summary>
	/// Creates the ability effect.
	/// </summary>
	protected virtual void CreateAbilityEffect()
	{
		GameObject tempEffect;

		//create effect object from prefab
		if((abilityEffectPrefab != null) && (abilityEffect == null))
		{
			tempEffect = Instantiate(abilityEffectPrefab) as GameObject;
			
			tempEffect.name = abilityEffectPrefab.name;

			//attach effect object as child
			tempEffect.transform.parent = transform;

			//reset effect object's local position to 0
			tempEffect.transform.localPosition = new Vector3(0f, 0f, 0f);

			//register animation finish event
			tempEffect.GetComponent<EffectAnimation>().Evt_AnimationFinished += EffectEnd;
			
			abilityEffect = tempEffect;
		}
	}

	/// <summary>
	/// Starts the ability effect.
	/// </summary>
	protected virtual void StartAbilityEffect()
	{
		if(abilityEffect != null)
		{
			//play effect
			abilityEffect.GetComponent<EffectAnimation>().PlayAnimation();
		}
	}

	/// <summary>
	/// Stops the ability effect.
	/// </summary>
	protected virtual void StopAbilityEffect()
	{
		if(abilityEffect != null)
		{
			//stop effect
			abilityEffect.GetComponent<EffectAnimation>().StopAnimation();
		}

	}
	
	/// <summary>
	/// Actives the ability.
	/// </summary>
	public virtual void ActiveAbility(GameObject owner)
	{
		//set this ability's owner
		character = owner;

		//make ability as child of owner
		transform.parent = owner.transform;

		//set local position to 0,0,-1-->so infront of character
		transform.localPosition = new Vector3(0f, 0f, -1f);

		isAbilityActive = true;

		if(Evt_AbilityStart != null)
		{
			Evt_AbilityStart(this);
		}

		//start effect
		StartAbilityEffect ();
	}

	/// <summary>
	/// Removes the ability from it's owner.
	/// Destroy ability
	/// 
	/// This is called when ability is actually end
	/// </summary>
	protected virtual void RemoveAbility()
	{
		//stop effect
		StopAbilityEffect ();

		isAbilityActive = false;

		if (abilityEffect != null) 
		{
			//unregister event
			abilityEffect.GetComponent<EffectAnimation>().Evt_AnimationFinished -= EffectEnd;
		}

		if(transform.parent)
		{
			//remove ability from owner
			transform.parent = null;
		}

		if(Evt_AbilityRemoved != null)
		{
			Evt_AbilityRemoved(this);
		}

		//GameObject.Destroy (gameObject);

		//recycle ability, so ability will not be instantiate again
		GameController.sharedGameController.objectPool.RecycleObject (gameObject);
	}

	/// <summary>
	/// Removes the ability immediately.
	/// 
	/// Kill ability right away.
	/// 
	/// This might be call during the ability.
	/// 
	/// Subclass have to do everything within this frame
	/// 
	/// code here is as same RemoveAbility
	/// </summary>
	public virtual void RemoveAbilityImmediately()
	{
		StopAbilityEffect ();
		
		isAbilityActive = false;

		if (abilityEffect != null) 
		{
			//unregister event
			abilityEffect.GetComponent<EffectAnimation>().Evt_AnimationFinished -= EffectEnd;
		}

		if(transform.parent)
		{
			transform.parent = null;
		}

		if(Evt_AbilityRemoved != null)
		{
			Evt_AbilityRemoved(this);
		}
		
		//GameObject.Destroy (gameObject);
		GameController.sharedGameController.objectPool.RecycleObject (gameObject);
	}

	/// <summary>
	/// Processes the ability.
	/// </summary>
	protected virtual void ProcessAbility()
	{

	}

	/// <summary>
	/// Indicate effect is finished
	/// 
	/// override subclass can do anything when effect finished playing
	/// </summary>
	protected virtual void EffectFinished()
	{

	}

	protected virtual void EffectEnd()
	{
		EffectFinished ();
	}
}
