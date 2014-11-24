using UnityEngine;
using System.Collections;

/// <summary>
/// Character effect.
/// 
/// This class handle character's effect such Damage, Cat Coine eaten, Fish Bone eaten, Cat Cookie eaten
/// </summary>
public class CharacterEffect : MonoBehaviour 
{
	/// <summary>
	/// The prefab effect when character get hit
	/// </summary>
	public GameObject damageEffectPrefab;
	
	/// <summary>
	/// The coin eaten effect prefab when character eat coin.
	/// </summary>
	public GameObject coinEatenEffectPrefab;
	
	/// <summary>
	/// The fish bone eaten effect prefab when character eat fish bone.
	/// </summary>
	public GameObject fishBoneEatenEffectPrefab;

	/// <summary>
	/// The cat cookie eaten effect prefab when character eat cat cookie.
	/// </summary>
	public GameObject catCookieEatenEffectPrefab;

	/// <summary>
	/// The damage effect gameobject.
	/// Hold damage effect
	/// </summary>
	private GameObject damageEffect;
	
	/// <summary>
	/// The coin effect when character eat coin.
	/// hold coin eaten effect
	/// </summary>
	private GameObject coinEffect;
	
	/// <summary>
	/// The fish bone effect when character eat fish bone.
	/// hold fish bone eaten effect
	/// </summary>
	private GameObject fishBoneEffect;

	/// <summary>
	/// The cat cookie effect when character eat cat cookie.
	/// hold cat cookie eaten effect
	/// </summary>
	private GameObject catCookieEffect;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Creates any effects bound to player.
	/// </summary>
	public void CreateEffects()
	{
		GameObject tempEffect;
		
		//create hit effect
		if((damageEffectPrefab != null) && (damageEffect == null))
		{
			//tempEffect = Instantiate(damageEffectPrefab) as GameObject;
			tempEffect = TrashMan.spawn(damageEffectPrefab);
			
			tempEffect.name = damageEffectPrefab.name;
			
			//make it as child
			tempEffect.transform.parent = transform;
			
			//center effect 
			tempEffect.transform.localPosition = new Vector3(0f, 0f, 0f);
			
			damageEffect = tempEffect;
		}
		
		//create coin eaten effect
		if((coinEatenEffectPrefab != null) && (coinEffect == null))
		{
			//tempEffect = Instantiate(coinEatenEffectPrefab) as GameObject;

			tempEffect = TrashMan.spawn(coinEatenEffectPrefab);
			
			tempEffect.name = coinEatenEffectPrefab.name;
			
			tempEffect.transform.parent = transform;
			
			tempEffect.transform.localPosition = new Vector3(0f, 0f, 0f);
			
			coinEffect = tempEffect;
		}
		
		//create fish bone eaten effect
		if((fishBoneEatenEffectPrefab != null) && (fishBoneEffect == null))
		{
			//tempEffect = Instantiate(fishBoneEatenEffectPrefab) as GameObject;
			tempEffect = TrashMan.spawn(fishBoneEatenEffectPrefab);
			
			tempEffect.name = fishBoneEatenEffectPrefab.name;
			
			tempEffect.transform.parent = transform;
			
			tempEffect.transform.localPosition = new Vector3(0f, 0f, 0f);
			
			fishBoneEffect = tempEffect;
		}

		//create cat cookie eaten effect
		if((catCookieEatenEffectPrefab != null) && (catCookieEffect == null))
		{
			//tempEffect  = Instantiate(catCookieEatenEffectPrefab) as GameObject;
			tempEffect = TrashMan.spawn(catCookieEatenEffectPrefab);

			tempEffect.name = catCookieEatenEffectPrefab.name;

			tempEffect.transform.parent = transform;

			tempEffect.transform.localPosition = new Vector3(0f, 0f, 0f);

			catCookieEffect = tempEffect;
		}
	}

	/// <summary>
	/// Stops all effects.
	/// </summary>
	public void StopAllEffects()
	{
		//check if any effects exist.... stop it
		if(transform.childCount > 0)
		{
			for(int i=0; i<transform.childCount; i++)
			{
				GameObject child = transform.GetChild(i).gameObject;

				if(child.activeInHierarchy)
				{
					if(child.GetComponent<EffectAnimation>() != null)
					{
						//tell ability to remove
						child.GetComponent<EffectAnimation>().StopAnimation();
					}
				}

			}
		}
	}

	public void PlayDamageEffect()
	{
		if(damageEffect != null)
		{
			damageEffect.GetComponent<EffectAnimation>().PlayAnimation();
		}
		else
		{
			Debug.LogError("Character's damage effect was not assign");
		}
	}

	public void PlayCoinEatenEffect()
	{
		if(coinEffect != null)
		{
			coinEffect.GetComponent<EffectAnimation>().PlayAnimation();
		}
		else
		{
			Debug.LogError("Character's coin eaten effect was not assign");
		}
	}
	
	public void PlayFishBoneEatenEffect()
	{
		if(fishBoneEffect != null)
		{
			fishBoneEffect.GetComponent<EffectAnimation>().PlayAnimation();
		}
		else
		{
			Debug.LogError("Character's fish bone eaten effect was not assign");
		}
	}

	public void PlayCatCookieEatenEffect()
	{
		if(catCookieEffect != null)
		{
			catCookieEffect.GetComponent<EffectAnimation>().PlayAnimation();
		}
		else
		{
			Debug.LogError("Character's cat cookie eaten effect was not assign");
		}
	}
}
