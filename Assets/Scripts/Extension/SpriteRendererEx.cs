using UnityEngine;
using System.Collections;

/// <summary>
/// Sprite renderer ex.
/// This is extension for SpriteRender
/// </summary>

public static class SpriteRendererEx
{

	/// <summary>
	/// Check if sprite is visible by camera 
	/// </summary>
	public static bool IsVisibleFromCamera(this Renderer renderer, Camera camera)
	{
		Plane[] plane= GeometryUtility.CalculateFrustumPlanes(camera);
		
		bool result = GeometryUtility.TestPlanesAABB (plane, renderer.bounds);
		
		//Debug.Log (result);
		
		return result;
	}
}
