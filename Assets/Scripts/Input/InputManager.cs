using UnityEngine;
using System.Collections;

/// <summary>
/// Input manager.
/// 
/// This class handle player input
/// 
/// Support  mobile and editor
/// </summary>
public class InputManager : MonoBehaviour 
{
	/// <summary>
	/// Whether this input manager is enable or not
	/// false InputManager will not send out any input event
	/// </summary>
	public bool inputManagerEnabled = true;

	/// <summary>
	/// Event for first touch down
	/// Notice this is not holding
	/// 
	/// <param name="position">The position of current touch.</param>
	/// </summary>
	public delegate void EventTouchDown(InputManager inputMgr, Vector2 position);
	public EventTouchDown Evt_TouchDown;

	/// <summary>
	/// Event for touch move
	/// This event fired as long as finger or mouse keep moving
	/// 
	/// <param name="lastPosition">The position of last touch.</param>
	/// <param name="currentPosition">The position of current touch.</param>
	/// <param name="moveAmount">The amount of movement from last position.</param>
	/// </summary>
	public delegate void EventTouchMove(InputManager inputMgr, Vector2 lastPosition, Vector2 currentPosition, Vector2 moveAmount);
	public EventTouchMove Evt_TouchMove;

	/// <summary>
	/// Event for release finger or mouse
	/// 
	/// <param name="position">The position of released finger or mouse.</param>
	/// </summary>
	public delegate void EventTouchUp(InputManager inputMgr, Vector2 position);
	public EventTouchUp Evt_TouchUp;

	/// <summary>
	/// Event for double click
	/// </summary>
	public delegate void EventDoubleClick(InputManager inputMgr);
	public EventDoubleClick Evt_DoubleClick;

	/// <summary>
	/// Event for single click
	/// For mobile only
	/// </summary>
	public delegate void EventSingleClick(InputManager inputMgr);
	public EventSingleClick Evt_SingleClick;

	/// <summary>
	/// How long between click count as double click
	/// If less then this value it will count as double click
	/// The smaller value the faster click should be
	/// </summary>
	public float doubleClickDuration = 0.3f;

	/// <summary>
	/// The touch move senstivity.
	/// </summary>
	public float touchMoveSenstivity = 100f;

	/// <summary>
	/// double click start time
	/// </summary>
	private float doubleClickStartTime;

	/// <summary>
	/// Current touch position. This is depend on platform
	/// </summary>
	private Vector2 currentTouchPosition;

	/// <summary>
	/// /last touch position
	/// </summary>
	private Vector2 lastTouchPosition;

	// Update is called once per frame
	void Update () 
	{
		if (inputManagerEnabled == false)
			return;

		if ((Application.platform == RuntimePlatform.WindowsPlayer) || 
			(Application.platform == RuntimePlatform.WindowsEditor) || 
			(Application.platform == RuntimePlatform.OSXPlayer) ||
			(Application.platform == RuntimePlatform.OSXEditor)) 
		{
			//process standalone and editor input
			ProcessStandaloneEditorInput();
		}

		if ((Application.platform == RuntimePlatform.IPhonePlayer)) 
		{
			//process ios input
			ProcessIOSInput();
		}
	}

	/// <summary>
	/// Gets current the mouse position.
	/// </summary>
	/// <returns>The mouse position.</returns>
	public Vector2 GetMousePosition()
	{
		return currentTouchPosition;
	}

	/// <summary>
	/// Processes the standalone editor input.
	/// </summary>
	void ProcessStandaloneEditorInput()
	{
		//mouse button first press down
		if (Input.GetMouseButtonDown (0)) 
		{

			//store current mouse position to last touch position
			lastTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			if(Evt_TouchDown != null)
			{
				Evt_TouchDown(this, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			}
		}

		//mouse button held down and keep holding
		if (Input.GetMouseButton (0)) 
		{

			//get current touch position
			Vector2 curPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			//get last touch position
			Vector2 lastPos = lastTouchPosition;

			//if current touch position is different from last touch position
			if(curPos != lastTouchPosition)
			{
				//find out how much
				Vector2 moveAmount = Vector2.zero;

				//fix senstivity
				if(touchMoveSenstivity <= 0f)
				{
					touchMoveSenstivity = 1f;
				}

				//find out movenment amount on x
				moveAmount.x = Mathf.Abs(curPos.x - lastTouchPosition.x) / (((float)Screen.width / touchMoveSenstivity) * ((float)Screen.width/(float)Screen.height));
				moveAmount.x = Mathf.Clamp(moveAmount.x, 0.1f, 20f);
	
				//find out movenment amount on y
				moveAmount.y = Mathf.Abs(curPos.y - lastTouchPosition.y) / (((float)Screen.height / touchMoveSenstivity) * ((float)Screen.width/(float)Screen.height));
				moveAmount.y = Mathf.Clamp(moveAmount.y, 0.1f, 20f);


				//store current touch position to last touch position
				lastTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

				if(Evt_TouchMove != null)
				{
					Evt_TouchMove(this, lastPos, new Vector2(Input.mousePosition.x, Input.mousePosition.y), moveAmount);
				}
			}
		}

		if (Input.GetMouseButtonUp (0)) 
		{

			//store current touch position to last touch position
			lastTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			if(Evt_TouchUp != null)
			{
				Evt_TouchUp(this, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			}
		}

		//double click
		if (Input.GetMouseButtonUp (0)) 
		{
			if((Time.time-doubleClickStartTime) < doubleClickDuration)
			{
				//double click event
				if(Evt_DoubleClick != null)
				{
					Evt_DoubleClick(this);
				}

				doubleClickStartTime = -doubleClickDuration;
			}
			else
			{
				doubleClickStartTime = Time.time;
			}
		}

		//store mouse position
		currentTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}

	/// <summary>
	/// Processes the IOS input.
	/// </summary>
	void ProcessIOSInput()
	{
		//touch begin
		if (Input.touchCount > 0) 
		{
			Touch touch = Input.touches[0];

			if(touch.phase == TouchPhase.Began)
			{
				currentTouchPosition  = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);


				//store current mouse position to last touch position
				lastTouchPosition = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);

				if(Evt_TouchDown != null)
				{
					Evt_TouchDown(this, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
				}
			}
		}

		//press and move
		if(Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];

			if(touch.phase == TouchPhase.Moved)
			{
				currentTouchPosition  = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);

				//get current mouse position
				Vector2 curPos = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);

				//get last touch position
				Vector2 lastPos = lastTouchPosition;
				
				//if current touch position is different from last touch position
				if(curPos != lastTouchPosition)
				{
					//find out how much
					Vector2 moveAmount = Vector2.zero;

					//fix senstivity
					if(touchMoveSenstivity <= 0f)
					{
						touchMoveSenstivity = 1f;
					}

					float touchXScale = 768.0f / ((float)Screen.width);
					float touchYScale = 1024.0f / ((float)Screen.height);

					//find out movenment amount on x
					moveAmount.x = Mathf.Abs(curPos.x - lastTouchPosition.x) / (((float)Screen.width / (touchMoveSenstivity/touchXScale)) * ((float)Screen.width/(float)Screen.height));
					moveAmount.x = Mathf.Clamp(moveAmount.x, 0.1f, 20f);

					//find out movenment amount on y
					moveAmount.y = Mathf.Abs(curPos.y - lastTouchPosition.y) / (((float)Screen.height / (touchMoveSenstivity/touchYScale)) * ((float)Screen.width/(float)Screen.height));
					moveAmount.y = Mathf.Clamp(moveAmount.y, 0.1f, 20f);
					
					//store current toucn position to last touch position
					lastTouchPosition = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
					
					if(Evt_TouchMove != null)
					{
						Evt_TouchMove(this, lastPos, new Vector2(Input.touches[0].position.x, Input.touches[0].position.y), moveAmount);
					}
				}
			}
		}

		//release
		if(Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			
			if(touch.phase == TouchPhase.Ended)
			{
				currentTouchPosition  = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);

				//store current touch position to last touch position
				lastTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				
				if(Evt_TouchUp != null)
				{
					Evt_TouchUp(this, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
				}
			}
		}

		//single tap
		if(Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			
			if((touch.tapCount == 1) && (touch.phase == TouchPhase.Ended))
			{
				currentTouchPosition  = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
				
				if(Evt_SingleClick != null)
				{
					Evt_SingleClick(this);
				}
			}
		}

		//double tap
		if(Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];

			if((touch.tapCount == 2) && (touch.phase == TouchPhase.Ended))
			{
				currentTouchPosition  = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);

				if(Evt_DoubleClick != null)
				{
					Evt_DoubleClick(this);
				}


			}
		}


	}
}
