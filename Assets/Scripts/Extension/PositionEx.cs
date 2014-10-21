using UnityEngine;
using System.Collections;

public class PositionEx
{

	/// <summary>
	/// Fixs the destination.
	/// Take destination and sprite to calculate proper destinaiton
	/// This allow you to place spawner just out side of camera and don't
	/// have to place percisely.
	/// </summary>
	/// <returns>The fixed obstacle destination.</returns>
	/// <param name="cam">Cam.</param>
	/// <param name="spriteRender">Sprite render.</param>
	/// <param name="target">Destination.</param>
	/// <param name="offset">the offset that is out of screen</param>
	public static Vector2 FixDestination(Camera cam, Renderer spriteRender, Vector2 destination, float offset = 0f)
	{
		//calculate destination for obstacle
		//destination must out of screen involve obstacle bound
		
		Vector2 retDest = destination;
		
		//find camera bound
		float camLeftBound = cam.GetLeftBorderWorldSpace (spriteRender.transform.position.z);
		float camRightBound = cam.GetRightBorderWorldSpace (spriteRender.transform.position.z);
		float camTopBound = cam.GetTopBorderWorldSpace (spriteRender.transform.position.z);
		float camBottomBound = cam.GetBottomBorderWorldSpace (spriteRender.transform.position.z);

		//final position
		float fx, fy;
		
		if(destination.x < camLeftBound)
		{
			// camera's left
			fx = camLeftBound - spriteRender.bounds.extents.x - offset;
		}
		else if(destination.x > camRightBound)
		{
			//camera's right
			fx = camRightBound + spriteRender.bounds.extents.x + offset;
		}
		else
		{
			//unknow
			fx = retDest.x;
		}
		
		if(destination.y < camBottomBound)
		{
			//camera's bottom
			fy = camBottomBound - spriteRender.bounds.extents.y - offset;
		}
		else if(destination.y > camTopBound)
		{
			//camera's top
			fy = camTopBound + spriteRender.bounds.extents.y + offset;
		}
		else
		{
			//unknow
			fy = retDest.y;
		}

		//set final destination
		retDest = new Vector2 (fx, fy);
		
		return retDest;
	}
	
	/// <summary>
	/// Fixs the spawn position.
	/// Take position and sprite to calculate proper destinaiton
	/// This allow you to place spawner just slightly out side of camera and don't
	/// have to place percisely.
	/// </summary>
	/// <returns>The fixed obstacle position.</returns>
	/// <param name="cam">Cam.</param>
	/// <param name="spriteRender">Sprite render.</param>
	/// <param name="target">Sapwn Pos.</param>
	public static Vector2 FixSpawnPosition(Camera cam, Renderer spriteRender, Vector2 spawnPos, float offset = 0f)
	{
		return FixDestination (cam, spriteRender, spawnPos, offset);
	}
}
