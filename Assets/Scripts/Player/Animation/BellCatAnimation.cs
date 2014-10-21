using UnityEngine;
using System.Collections;

public class BellCatAnimation : CharacterAnimation 
{


	protected override void Awake()
	{
		base.Awake ();
	}

	protected override void InitAnimation(Animator anim)
	{
		base.InitAnimation (anim);
	}

	public override void NormalAnim()
	{

		if (anim.GetCurrentAnimatorStateInfo (0).GetHashCode() == normal_bool) 
		{
			return;
		}

		//turn on normal anim
		anim.SetBool (normal_bool, true);

		//turn off other anim
		anim.SetBool (moving_bool, false);
		anim.SetBool (onAttack_bool, false);
		anim.SetBool (victory_bool, false);
		anim.SetBool (dead_bool, false);
	}


	public override void MovingAnim()
	{
		if (anim.GetCurrentAnimatorStateInfo (0).GetHashCode() == moving_bool) 
		{
			return;
		} 

		//turn on move left anim
		anim.SetBool (moving_bool, true);

		//turn off other anim
		anim.SetBool (normal_bool, false);
		anim.SetBool (onAttack_bool, false);
	}

	public override void VictoryAnim()
	{
		if (anim.GetCurrentAnimatorStateInfo (0).GetHashCode() == victory_bool) 
		{
			return;
		}

		//turn on victory anim
		anim.SetBool (victory_bool, true);

		//turn off other anim
		anim.SetBool (normal_bool, false);
		anim.SetBool (moving_bool, false);
		anim.SetBool (onAttack_bool, false);
		anim.SetBool (dead_bool, false);
	}

	public override void Dead()
	{
		if (anim.GetCurrentAnimatorStateInfo (0).GetHashCode() == dead_bool) 
		{
			return;
		}

		//turn on dead anim
		anim.SetBool (dead_bool, true);

		//turn off other anim
		anim.SetBool (normal_bool, false);
		anim.SetBool (moving_bool, false);
		anim.SetBool (onAttack_bool, false);
		anim.SetBool (victory_bool, false);
	}

	public override void OnAttack()
	{
		if (anim.GetCurrentAnimatorStateInfo (0).GetHashCode() == onAttack_bool) 
		{
			return;
		}

		//turn on dead anim
		anim.SetBool (onAttack_bool, true);

		//turn off other anim
		anim.SetBool (normal_bool, false);
		anim.SetBool (moving_bool, false);

	}

	public override void Reset()
	{
		anim.StopPlayback ();
		anim.Play (normal_bool);
	}
}
