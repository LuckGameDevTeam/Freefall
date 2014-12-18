using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Player control.
/// This class control character.
/// Character gameobject must have this script attacted.
/// </summary>

[RequireComponent (typeof(CharacterHealth))]
[RequireComponent (typeof(CharacterEffect))]
[RequireComponent (typeof(CharacterAbilityControl))]
public class CharacterControl : MonoBehaviour 
{
	/// <summary>
	/// Event for character dead
	/// the moment character dead
	/// </summary>
	public delegate void EventCharacterDead(CharacterControl chaControl);
	public EventCharacterDead Evt_CharacterDead;

	/// <summary>
	/// Event for character victory
	/// the moment character victory
	/// </summary>
	public delegate void EventCharacterVictory(CharacterControl chaControl);
	public EventCharacterVictory Evt_CharacterVictory;

	/// <summary>
	/// Event for character dead action finish
	/// registe to receive character dead finish event
	/// Do not use coroutine in your event handler
	/// </summary>
	public delegate void EventCharacterDeadFinished(CharacterControl chaControl);
	public EventCharacterDeadFinished Evt_CharacterDeadFinished;

	/// <summary>
	/// Event for character victory action finish
	/// registe to receive character victory finish event
	/// Do not use coroutine in your event handler
	/// </summary>
	public delegate void EventCharacterVictoryFinished(CharacterControl chaControl);
	public EventCharacterVictoryFinished Evt_CharacterVictoryFinished;

	public delegate void EventCharacterStart(CharacterControl chaControl);
	public EventCharacterStart Evt_CharacterStart;

	/// <summary>
	/// The under attack clip.
	/// </summary>
	public AudioClip underAttackClip;

	/// <summary>
	/// The victory clip.
	/// </summary>
	public AudioClip victoryClip;

	/// <summary>
	/// If false character will do start falling and
	/// keep animation without anything and control
	/// </summary>
	[System.NonSerialized]
	public bool isInGame = true;

	/// <summary>
	/// character move speed
	/// </summary>
	public float moveSpeed = 5f;

	/// <summary>
	/// return speed when character automatic
	/// move back to center of screen
	/// </summary>
	public float returnSpeed = 5f;

	/// <summary>
	/// character start falling speed
	/// </summary>
	public float startFallingSpeed = 5f;

	/// <summary>
	/// chararcter floating down speed
	/// When victory character first move downward 
	/// </summary>
	public float floatingDownSpeed = 5f;

	/// <summary>
	/// character floating up speed
	/// when victory character first move downward then move upward
	/// </summary>
	public float floatingUpSpeed = 5f;

	/// <summary>
	/// character straight down speed
	/// When character dead go straight down 
	/// </summary>
	public float straightDownSpeed = 5f;

	/// <summary>
	/// If distance from player to destination is less or equal
	/// to this value, player will shift to destination immediately
	/// </summary>
	public float migrate = 0.2f;

	/// <summary>
	/// The character flip migrate.
	/// If touch move amount on x in frame is greater
	/// then or equal to this value then character will flip.
	/// Adjust this value to prevent character flip too quick
	/// 
	/// Range is from 0.1f~1.0f
	/// </summary>
	[Range(0.1f, 1.0f)]
	public float characterFlipMigrate = 0.5f;

	/// <summary>
	/// How long does it take to transit from other moving state
	/// to normal, this related to animation
	/// </summary>
	public float transToNormal = 0.5f;

	/// <summary>
	/// How can player recover from on attack to normal 
	/// </summary>
	public float recoverTime = 0.5f;

	/// <summary>
	/// This value indicate player can control character or not
	/// indicate character is in action mode or not
	/// performing action
	/// </summary>
	[System.NonSerialized]
	public bool actionMode = false;

	/// <summary>
	/// Character will start after a delay.
	/// </summary>
	public float delayStart = 1f;

	/// <summary>
	/// The victory enlarge.
	/// 
	/// true character enlarge when victory floating upward
	/// </summary>
	public bool victoryEnlarge = false;

	/// <summary>
	/// The size of the enlarge.
	/// </summary>
	public float enlargeSize = 1.0f;

	/// <summary>
	/// Test for dead action purpose
	/// </summary>
	//public bool dead = false;

	//Current jump force
	//keep changing this value cause multiple jump
	//public float currentJumpForce = 0f;

	//current jump count
	//private int currentJumpCount = 0;

	//Falling start time
	//public float startFallTime = 0f;

	//Last mosue position
	//private Vector2 lastMosuePos;



	/// <summary>
	/// When game start where is player going to be
	/// </summary>
	private Vector2 startPosition;

	/// <summary>
	/// Character current speed
	/// </summary>
	private float characterSpeed = 5f;

	/// <summary>
	/// character current velocity, this is used in player control mode
	/// </summary>
	private Vector2 currentVelocity = Vector2.zero;

	/// <summary>
	/// character's moving velocity
	/// </summary>
	public Vector2 movingVelocity = Vector2.zero;

	/// <summary>
	/// character's last position
	/// </summary>
	private Vector2 lastPosition = Vector2.zero;


	/// <summary>
	/// The position that player will move back to after any action complete
	/// default value is the position where at center of screen
	/// </summary>
	private Vector2 defaultPosition;

	/// <summary>
	/// The destination where player will move toward
	/// Change this value make player smooth move toward destination
	/// This is used in action mode
	/// </summary>
	private Vector2 dest;

	/// <summary>
	/// Character start when game time greater equal then
	/// this value, this involve control and move.
	/// </summary>
	private float initDelay;

	/// <summary>
	/// current on attack count of obstacle
	/// </summary>
	//private int onAttactCount = 0;

	/// <summary>
	/// Time to transit to normal
	/// </summary>
	private float transToNormalTime = 0f;

	/// <summary>
	/// Next recover time
	/// </summary>
	private float nextRecoverTime = 0f;

	/// <summary>
	/// Character animation that control animation
	/// for character
	/// </summary>
	private CharacterAnimation chaAnim;

	/// <summary>
	/// reference CharacterHealth
	/// </summary>
	private CharacterHealth characterHealth;

	/// <summary>
	/// Reference to character effect.
	/// </summary>
	private CharacterEffect characterEffect;

	/// <summary>
	/// InputManager
	/// </summary>
	private InputManager inputManager;

	/// <summary>
	/// The sound player.
	/// </summary>
	private SFXPlayer soundPlayer;

	/// <summary>
	/// Reference to GameController
	/// </summary>
	//private GameController gameController;

	public enum CharacterState
	{
		Unknow = 1<<0,
		Normal = 1<<1,
		StartFalling = 1<<2,//initially falling to center of screen
		MovingLeft = 1<<3,
		MovingRight = 1<<4,
		MovingUp = 1<<5,
		MovingDown = 1<<6,
		ReturnCenter = 1<<7,//character automatic return center of screen
		VictoryFloatingDown = 1<<8,//in victory action
		VictoryFloatingUp = 1<<9,//in victory action
		DeadStraightDown = 1<<10,//in dead/defeat action
		OnAttack = 1<<11,//in getting attack
		Attack = 1<<12,
		Victory = 1<<13,//character win
		Dead = 1<<14//character dead
	};

	/// <summary>
	/// Character state
	/// </summary>
	[System.NonSerialized]
	public CharacterState state = CharacterState.Unknow;

	//character bounce when contact with boss
	private float pushForce = 0f;
	private Vector2 pushDirection = Vector2.zero;
	public float bounceForce = 10f;
	public int maxBounceCount = 4;
	public int currentBounceCount = 0;

	void Awake()
	{
		//find GameController
		//gameController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<GameController>();

		//find character animation
		chaAnim = GetComponent<CharacterAnimation> ();

		//find input manager
		inputManager = GetComponent<InputManager> ();

		//find character health
		characterHealth = GetComponent<CharacterHealth> ();

		//find character effect
		characterEffect = GetComponent<CharacterEffect> ();

		soundPlayer = GetComponent<SFXPlayer> ();

		if(soundPlayer == null)
		{
			soundPlayer = gameObject.AddComponent<SFXPlayer>();
			soundPlayer.ignoreTimeScale = false;
		}

	}

	void OnEnable()
	{
		//register input event
		inputManager.Evt_SingleClick += EventSingleClick;
		inputManager.Evt_DoubleClick += EventDoubleClick;
		inputManager.Evt_TouchDown += EventTouchDown;
		inputManager.Evt_TouchMove += EventTouchMove;
		inputManager.Evt_TouchUp += EventTouchUp;

		//register character health event
		characterHealth.Evt_TakeDamage += EventTakeDamage;
		characterHealth.Evt_HealthDepleted += EventHealthDepleted;
	}

	void OnDisable()
	{
		//unregister input event
		inputManager.Evt_SingleClick -= EventSingleClick;
		inputManager.Evt_DoubleClick -= EventDoubleClick;
		inputManager.Evt_TouchDown -= EventTouchDown;
		inputManager.Evt_TouchMove -= EventTouchMove;
		inputManager.Evt_TouchUp -= EventTouchUp;

		//unregister character health event
		characterHealth.Evt_TakeDamage -= EventTakeDamage;
		characterHealth.Evt_HealthDepleted -= EventHealthDepleted;
	}

	void Start()
	{
		//initialize character
		InitCharacter ();

		//start initial falling action
		StartCoroutine("StartFalling");

	}

	/*
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == Tags.obstacle)
		{
			//onAttactCount++;

			//add attack state
			state |= CharacterState.OnAttack;

			//set next recover time
			nextRecoverTime = Time.time + recoverTime;

		}

	}
	*/


	/*
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == Tags.obstacle)
		{
			//onAttactCount--;

			if(onAttactCount == 0)
			{
				//remove on attack state
				state &= ~CharacterState.OnAttack;
			}

		}
	}
	*/

	//character bounce
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.GetComponent<Obstacle>() != null)
		{
			if(other.gameObject.GetComponent<Obstacle>().monsterType == MonsterTypes.Boss)
			{
				//make character non controlable
				inputManager.inputManagerEnabled = false;

				//increase bounce count
				currentBounceCount++;

				pushForce = bounceForce;
				
				pushDirection = transform.ConvertPositionToVector2() - other.contacts[0].point;
				
				CheckBounceOver();
			}
		}

	}


	void Update()
	{
		//if game time less then initDelay...return
		if (Time.time < initDelay)
			return;

		//processing player move
		MoveCharacter ();

		//calcualte moving velocity
		movingVelocity = transform.ConvertPositionToVector2 () - lastPosition;
		lastPosition = transform.ConvertPositionToVector2 ();

		//processing player animation
		ProcessAnim ();

		if((Time.time > nextRecoverTime) && ((state & CharacterState.OnAttack) == CharacterState.OnAttack))
		{
			Recover();
		}

	}

	////////////////////////////////Input Event////////////////////////////////
	
	/// <summary>
	///  Handle event single click on mobile.
	/// </summary>
	/// <param name="inputMgr">Input mgr.</param>
	void EventSingleClick(InputManager inputMgr)
	{
		//if player can not control character
		if (actionMode)
			return;

	}

	/// <summary>
	///  Handle event double click.
	/// </summary>
	/// <param name="inputMgr">Input mgr.</param>
	void EventDoubleClick(InputManager inputMgr)
	{
		//if player can not control character
		if (actionMode)
			return;
	}

	/// <summary>
	/// Handle event for touch down
	/// </summary>
	/// <param name="inputMgr">Input mgr.</param>
	/// <param name="position">Position.</param>
	void EventTouchDown(InputManager inputMgr, Vector2 position)
	{
		//if player can not control character
		if (actionMode)
			return;

	}

	/// <summary>
	/// Handle event for touch move
	/// </summary>
	/// <param name="inputMgr">Input mgr.</param>
	/// <param name="position">Position.</param>
	/// <param name="moveAmount">Move amount.</param>
	void EventTouchMove(InputManager inputMgr, Vector2 lastPosition, Vector2 currentPosition, Vector2 moveAmount)
	{
		//if player can not control character
		if (actionMode)
			return;

		//DebugEx.Debug ("Touch move:"+moveAmount);

		//change spped
		characterSpeed = moveSpeed;

		//turn of actionMode
		actionMode = false;

		//calcualte velocity
		currentVelocity = new Vector2 (Mathf.Sign(currentPosition.x - lastPosition.x) * (moveAmount.x * characterSpeed) * Time.deltaTime, 
		                               Mathf.Sign(currentPosition.y - lastPosition.y) * (moveAmount.y * characterSpeed) * Time.deltaTime);

		//check moving direction on x
		if(Mathf.Sign(currentPosition.x - lastPosition.x) < 0f)
		{
			//moving left

			//add MovingLeft state
			state |= CharacterState.MovingLeft;

			//remove MovingRight state
			state &= ~CharacterState.MovingRight;

			//change sprite direction to left
			if(moveAmount.x > characterFlipMigrate)
			{
				//if character current facing right
				if(Mathf.Sign(transform.localScale.x) == -1f)
				{
					transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
				}

			}
		}
		else if(Mathf.Sign(currentPosition.x - lastPosition.x) > 0f)
		{
			//move right

			//add MovingRight state
			state |= CharacterState.MovingRight;

			//remove MovingLeft state
			state &= ~CharacterState.MovingLeft;

			//change sprite direction to right
			if(moveAmount.x > characterFlipMigrate)
			{
				//if character current facing left
				if(Mathf.Sign(transform.localScale.x) == 1f)
				{
					transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
				}
			}
		}

		//check moving direction on y
		if(Mathf.Sign(currentPosition.y - lastPosition.y) < 0f)
		{
			//moving down

			//add MovingDown state
			state |= CharacterState.MovingDown;

			//remove MovingUp state
			state &= ~CharacterState.MovingUp;
		}
		else if(Mathf.Sign(currentPosition.y - lastPosition.y) > 0f)
		{
			//moving up

			//add MovingUp state
			state |= CharacterState.MovingUp;

			//remove MovingDown state
			state &= ~CharacterState.MovingDown;
		}

		//remove normal state
		state &= ~CharacterState.Normal;

		//remove return center of screen state...player trying to move character
		//don't let character automatic return center of screen
		state &= ~CharacterState.ReturnCenter;

	}

	/// <summary>
	/// Handle event for touch up
	/// </summary>
	/// <param name="inputMgr">Input mgr.</param>
	/// <param name="position">Position.</param>
	void EventTouchUp(InputManager inputMgr, Vector2 position)
	{
		//if player can not control character
		if (actionMode)
			return;

		//add return center of screen state...character automatic return to center of screen
		state |= CharacterState.ReturnCenter;
	}

	////////////////////////////////Input Event////////////////////////////////


	////////////////////////////////Character Health Event////////////////////////////////

	/// <summary>
	/// Handle event for character health take damage
	/// </summary>
	void EventTakeDamage(float damageAmount, float healthBefore, float healthAfter)
	{
		//add attack state
		state |= CharacterState.OnAttack;
		
		//set next recover time
		nextRecoverTime = Time.time + recoverTime;

		//play damage effect
		characterEffect.PlayDamageEffect ();

		//play under attack sound
		if(underAttackClip != null)
		{
			if(soundPlayer == null)
			{
				soundPlayer = gameObject.AddComponent<SFXPlayer>();
			}

			if(soundPlayer.IsPlaying)
			{
				soundPlayer.StopSound();
			}

			soundPlayer.sfxClip = underAttackClip;
			soundPlayer.PlaySound();
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" can not play under attack sound, under attack clip not assigned");
		}
	}

	/// <summary>
	/// Handle event for character health total depleted
	/// </summary>
	void EventHealthDepleted()
	{
		PlayerDead ();
	}

	////////////////////////////////Character Health Event////////////////////////////////

	////////////////////////////////Public Interface////////////////////////////////

	/// <summary>
	/// Player victory.
	/// Call this to make player vectory
	/// This method contain any victory action should be perform.
	/// </summary>
	public void PlayerVictory()
	{
		characterHealth.invulnerable = true;

		//make state vicrtory
		state = CharacterState.Victory;

		if(Evt_CharacterVictory != null)
		{
			Evt_CharacterVictory(this);
		}

		//remove all aiblities
		RemoveAllAbility ();

		//play victory sound
		if(victoryClip != null)
		{
			if(soundPlayer != null)
			{
				soundPlayer.sfxClip = victoryClip;
				soundPlayer.PlaySound();
			}
		}
		else
		{
			DebugEx.DebugError(gameObject.name+" unable to play victory clip, victory clip not assigned");
		}
		
		//start victory action
		StartCoroutine ("FloatingDownward");
	}

	/// <summary>
	/// Player dead/defeat.
	/// Call this to make player dead/defeat
	/// This method contain any dead/defeat action should be perform.
	/// </summary>
	public void PlayerDead()
	{
		characterHealth.invulnerable = true;

		//make state dead
		state = CharacterState.Dead;

		if(Evt_CharacterDead != null)
		{
			Evt_CharacterDead(this);
		}

		//start dead action
		StartCoroutine ("StraightDownward");

	}

	/// <summary>
	/// Restart character.
	/// </summary>
	public void Restart()
	{
		//reset character state to normal
		state = CharacterState.Unknow;

		//reset animation
		chaAnim.Reset ();

		//reset health
		CharacterHealth health = GetComponent<CharacterHealth> ();
		health.ResetHealth ();

		//stop all effects
		characterEffect.StopAllEffects ();

		//remove all abilities
		RemoveAllAbility ();

		//initialize character
		InitCharacter ();
		
		//start initial falling action
		StartCoroutine("StartFalling");
	}

	/// <summary>
	/// Adds ability gameobject to character.
	/// 
	/// Any ability that is currently running will be removed
	/// </summary>
	/// <param name="abilityObject">Ability object.</param>
	public void AddAbility(GameObject abilityObject)
	{
		//check if any ability exist.... remove it
		if(transform.childCount > 0)
		{
			for(int i=0; i<transform.childCount; i++)
			{
				GameObject child = transform.GetChild(i).gameObject;

				if(child.GetComponent<Ability>() != null)
				{
					//tell ability to remove
					child.GetComponent<Ability>().RemoveAbilityImmediately();
				}
			}
		}

		//check if ability object is an ability
		Ability ability = abilityObject.GetComponent<Ability> ();
		if(ability != null)
		{

			//active ability 
			ability.ActiveAbility(gameObject);
		}
		else
		{
			DebugEx.DebugError("You give character an non ability gameobject");
		}
	}

	/// <summary>
	/// Removes all ability.
	/// </summary>
	public void RemoveAllAbility()
	{
		//check if any ability exist.... remove it
		if(transform.childCount > 0)
		{
			for(int i=0; i<transform.childCount; i++)
			{
				GameObject child = transform.GetChild(i).gameObject;
				
				if(child.GetComponent<Ability>() != null)
				{
					//tell ability to remove
					child.GetComponent<Ability>().RemoveAbilityImmediately();
				}
			}
		}
	}

	/// <summary>
	/// Collisions setting.
	/// </summary>
	/// <param name="enable">true enable collision...otherwise false</param>
	public void CollisionSetting(bool sEnable)
	{
		gameObject.GetComponent<Collider2D> ().enabled = sEnable;

	}

	/// <summary>
	/// Bounce setting
	/// </summary>
	/// <param name="enabled">true enable bounce...otherwise false</param>
	public void BounceSetting(bool sEnabled)
	{

		Collider2D[] c = GetComponentsInChildren<Collider2D> ();

		for(int i=0; i<c.Length; i++)
		{
			c[i].enabled = sEnabled;
		}
	}

	////////////////////////////////Public Interface////////////////////////////////

	////////////////////////////////Internal////////////////////////////////

	/// <summary>
	/// Initialize character
	/// </summary>
	void InitCharacter()
	{
		Camera cam = Camera.main;
		
		//set player's start position
		float camTopBorder = cam.GetTopBorderWorldSpace (transform.position.z);
		startPosition = new Vector2 (cam.transform.position.x, camTopBorder + renderer.bounds.extents.y + 2f);
		
		//set start position to player
		transform.position = new Vector3 (startPosition.x, startPosition.y, transform.position.z);

		//set default flip to left
		transform.localScale = new Vector3 (1f, 1f, 1f);
		
		//set default position to main camera position which is center of camera
		defaultPosition = new Vector2 (cam.transform.position.x, cam.transform.position.y);
		
		//set init delay
		initDelay = Time.time + delayStart;
		
		//initial character speed
		characterSpeed = moveSpeed;

		//no bounce
		pushForce = 0f;
		currentBounceCount = 0;
		pushDirection = Vector2.zero;

		//create effect
		if(isInGame)
		{
			characterEffect.CreateEffects ();
		}

		//init animation
		chaAnim.InitAnim ();

		lastPosition = transform.ConvertPositionToVector2 ();

		//enable collision
		CollisionSetting (true);

		//enable bounce
		BounceSetting (true);

		/*character bounce
		pushForce = 0f;
		pushDirection = Vector2.zero;
		*/
	}

	/// <summary>
	/// processing character animation
	/// </summary>
	void ProcessAnim()
	{
		if (chaAnim == null)
			return;


		//victory
		if((state & CharacterState.VictoryFloatingUp) == CharacterState.VictoryFloatingUp)
		{
			chaAnim.VictoryAnim();
			return;
		}
		
		//dead
		if((state & CharacterState.Dead) == CharacterState.Dead)
		{
			chaAnim.Dead();
			return;
		}
		
		//on attack
		if((state & CharacterState.OnAttack) == CharacterState.OnAttack)
		{
			chaAnim.OnAttack();
			return;
		}

		//falling to screen center
		if (state == CharacterState.StartFalling)
		{
			chaAnim.NormalAnim ();
		}

		if (((state & CharacterState.Normal) == CharacterState.Normal) || 
		    ((state & CharacterState.Victory) == CharacterState.Victory))
		{
			chaAnim.NormalAnim ();
		}


		if(((state & CharacterState.MovingLeft) == CharacterState.MovingLeft) ||
		   ((state & CharacterState.MovingRight) == CharacterState.MovingRight) ||
		   ((state & CharacterState.MovingUp) == CharacterState.MovingUp) ||
		   ((state & CharacterState.MovingDown) == CharacterState.MovingDown))
		{
			chaAnim.MovingAnim();
		}


	}

	/// <summary>
	/// Player was on attack now recover to normal
	/// </summary>
	void Recover()
	{
		//remove on attack state
		state &= ~CharacterState.OnAttack;
	}

	/// <summary>
	/// Checks if the bounce over or not.
	/// </summary>
	void CheckBounceOver()
	{
		if(currentBounceCount >= maxBounceCount)
		{
			pushForce = 0f;
			currentBounceCount = 0;
			pushDirection = Vector2.zero;
			
			//add return center of screen state...character automatic return to center of screen
			state |= CharacterState.ReturnCenter;
			
			inputManager.inputManagerEnabled = true;
		}
	}


	/// <summary>
	/// This prevent sprite moving out of screen.
	/// It will recalculate new destinaition if needed.
	/// It take sprite's bound into count and compare to camera border.
	/// </summary>
	/// <returns>The new position.</returns>
	/// <param name="currentPos">Current pos.</param>
	/// <param name="renderer">Character's render.</param>
	/// <param name="cam">Camera it will be use to compare.</param>
	Vector2 FixPosition(Vector2 currentPos, Renderer renderer, Camera cam)
	{
		Vector2 retPos = currentPos;
		
		//find left and right border of camera in world space
		float camLeftBorder = cam.GetLeftBorderWorldSpace(transform.position.z);
		float camRightBorder = cam.GetRightBorderWorldSpace(transform.position.z);
		float camTopBorder = cam.GetTopBorderWorldSpace (transform.position.z);
		float camBottomBorder = cam.GetBottomBorderWorldSpace (transform.position.z);
		
		//check if sprite is out of camera left border...adjust new position of sprite
		if((retPos.x-renderer.bounds.extents.x) < camLeftBorder)
		{
			//adjust sprtie position
			retPos.x = camLeftBorder + renderer.bounds.extents.x;
		}
		
		//check if sprite is out of camera right border...adjust new position of sprite
		if((retPos.x+renderer.bounds.extents.x) > camRightBorder)
		{
			//adjust sprtie position
			retPos.x = camRightBorder - renderer.bounds.extents.x;
		}

		//check if sprite is out of camera top border...adjust new position of sprite
		if((retPos.y+renderer.bounds.extents.y) > camTopBorder)
		{
			retPos.y = camTopBorder - renderer.bounds.extents.y;
		}

		//check if sprite is out of camera bottom border...adjust new position of sprite
		if((retPos.y-renderer.bounds.extents.y) < camBottomBorder)
		{
			retPos.y = camBottomBorder + renderer.bounds.extents.y;
		}
		
		return retPos;
	}
	
	/// <summary>
	/// Moves the character.
	/// Handle player's moving
	/// </summary>
	void MoveCharacter()
	{

		if(!actionMode) //player is controlling character
		{
			//character bounce
			if(pushForce > 0f)
			{
				Vector2 wallNormal = Vector2.zero;

				//find out the normal of which side of screen character against
				if((transform.position.x - renderer.bounds.extents.x) <= Camera.main.GetLeftBorderWorldSpace(transform.position.z))
				{
					wallNormal = new Vector2(1f, 0f);
				}
				else if((transform.position.x + renderer.bounds.extents.x) >= Camera.main.GetRightBorderWorldSpace(transform.position.z))
				{
					wallNormal = new Vector2(-1f, 0f);
				}
				else if((transform.position.y - renderer.bounds.extents.y) <= Camera.main.GetBottomBorderWorldSpace(transform.position.z))
				{
					wallNormal = new Vector2(0f, 1f);
				}
				else if((transform.position.y + renderer.bounds.extents.y) >= Camera.main.GetTopBorderWorldSpace(transform.position.z))
				{
					wallNormal = new Vector2(0f, -1f);
				}

				//if wall normal is not 0
				if(wallNormal != Vector2.zero)
				{
					//increase bounce count by 1
					currentBounceCount++;

					//calculate push direction, formula is from internet
					Vector2 newDirectoin = (-2 * Vector2.Dot((pushDirection), wallNormal)*wallNormal + (pushDirection));

					pushDirection = newDirectoin.normalized;
				}

				pushDirection = pushDirection.normalized;

				//calculate push amount
				Vector2 amount = (pushForce * Time.deltaTime) * pushDirection;

				//apply amount of push
				transform.position = new Vector3 (transform.position.x + amount.x, transform.position.y + amount.y, transform.position.z);

				CheckBounceOver();

				return;
			}


			//move character if velocity is not zero
			if(currentVelocity != Vector2.zero)
			{
				//pushForce = 0f;

				//get character current position
				Vector2 characterPos = transform.ConvertPositionToVector2 ();
				
				//find out new position
				characterPos = new Vector2 (characterPos.x + currentVelocity.x, characterPos.y + currentVelocity.y);
				
				//prevent out of screen
				characterPos = FixPosition (characterPos, renderer, Camera.main);
				
				//move character
				transform.position = new Vector3 (characterPos.x, characterPos.y, transform.position.z);
				
				//clear velocity
				currentVelocity = Vector2.zero;

				//set time to transit to normal
				transToNormalTime = Time.time + transToNormal;
			}
			else if((Time.time >= transToNormalTime) && (currentVelocity == Vector2.zero))
			{

				//add normal state
				state |= CharacterState.Normal;

				//remove Moving Right,Left,Up,Down state
				state &= ~CharacterState.MovingLeft;
				state &= ~CharacterState.MovingRight;
				state &= ~CharacterState.MovingUp;
				state &= ~CharacterState.MovingDown;

			}

			//player not touch screen...automatic return to center of screen
			if((transform.ConvertPositionToVector2() != defaultPosition) && ((state & CharacterState.ReturnCenter) == CharacterState.ReturnCenter))
			{
				//set character current speed to return speed
				characterSpeed = returnSpeed;

				//if distance, which between character current position and center of screen is less and equal than migrate...
				//set character position to center of screen...otherwise move character
				if(Vector2.Distance(transform.ConvertPositionToVector2(), defaultPosition) <= migrate)
				{
					//set character position to center of screen
					transform.position = new Vector3(defaultPosition.x, defaultPosition.y, transform.position.z);

					//remove return center of screen state
					state &= ~CharacterState.ReturnCenter;
				}
				else
				{
					//find out new position
					Vector2 newPos = Vector2.Lerp(transform.ConvertPositionToVector2(), defaultPosition, characterSpeed * Time.deltaTime);

					//move character 
					transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
				}
			}

		}
		else //player is not controlling character and character is perfoming action moving to destination
		{
			if(transform.ConvertPositionToVector2() != dest)
			{
				//clear velocity
				currentVelocity = Vector2.zero;
				
				//remove Moving Right,Left,Up,Down state
				state &= ~CharacterState.MovingLeft;
				state &= ~CharacterState.MovingRight;
				state &= ~CharacterState.MovingUp;
				state &= ~CharacterState.MovingDown;
				
				
				if(Vector2.Distance(dest, transform.ConvertPositionToVector2()) <= migrate)
				{
					//character almost reach destination, shift to position
					transform.position = new Vector3(dest.x, dest.y, transform.position.z);
					
					//turn of actionMode
					actionMode = false;
				}
				else
				{
					//calculate new position by smoothing move
					Vector2 newPos = Vector2.Lerp(transform.position, dest, characterSpeed * Time.deltaTime);
					
					//move character
					transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
				}
			}

		}

	}

	////////////////////////////////Internal////////////////////////////////


	////////////////////////////////Action////////////////////////////////

	/// <summary>
	/// For character initial falling to center of camera from top.
	/// Player can not control character during this action.
	/// </summary>
	/// <returns>The falling.</returns>
	IEnumerator StartFalling()
	{
		//change state to StartFalling
		state = CharacterState.StartFalling;

		//turn on actionMode player can't cointrol
		actionMode = true;

		//set dest equal to default position
		dest = defaultPosition;

		//change character speed to start falling speed
		characterSpeed = startFallingSpeed;

		while (true) 
		{
			//check if player position is at default position(center of camera)
			if(transform.ConvertPositionToVector2() == defaultPosition)
			{
				//change state to Normal
				state = CharacterState.Normal;

				//start falling end
				//turn off actionMode player can control
				actionMode = false;

				//change speed back to move speed
				characterSpeed = moveSpeed;

				break;
			}

			yield return null;
		}

		if(isInGame == false)
		{
			GetComponent<InputManager>().enabled = false;
			GetComponent<CharacterAbilityControl>().enabled = false;
		}
		else
		{
			//tell GameController to start game
			GameController.sharedGameController.StartGame ();
			
			//tell ability control to start
			CharacterAbilityControl caControl = GetComponent<CharacterAbilityControl> ();
			caControl.running = true;
		}

		if(Evt_CharacterStart != null)
		{
			Evt_CharacterStart(this);
		}
	}

	/// <summary>
	/// When character dead, character move straight down until out of screen
	/// This is for straight downward moving
	/// </summary>
	/// <returns>The downward.</returns>
	IEnumerator StraightDownward()
	{
		//tell ability control to stop
		CharacterAbilityControl control = GetComponent<CharacterAbilityControl> ();
		control.running = false;

		//change state to DeadStraightDown
		state |= CharacterState.DeadStraightDown;


		//turn on actionMode player can't cointrol
		actionMode = true;

		//change speed to straight downward
		characterSpeed = straightDownSpeed;

		//find dest for straight downward
		float camBottomBorder = Camera.main.GetBottomBorderWorldSpace (transform.position.z);
		dest = new Vector2 (transform.position.x, camBottomBorder - renderer.bounds.extents.y - 1f);

		while (true) 
		{
			//if character move downward and reach dest
			if(transform.ConvertPositionToVector2() == dest)
			{
				//remove state 
				state &= ~CharacterState.DeadStraightDown;
				state &= ~CharacterState.Dead;

				//change state to Unknow
				state = CharacterState.Unknow;
				
				//change speed back to move speed
				characterSpeed = moveSpeed;
				
				break;
			}
			
			yield return null;
		}

		//fire dead action finished event 
		if(Evt_CharacterDeadFinished != null)
		{
			Evt_CharacterDeadFinished(this);
		}

		characterHealth.invulnerable = false;
	}

	/// <summary>
	/// When victory characte move downward until out of screen and then upward until out of top of screen
	/// This is for downward moving
	/// </summary>
	/// <returns>The downward.</returns>
	IEnumerator FloatingDownward()
	{
		//tell ability control to stop
		CharacterAbilityControl control = GetComponent<CharacterAbilityControl> ();
		control.running = false;

		//add state VictoryFloatingDown
		state |= CharacterState.VictoryFloatingDown;

		//turn on actionMode player can't cointrol
		actionMode = true;

		//change speed to floating downward
		characterSpeed = floatingDownSpeed;

		//start to slow down background
		GameController.sharedGameController.SlowDownBackgroundScrolling ();

		//find dest for floating downward
		float camBottomBorder = Camera.main.GetBottomBorderWorldSpace (transform.position.z);
		dest = new Vector2 (Camera.main.transform.position.x, camBottomBorder - renderer.bounds.extents.y - 1f);

		while (true) 
		{

			//if character move downward and reach dest
			if(transform.ConvertPositionToVector2() == dest)
			{
				//remove state
				state &= ~CharacterState.VictoryFloatingDown;

				//add state Unknow
				state |= CharacterState.Unknow;

				//change speed back to move speed
				characterSpeed = moveSpeed;

				break;
			}

			yield return null;
		}

		//start moving upward 
		StartCoroutine ("FloatingUpward");
	}

	/// <summary>
	/// When victory characte move downward until out of screen and then upward until out of top of screen
	/// This is for upward moving
	/// </summary>
	/// <returns>The upward.</returns>
	IEnumerator FloatingUpward()
	{
		//add state VictoryFloatingUp
		state |= CharacterState.VictoryFloatingUp;

		//turn on actionMode player can't cointrol
		actionMode = true;

		//change speed to floating upward
		characterSpeed = floatingUpSpeed;

		//stop background scrolling
		GameController.sharedGameController.StopBackgroundScrolling ();

		//check if need to enlarge character
		if(victoryEnlarge)
		{
			transform.localScale = new Vector3(enlargeSize, enlargeSize, 1.0f);
		}

		//find dest for floating upward
		float camTopBorder = Camera.main.GetTopBorderWorldSpace (transform.position.z);
		dest = new Vector2 (Camera.main.transform.position.x, camTopBorder + renderer.bounds.extents.y + 1f);

		while (true) 
		{
			//if character move upward and reach dest
			if(transform.ConvertPositionToVector2() == dest)
			{
				//remove state
				state &= ~CharacterState.VictoryFloatingUp;
				state &= ~CharacterState.Victory;

				//add state Unknow
				state |= CharacterState.Unknow;
				
				//change speed back to move speed
				characterSpeed = moveSpeed;
				
				break;
			}
			
			yield return null;
		}

		//fire victory action finished event
		if(Evt_CharacterVictoryFinished != null)
		{
			Evt_CharacterVictoryFinished(this);
		}

		characterHealth.invulnerable = false;
	}

	////////////////////////////////Action////////////////////////////////

	////////////////////////////////Properties////////////////////////////////

	/// <summary>
	/// is character dead or not
	/// </summary>
	/// /// <value><c>true</c> if character is dead; otherwise, <c>false</c>.</value>
	public bool IsDead{ get{ return ((state & CharacterState.Dead) == CharacterState.Dead);  }}

	/// <summary>
	/// is character victory
	/// </summary>
	/// <value><c>true</c> if character is victory; otherwise, <c>false</c>.</value>
	public bool IsVictory{get{ return ((state & CharacterState.Victory) == CharacterState.Victory); }}

	/// <summary>
	/// Character's current moving velocity
	/// </summary>
	/// <value>The moving velocity.</value>
	public Vector2 MovingVelocity{get{return movingVelocity;}}

	////////////////////////////////Properties////////////////////////////////
	
}
