using UnityEngine;
using System.Collections;

/// <summary>
/// Character health.
/// 
/// This class manage character's health.
/// </summary>
public class CharacterHealth : MonoBehaviour 
{
	/// <summary>
	/// Event for taking damage
	/// </summary>
	public delegate void EventTakeDamage(float damageAmount, float healthBefore, float healthAfter);
	public EventTakeDamage Evt_TakeDamage;

	public delegate void EventHealthChanged(float healthBefore, float healthAfter);
	public EventHealthChanged Evt_HealthChanged;

	/// <summary>
	/// Event for health total depleted
	/// </summary>
	public delegate void EventHealthDepleted();
	public EventHealthDepleted Evt_HealthDepleted;

	/// <summary>
	/// Character's health
	/// </summary>
	public float maxHealth = 5f;

	/// <summary>
	/// Whether health can take damage or not
	/// true health will not take damage
	/// </summary>
	public bool invulnerable = false;

	/// <summary>
	/// Characte's current health
	/// </summary>
	private float health = 5f;

	/// <summary>
	/// indicate health is totally depleted
	/// </summary>
	private bool healthDepleted = false;

	public void Start()
	{
		health = maxHealth;
	}

	/// <summary>
	/// Resets the health.
	/// </summary>
	public void ResetHealth()
	{
		float hBefore = health;

		health = maxHealth;

		if(Evt_HealthChanged != null)
		{
			Evt_HealthChanged(hBefore, health);
		}

		healthDepleted = false;
	}

	/// <summary>
	/// Adds the HP.
	/// </summary>
	/// <param name="hpToAdd">Hp to add.</param>
	public void AddHP(float hpToAdd)
	{
		if(health > 0)
		{
			if((health+hpToAdd) >= maxHealth)
			{
				float hBefore = health;

				health = maxHealth;

				if(Evt_HealthChanged != null)
				{
					Evt_HealthChanged(hBefore, health);
				}
			}
			else
			{
				float hBefore = health;

				health += hpToAdd;

				if(Evt_HealthChanged != null)
				{
					Evt_HealthChanged(hBefore, health);
				}
			}
		}
		else
		{
			Debug.LogWarning("Character HP is 0 can not add hp");
		}
	}

	/// <summary>
	/// Gets the current health.
	/// </summary>
	/// <returns>The current health.</returns>
	public float GetCurrentHealth()
	{
		return health;
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <param name="damage">Damage.</param>
	public void TakeDamage(float damage)
	{
		if(invulnerable || healthDepleted)
		{
			return;
		}

		if ((health - damage) > 0f)
		{
			float hBefore = health;
			float hAfter = health - damage;

			health -= damage;

			if(Evt_TakeDamage != null)
			{
				Evt_TakeDamage(damage, hBefore, hAfter);
			}

			if(Evt_HealthChanged != null)
			{
				Evt_HealthChanged( hBefore, hAfter);
			}
		}
		else
		{
			healthDepleted = true;

			if(Evt_HealthChanged != null)
			{
				Evt_HealthChanged( health, 0);
			}

			health = 0f;

			if(Evt_HealthDepleted != null)
			{
				Evt_HealthDepleted();
			}

		}

		//Debug.Log ("Health:" + health);
	}
}
