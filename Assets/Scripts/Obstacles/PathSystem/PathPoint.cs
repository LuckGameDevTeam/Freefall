using UnityEngine;
using System.Collections;

/// <summary>
/// Path point.
/// 
/// This class must attacted to gameobject that represent
/// path point.
/// 
/// Each path point must chain together to work
/// </summary>
public class PathPoint : MonoBehaviour 
{
	/// <summary>
	/// The path prefix.
	/// 
	/// Must be unique between path, but same prefix for PathPoint
	/// in same path 
	/// </summary>
	public string PathPrefix;

	/// <summary>
	/// The begin wait.
	/// 
	/// How long is it going to wait unit path moving
	/// </summary>
	public float beginWait = 0f;

	/// <summary>
	/// The last point.
	/// 
	/// if this is null, mean this is the first point in path
	/// </summary>
	[System.NonSerialized]
	public PathPoint lastPoint;

	/// <summary>
	/// The next point in path.
	/// 
	/// if this is null, mean this is last point in path
	/// </summary>
	public PathPoint nextPoint;

	void OnEnable()
	{
		//Auto assign this point as last point to next point as long as it
		//has next point
		if(nextPoint != null)
		{
			nextPoint.lastPoint = this;
		}
	}

	/// <summary>
	/// Gets the position.
	/// </summary>
	/// <returns>The position.</returns>
	public Vector3 GetPosition()
	{
		return transform.position;
	}

	/// <summary>
	/// Gets the position in 2d.
	/// </summary>
	/// <returns>The position2 d.</returns>
	public Vector2 GetPosition2D()
	{
		return new Vector2 (transform.position.x, transform.position.y);
	}
}
