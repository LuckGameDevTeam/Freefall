using UnityEngine;
using System.Collections;

/// <summary>
/// Transform ex.
/// This is extension class for Transform
/// </summary>

public static class TransformEx 
{

	/// <summary>
	/// Converts transform vector3 to vector2.
	/// </summary>
	/// <returns>The to vector2.</returns>
	/// <param name="transform">Transform.</param>
	public static Vector2 ConvertPositionToVector2(this Transform transform)
	{
		return new Vector2 (transform.position.x, transform.position.y);
	}
}
