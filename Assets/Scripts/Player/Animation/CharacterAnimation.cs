using UnityEngine;
using System.Collections;

/// <summary>
/// Character animation.
/// This class control character animation
/// </summary>

public class CharacterAnimation : MonoBehaviour 
{
	/// <summary>
	/// Animator that control character animation
	/// </summary>
	protected Animator anim;

	//Animtion parameters hash id
	protected int normal_bool;
	protected int moving_bool;
	protected int dead_bool;
	protected int victory_bool;
	protected int onAttack_bool;
	
	protected virtual void Awake()
	{
		//find animator component
		anim = GetComponent<Animator> ();

		//init animation
		InitAnimation (anim);
	}

	/// <summary>
	/// Initialize animation
	/// Subclass must implement.
	/// </summary>
	/// <param name="anim">Animation.</param>
	protected virtual void InitAnimation(Animator anim)
	{
		//convert animation's parameters to hash id
		normal_bool = Animator.StringToHash ("Normal");
		moving_bool = Animator.StringToHash("Moving");
		dead_bool = Animator.StringToHash("Dead");
		victory_bool = Animator.StringToHash("Victory");
		onAttack_bool = Animator.StringToHash("OnAttack");
	}

	/// <summary>
	/// Character anim for falling
	/// This animation is character's normal
	/// which keep falling down
	/// </summary>
	public virtual void NormalAnim()
	{

	}

	/// <summary>
	/// Character anim for moving
	/// </summary>
	public virtual void MovingAnim()
	{
	}

	/// <summary>
	/// When character victory
	/// </summary>
	public virtual void VictoryAnim()
	{
	}

	/// <summary>
	/// Character anim for dead
	/// </summary>
	public virtual void Dead()
	{
	}

	/// <summary>
	/// Character anim for character get attack
	/// </summary>
	public virtual void OnAttack()
	{
	}

	/// <summary>
	/// Reset animation.
	/// </summary>
	public virtual void Reset()
	{

	}
}
