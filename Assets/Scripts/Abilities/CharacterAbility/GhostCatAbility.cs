using UnityEngine;
using System.Collections;

/// <summary>
/// Ghost cat ability.
/// 
/// This class is subclass of Ability
/// </summary>
public class GhostCatAbility : Ability 
{
	/// <summary>
	/// The fade density.
	/// </summary>
	public float fadeDensity = 0.3f;

	/// <summary>
	/// The fade in speed.
	/// </summary>
	public float fadeInSpeed = 5.0f;

	/// <summary>
	/// The fade out speed.
	/// </summary>
	public float fadeOutSpeed = 5.0f;

	/// <summary>
	/// The fade in margin
	/// 
	/// The result will be fadeDensity+fadeInMargin
	/// 
	/// Current alpha under than result value will stop fade in
	/// </summary>
	public float fadeInMargin = 0.2f;

	/// <summary>
	/// The fade out margin.
	/// 
	/// The result will be 1.0-fadeOutMargin
	/// 
	/// Current alpha higher than result value will stop fade out
	/// </summary>
	public float fadeOutMargin = 0.2f;

	/// <summary>
	/// Reference to character material
	/// </summary>
	private Material mat;

	protected override void OnDisable ()
	{
		base.OnDisable ();

		//start fade in process
		StopCoroutine ("FadeIn");

		//start fade out process
		StopCoroutine ("FadeOut");
	}

	public override void ActiveAbility(GameObject owner)
	{
		//////////copy from parent and modify/////////////
		character = owner;
		
		//make ability as child of owner
		transform.parent = owner.transform;
		
		transform.localPosition = new Vector3(0f, 0f, -1f);
		
		isAbilityActive = false;
		
		if(Evt_AbilityStart != null)
		{
			Evt_AbilityStart(this);
		}
		//////////copy from parent and modify/////////////

		//get character's material
		mat = character.GetComponent<SpriteRenderer> ().materials [0];

		//disable character collider
		//character.GetComponent<Collider2D> ().enabled = false;

		//disable character's collision component
		character.GetComponent<CharacterControl> ().CollisionSetting (false);

		//character.GetComponent<CharacterControl> ().BounceSetting (false);

		//set fade in margin
		fadeInMargin = fadeDensity + fadeInMargin;

		//set fade out margin
		fadeOutMargin = 1.0f - fadeOutMargin;

		//start fade in
		StartCoroutine("FadeIn");
	}
	
	protected override void RemoveAbility()
	{
		//stop ability it's own effect
		StopAbilityEffect ();

		//set ability active to false
		isAbilityActive = false;

		//start fade out
		StartCoroutine("FadeOut");

	}

	public override void RemoveAbilityImmediately()
	{
		base.RemoveAbilityImmediately ();

		//set alpha back to original alpha
		mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f);

		//enable character collider
		//character.GetComponent<Collider2D> ().enabled = true;

		//enable character's collision component
		character.GetComponent<CharacterControl> ().CollisionSetting (true);

		//character.GetComponent<CharacterControl> ().BounceSetting (true);

	}
	
	protected override void ProcessAbility()
	{
		base.ProcessAbility ();

	}

	IEnumerator FadeIn()
	{
		while(true)
		{
			//check if alpha is lower or equal than fade in margin
			if(mat.color.a <= fadeInMargin)
			{
				break;
			}

			//calculate current fade in alpha value
			Color curC = Color.Lerp(mat.color, new Color(mat.color.r, mat.color.g, mat.color.b, fadeDensity), fadeInSpeed * Time.deltaTime);

			//assing current alpha color to material
			mat.color = curC;

			//wait for one frame
			yield return null;
		}

		//set material color to defined color (alpha)
		mat.color = new Color (mat.color.r, mat.color.g, mat.color.b, fadeDensity);

		//set ability active to true
		isAbilityActive = true;

		//play ability effect
		StartAbilityEffect ();
	}

	IEnumerator FadeOut()
	{
		while(true)
		{
			//check if alpha is greater or equal than fade out margin
			if(mat.color.a >= fadeOutMargin)
			{
				break;
			}

			//calculate current fade out alpha value
			Color curC = Color.Lerp(mat.color, new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f), fadeOutSpeed * Time.deltaTime);

			//assing current alpha color to material
			mat.color = curC;

			//wait for one frame
			yield return null;
		}

		//set material color to defined color (alpha)
		mat.color = new Color(mat.color. r, mat.color.g, mat.color.b, 1.0f);

		//////////copy from parent//////////////
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
		//////////copy from parent//////////////


		///enable character collider
		//character.GetComponent<Collider2D> ().enabled = true;

		//enable character collision component
		character.GetComponent<CharacterControl> ().CollisionSetting (true);
		//character.GetComponent<CharacterControl> ().BounceSetting (true);

	}
}
