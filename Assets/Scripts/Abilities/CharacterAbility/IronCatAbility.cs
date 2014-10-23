using UnityEngine;
using System.Collections;

/// <summary>
/// Iron cat ability.
/// 
/// This class is subclass of CatSwordAbility
/// The ability of this character is as same as CatSwordAbility
/// </summary>
public class IronCatAbility : CatSwordAbility 
{
	/// <summary>
	/// The ability clip.
	/// </summary>
	public AudioClip abilityClip;

	public override void ActiveAbility(GameObject owner)
	{
		if(abilityClip != null)
		{
			AudioSource.PlayClipAtPoint(abilityClip, transform.position);
		}

		base.ActiveAbility (owner);
	}
}
