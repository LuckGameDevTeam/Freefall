using UnityEngine;
using System.Collections;

/// <summary>
/// Camera ex.
/// This is extension class for Camera
/// </summary>

public static class CameraEx 
{
	/// <summary>
	/// Gets the left border of camera in world space.
	/// Return value is on x axis.
	/// </summary>
	/// <returns>The camera left border world space.</returns>
	/// <param name="camera">Camera.</param>
	/// <param name="zAxis">Z axis.</param>
	public static float GetLeftBorderWorldSpace(this Camera camera, float zAxis)
	{
		return camera.ViewportToWorldPoint (new Vector3 (0f, 0f, zAxis)).x;
	}

	/// <summary>
	/// Gets the right border of camera in world space.
	/// Return value is on x axis.
	/// </summary>
	/// <returns>The camera right border world space.</returns>
	/// <param name="camera">Camera.</param>
	/// <param name="zAxis">Z axis.</param>
	public static float GetRightBorderWorldSpace(this Camera camera, float zAxis)
	{
		return camera.ViewportToWorldPoint (new Vector3 (1f, 0f, zAxis)).x;
	}

	/// <summary>
	/// Gets the top border of camera in world space.
	/// Return value is on y axis.
	/// </summary>
	/// <returns>The top border world space.</returns>
	/// <param name="camera">Camera.</param>
	/// <param name="zAxis">Z axis.</param>
	public static float GetTopBorderWorldSpace(this Camera camera, float zAxis)
	{
		return camera.ViewportToWorldPoint (new Vector3 (0f, 1f, zAxis)).y;
	}

	/// <summary>
	/// Gets the bottom border of camera in world space.
	/// Return value is on y axis
	/// </summary>
	/// <returns>The bottom border world space.</returns>
	/// <param name="camera">Camera.</param>
	/// <param name="zAxis">Z axis.</param>
	public static float GetBottomBorderWorldSpace(this Camera camera, float zAxis)
	{
		return camera.ViewportToWorldPoint (new Vector3 (0f, 0f, zAxis)).y;
	}
}
