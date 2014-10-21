using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Scrollable background.
/// 
/// To use this background, first create empty gameobject that will
/// contain multiple sprites that is going to be scrolling. Attach
/// This script to gameobject.
/// 
/// Add sprite as child of gameobject. Each sprites must have different y and in order,
/// script will auto align each sprites on x and y, in addition first sprtie will have same x,y as parent gameobject. 
/// 
/// </summary>

public class ScrollableBackground : MonoBehaviour 
{

	/// <summary>
	/// Move speed on y axis
	/// </summary>
	public float speed = 0f;

	/// <summary>
	/// Change this value to stop/start scrolling
	/// </summary>
	public bool isScrolling = true;

	/// <summary>
	/// Indicate background is slowing down
	/// Value will reverse back to false from true when
	/// background totally stop
	/// </summary>
	public bool isSlowingDown = false;

	/// <summary>
	/// The slow down constant speed.
	/// 
	/// </summary>
	public float slowDownSpeed = 2f;

	/// <summary>
	/// The current slow down speed.
	/// Once it reach the scrolling speed
	/// background will stop scrolling
	/// </summary>
	private float currentSlowDownSpeed = 0f;


	/// <summary>
	/// Sprites that will be scroll
	/// </summary>
	private List<Transform> backgroundParts;

	void Start()
	{
		//Check if we have child
		if (transform.childCount == 0) 
		{
			Debug.LogError("Scroll background has no child attached");
		}

		//Create list if needed
		if(backgroundParts == null)
			backgroundParts = new List<Transform>();

		//Add all child sprties to list
		for(int i=0; i<transform.childCount; i++)
		{

			Transform child = transform.GetChild(i);

			//If child has renderer attached  
			if(child.renderer != null)
			{
				//Add child
				backgroundParts.Add(child);
			}
		}

		//sort sprites by y
		SortSpritesByY (backgroundParts);


		//Make sprites place in order 
		for(int i=0; i<backgroundParts.Count; i++)
		{
			//get current child
			Transform child = backgroundParts[i];

			//if this is first child in list, make child's x and y as same as it's parent.
			//keep z axis
			if(i == 0)
			{
				child.position = new Vector3(transform.position.x, transform.position.y, child.position.z);

				continue;
			}

			//get previous child, so we can reference from it's position
			Transform previousChild = backgroundParts[i-1];

			//get previous child's half bound
			float pChildYHalfBound = previousChild.renderer.bounds.extents.y;

			//get current child's half bound
			float childYHalfBound = child.renderer.bounds.extents.y;

			//align x axis to previous child, substract previous child and current childs' half bound from previous child's y position
			//as well as make z axis as same as previous child
			child.position = new Vector3(previousChild.position.x, previousChild.position.y - (pChildYHalfBound + childYHalfBound), previousChild.position.z);
		}

		//debug
		/*
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		
		for(int i=0 ; i < planes.Length; i++) 
		{
			GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
			p.name = "Plane " + i.ToString();
			p.transform.position = -planes[i].normal * planes[i].distance;
			p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
		}
		*/

	}

	void Update()
	{
		//if we can't scroll...return
		if(!isScrolling)
			return;
						
		//move y 
		float movement = speed * Time.deltaTime;

		//if slow down enable
		if (isSlowingDown) 
		{
			//increase current slow down speed
			currentSlowDownSpeed += (slowDownSpeed * Time.deltaTime);

			//add slow down to movement speed
			movement = movement - (currentSlowDownSpeed * Time.deltaTime);

			//check if current slow down speed greater or equal than scrolling speed...stop scrolling, disable slow down
			if(currentSlowDownSpeed >= speed)
			{
				isScrolling = false;

				isSlowingDown = false;

				currentSlowDownSpeed = 0f;
			}

			//make sure movement is not lower than 0
			if(movement <= 0f)
			{
				movement = 0f;
			}
		}

		//scroll background
		transform.Translate (new Vector3(0f, movement, 0f));

		Transform firstChild = backgroundParts [0];

		if(firstChild != null)
		{
			if(firstChild.position.y > Camera.main.transform.position.y)
			{

				//Check if child out of camera
				if(firstChild.renderer.IsVisibleFromCamera(Camera.main) == false)
				{

					//get last child
					Transform lastChild = backgroundParts[backgroundParts.Count-1];

					//get last child position
					Vector3 lastPosition = lastChild.position;

					//get last child size
					Vector3 lastSize = lastChild.renderer.bounds.max - lastChild.renderer.bounds.min;

					//set first child position after last chlild
					firstChild.position = new Vector3(firstChild.position.x, lastPosition.y - lastSize.y, firstChild.position.z);

					//put first child to last in list
					backgroundParts.Remove(firstChild);
					backgroundParts.Add(firstChild);

				}
			}
		}
	}

	/// <summary>
	/// Stop scrolling.
	/// </summary>
	public void StopScrolling()
	{
		isScrolling = false;
	}

	/// <summary>
	/// Start scrolling.
	/// </summary>
	public void StartScrolling()
	{
		currentSlowDownSpeed = 0f;

		isSlowingDown = false;

		isScrolling = true;
	}

	/// <summary>
	/// Start to slow down scrolling.
	/// </summary>
	public void StartSlowDown()
	{

		currentSlowDownSpeed = 0f;

		isSlowingDown = true;
	}

	/// <summary>
	/// Check if sprite is visible by camera 
	/// </summary>
	bool IsVisibleFromCamera(Renderer renderer, Camera camera)
	{
		Plane[] plane= GeometryUtility.CalculateFrustumPlanes(camera);

		bool result = GeometryUtility.TestPlanesAABB (plane, renderer.bounds);

		//Debug.Log (result);

		return result;
	}

	/// <summary>
	/// Bubble sort for background sprites
	/// It use y axis to determine which one come first or last
	/// </summary>
	/// <param name="backgroundSprites">Background sprites.</param>
	void SortSpritesByY(List<Transform> backgroundSprites)
	{
		for(int i=0; i<backgroundParts.Count-1; i++)
		{
			Transform child = backgroundParts[i];
			Transform nextChild = backgroundParts[i+1];

			if(child.position.y > nextChild.position.y)
			{
				//Transform temp = child;
				backgroundParts[i] = backgroundParts[i+1];
				backgroundParts[i+1] = child;
			}
		}
	}
}
