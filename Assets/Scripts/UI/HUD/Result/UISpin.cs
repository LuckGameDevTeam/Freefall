using UnityEngine;
using System.Collections;

/// <summary>
/// UI spin.
/// 
/// This class used to spin UI object
/// This implementation is copy and modify from NGUI Spin.cs
/// </summary>
public class UISpin : MonoBehaviour 
{

	public Vector3 rotationsPerSecond = new Vector3(0f, 0.1f, 0f);
	public bool ignoreTimeScale = true;
	
	Rigidbody mRb;
	Transform mTrans;
	
	void Start ()
	{
		mTrans = transform;
		mRb = rigidbody;
	}
	
	void Update ()
	{
		if (mRb == null)
		{
			ApplyDelta(ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime);
		}
	}
	
	void FixedUpdate ()
	{
		if (mRb != null)
		{
			ApplyDelta(Time.deltaTime);
		}
	}
	
	public void ApplyDelta (float delta)
	{
		delta *= Mathf.Rad2Deg * Mathf.PI * 2f;
		Quaternion offset = Quaternion.Euler(rotationsPerSecond * delta);
		
		if (mRb == null)
		{
			mTrans.rotation = mTrans.rotation * offset;
		}
		else
		{
			mRb.MoveRotation(mRb.rotation * offset);
		}
	}
}
