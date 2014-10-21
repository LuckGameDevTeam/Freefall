using UnityEngine;
using System.Collections;

public class ExampleAnimation : CharacterAnimation 
{
	private int moveLeft_Hash_Bool;
	private int moveRight_Hash_Bool;
	private int normal_Hash_Bool;

	protected override void Awake()
	{
		base.Awake ();
	}

	protected override void InitAnimation(Animator anim)
	{
		base.InitAnimation (anim);

		moveLeft_Hash_Bool = Animator.StringToHash("MoveLeft");
		moveRight_Hash_Bool = Animator.StringToHash("MoveRight");
		normal_Hash_Bool = Animator.StringToHash("Normal");
	}

	public override void NormalAnim()
	{
		base.NormalAnim ();

		anim.SetBool (normal_Hash_Bool, true);
		anim.SetBool (moveLeft_Hash_Bool, false);
		anim.SetBool (moveRight_Hash_Bool, false);


	}

	
}
