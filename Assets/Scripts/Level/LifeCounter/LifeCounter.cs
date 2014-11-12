using UnityEngine;
using System.Collections;
using System;
using Soomla.Store;
using SIS;

/// <summary>
/// Life counter.
/// 
/// GameObject has this script must live forever
/// 
/// This class deal with player's life counting during or
/// off game
/// </summary>
public class LifeCounter : MonoBehaviour 
{
	//Defination of PlayerLife keyword
	public static string PlayerLife = "PlayerLife";

	public enum RecoverUnit
	{
		Unit_Year,
		Unit_Month,
		Unit_Day,
		Unit_Hour,
		Unit_Minute,
		Unit_Second
	}

	public delegate void EventLifeCountChanged(LifeCounter counter ,int lifeCount);
	/// <summary>
	/// Event for life count changed.
	/// </summary>
	public EventLifeCountChanged Evt_LifeCountChanged;

	public delegate void EventLifeRegenStop(LifeCounter counter);
	/// <summary>
	/// Event when life counter regenerate process stop
	/// </summary>
	public EventLifeRegenStop Evt_LifeRegenStop;

	public delegate void EventLifeFull(LifeCounter counter);
	/// <summary>
	/// Event when life counter regenerate is full
	/// </summary>
	public EventLifeFull Evt_LifeFull;

	public delegate void EventLifeRegenTick(LifeCounter counter, TimeSpan timeLeft);
	public EventLifeRegenTick Evt_LifeRegenTick;

	/// <summary>
	/// The time unit.
	/// 
	/// This define the time unit that is for recover 1 life
	/// </summary>
	public RecoverUnit timeUnit = RecoverUnit.Unit_Hour;

	/// <summary>
	/// The recover period.
	/// 
	/// How long does it take to recover 1 life base on RecoverUnit
	/// </summary>
	public int recoverPeriod = 1;

	/// <summary>
	/// The max life can recover.
	/// </summary>
	public int maxLifeRecover = 3;

	/// <summary>
	/// The life give count.
	/// 
	/// When give life to player how many life should give
	/// </summary>
	public int lifeGiveCount = 1;

	/// <summary>
	/// The push notifi message.
	/// </summary>
	public string pushNotifiMsgKey = "LifeRecovered";

	/// <summary>
	/// The is regenerating life.
	/// </summary>
	private bool isRegeneratingLife = false;

	/// <summary>
	/// The next give life date time.
	/// </summary>
	private DateTime nextGiveLifeDateTime;

	/// <summary>
	/// The current life count.
	/// </summary>
	private int currentLifeCount = 0;

	void OnDisable()
	{
		//StoreEvents.OnGoodBalanceChanged -= onGoodBalanceChanged;
		DBManager.updatedDataEvent -= OnDBManagerDataUpdate;
	}

	// Use this for initialization
	void Start () 
	{

		//initialize
		Init ();

	}

	// Update is called once per frame
	void Update () 
	{
		//if life regen
		if(isRegeneratingLife)
		{
			int result = DateTime.Now.CompareTo(nextGiveLifeDateTime);

			//if time is reach or over
			if((result == 0) || (result > 0))
			{
				//stop life regenerate
				isRegeneratingLife = false;

				if(Evt_LifeRegenStop != null)
				{
					Evt_LifeRegenStop(this);
				}
				
				//if(StoreInventory.GetItemBalance(StoreAssets.PLAYER_LIFE_ITEM_ID) >= maxLifeRecover)
				if(DBManager.GetPlayerData(PlayerLife).AsInt >= maxLifeRecover)
				{
					isRegeneratingLife = false;

					if(Evt_LifeFull != null)
					{
						Evt_LifeFull(this);
					}

					return;
				}

				//give 1 life count
				GiveLife(lifeGiveCount);

				Debug.Log("give "+lifeGiveCount+" life to player");

			}
			else
			{
				//calculate time left
				TimeSpan theTimeLeft = nextGiveLifeDateTime.Subtract(DateTime.Now);

				if(Evt_LifeRegenTick != null)
				{
					Evt_LifeRegenTick(this, theTimeLeft);
				}
			}
		}
	}

	/// <summary>
	/// Init life counter.
	/// </summary>
	void Init()
	{
		CancelAllNotifications ();

		//StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
		DBManager.updatedDataEvent += OnDBManagerDataUpdate;

		//currentLifeCount = StoreInventory.GetItemBalance (StoreAssets.PLAYER_LIFE_ITEM_ID);
		currentLifeCount = DBManager.GetPlayerData (PlayerLife).AsInt;

		if(Evt_LifeCountChanged != null)
		{
			Evt_LifeCountChanged(this ,currentLifeCount);
		}

		AnalyzeLifeCountOffGame ();

		Debug.Log("Life counter is running...");
	}

	/// <summary>
	/// Gives the life.
	/// </summary>
	/// <param name="amount">Amount.</param>
	void GiveLife(int amount)
	{
		//currentLifeCount += amount;

		//StoreInventory.GiveItem(StoreAssets.PLAYER_LIFE_ITEM_ID, amount);

		DBManager.IncrementPlayerData (PlayerLife, amount);

		if(Evt_LifeCountChanged != null)
		{
			Evt_LifeCountChanged(this, currentLifeCount);
		}
	}

	/// <summary>
	/// Raises the application pause event.
	/// 
	/// Deal with event that application enter background
	/// </summary>
	/// <param name="pauseStatus">If set to <c>true</c> pause status.</param>
	void OnApplicationPause(bool pauseStatus)
	{
		if(pauseStatus)
		{
			Debug.Log("app enter background");

			//int currentLifeCount = StoreInventory.GetItemBalance(StoreAssets.PLAYER_LIFE_ITEM_ID);
			int currentLifeCount = DBManager.GetPlayerData(PlayerLife).AsInt;

			if(currentLifeCount < maxLifeRecover)
			{
				//stop life regen
				isRegeneratingLife = false;

				//save current date time
				LastEnterBackgroundTime data = new LastEnterBackgroundTime();

				DateTime baseTime = DateTime.Now;

				data.years = baseTime.Year;
				data.months = baseTime.Month;
				data.days = baseTime.Day;
				data.hours = baseTime.Hour;
				data.minutes = baseTime.Minute;
				data.seconds = baseTime.Second;

				LastEnterBackgroundTime.Save(data);


				//calculate how many life should recovered
				int diff = maxLifeRecover - currentLifeCount;

				//set push notifi
				switch(timeUnit)
				{
				case RecoverUnit.Unit_Year:

					int years = diff * recoverPeriod;

					GenerateNotification(pushNotifiMsgKey, DateTime.Now.AddYears(years));

					break;

				case RecoverUnit.Unit_Month:

					int months = diff * recoverPeriod;

					GenerateNotification(pushNotifiMsgKey, DateTime.Now.AddMonths(months));
					
					break;

				case RecoverUnit.Unit_Day:

					int days = diff * recoverPeriod;
					
					GenerateNotification(pushNotifiMsgKey, DateTime.Now.AddDays(days));
					
					break;

				case RecoverUnit.Unit_Hour:

					int hours = diff * recoverPeriod;
					
					GenerateNotification(pushNotifiMsgKey, DateTime.Now.AddHours(hours));
					
					break;

				case RecoverUnit.Unit_Minute:

					int minutes = diff * recoverPeriod;
					
					GenerateNotification(pushNotifiMsgKey, DateTime.Now.AddMinutes(minutes));
					
					break;

				case RecoverUnit.Unit_Second:

					int seconds = diff * recoverPeriod;
					
					GenerateNotification(pushNotifiMsgKey, DateTime.Now.AddSeconds(seconds));
					
					break;
				}
			}
		}
		else
		{
			Debug.Log("app enter foreground");

			CancelAllNotifications ();

			AnalyzeLifeCountOffGame();
		}
	}

	/*
	/// <summary>
	/// Ons the good balance changed.
	/// </summary>
	/// <param name="good">Good.</param>
	/// <param name="balance">Balance.</param>
	/// <param name="amountAdded">Amount added.</param>

	void onGoodBalanceChanged(VirtualGood good, int balance, int amountAdded)
	{
		if(good.ItemId == StoreAssets.PLAYER_LIFE_ITEM_ID)
		{
			currentLifeCount = balance;

			if(Evt_LifeCountChanged != null)
			{
				Evt_LifeCountChanged(this, currentLifeCount);
			}

			AnalyzeLifeCountOnGame();
		}
	}
	*/

	/// <summary>
	/// Handle when DBManager has data update
	/// </summary>
	void OnDBManagerDataUpdate()
	{
		int lifeCount = DBManager.GetPlayerData (PlayerLife).AsInt;

		if(currentLifeCount != lifeCount)
		{
			currentLifeCount = lifeCount;

			if(Evt_LifeCountChanged != null)
			{
				Evt_LifeCountChanged(this, currentLifeCount);
			}

			AnalyzeLifeCountOnGame();
		}

	}

	/// <summary>
	/// Analyzes the life count.
	/// 
	/// Check if it is time to start counting time and regenerate life
	/// This is used for in game check
	/// 
	/// Call it when time is right, do not call this frequently
	/// </summary>
	void AnalyzeLifeCountOnGame()
	{
		//if(StoreInventory.GetItemBalance(StoreAssets.PLAYER_LIFE_ITEM_ID) < maxLifeRecover)
		if(DBManager.GetPlayerData(PlayerLife).AsInt < maxLifeRecover)
		{
			//start regenerating life
			isRegeneratingLife = true;

			//set next date time to give life
			switch(timeUnit)
			{
			case RecoverUnit.Unit_Year:

				nextGiveLifeDateTime = DateTime.Now.AddYears(recoverPeriod);
				break;

			case RecoverUnit.Unit_Month:

				nextGiveLifeDateTime = DateTime.Now.AddMonths(recoverPeriod);
				break;

			case RecoverUnit.Unit_Day:

				nextGiveLifeDateTime = DateTime.Now.AddDays(recoverPeriod);
				break;

			case RecoverUnit.Unit_Hour:

				nextGiveLifeDateTime = DateTime.Now.AddHours(recoverPeriod);
				break;

			case RecoverUnit.Unit_Minute:

				nextGiveLifeDateTime = DateTime.Now.AddMinutes(recoverPeriod);
				break;

			case RecoverUnit.Unit_Second:

				nextGiveLifeDateTime = DateTime.Now.AddSeconds(recoverPeriod);
				break;

			}
		}
		else
		{
			isRegeneratingLife = false;

			if(Evt_LifeRegenStop != null)
			{
				Evt_LifeRegenStop(this);
			}

			if(Evt_LifeFull != null)
			{
				Evt_LifeFull(this);
			}
		}
	}

	/// <summary>
	/// Analyzes the life count off game.
	/// 
	/// Check how many life should give to player/
	/// This is used for off game(app enter background or killed)
	/// Each time app enter background, app will same the date time at that moment then
	/// this function will load back that date time and check to see how many life should give to
	/// player. By this way you can have a fake date time counter when off game.
	/// 
	/// Call it when time is right, do not call this frequently
	/// </summary>
	void AnalyzeLifeCountOffGame()
	{
		//get back current player life count
		//int currentAmount = StoreInventory.GetItemBalance (StoreAssets.PLAYER_LIFE_ITEM_ID); 
		int currentAmount = DBManager.GetPlayerData (PlayerLife).AsInt;

		//if life count greater equal then max return
		if (currentAmount >= maxLifeRecover) 
		{
			return;
		}
		

		//if enter background date time is exist...calculate how many life to give to player
		//...otherwise analyze life count on game
		if (LastEnterBackgroundTime.IsFileExist ()) 
		{
			LastEnterBackgroundTime lBT = LastEnterBackgroundTime.Load ();

			//get back date time the moment enter background
			DateTime lastTimeBase = new DateTime (
				lBT.years, 
				lBT.months, 
				lBT.days, 
				lBT.hours, 
				lBT.minutes, 
				lBT.seconds);

			//calculate difference between now and date time enter background
			TimeSpan diff = DateTime.Now.Subtract (lastTimeBase);

			//total years
			int years = (int)diff.TotalDays / 365;

			//total months
			int months = (years*12)+((int)diff.TotalDays % 365);

			//the amount of life to give
			int giveAmount = 0;
			
			switch (timeUnit) 
			{
			case RecoverUnit.Unit_Year:

				//calcuate how many life should give
				int y_many = years / recoverPeriod;
				if (y_many >= 0) 
				{
					giveAmount = y_many;
					
					if ((currentAmount + giveAmount) > maxLifeRecover) 
					{
						giveAmount = maxLifeRecover - currentAmount;
					}
					
					
				}
				
				break;
				
			case RecoverUnit.Unit_Month:

				//calcuate how many life should give
				int m_many = months / recoverPeriod;
				if (m_many >= 0) 
				{
					giveAmount = m_many;
					
					if ((currentAmount + giveAmount) > maxLifeRecover)
					{
						giveAmount = maxLifeRecover - currentAmount;
					}
					
					
				}
				
				break;
				
			case RecoverUnit.Unit_Day:


				//calcuate how many life should give
				int d_many = (int)diff.TotalDays / recoverPeriod;
				if (d_many >= 0) 
				{
					
					giveAmount = d_many;
					
					if ((currentAmount + giveAmount) > maxLifeRecover) 
					{
						giveAmount = maxLifeRecover - currentAmount;
					}
					
					
				}
				
				break;
				
			case RecoverUnit.Unit_Hour:

				//calcuate how many life should give
				int h_many = (int)diff.TotalHours / recoverPeriod;
				if (h_many >= 0) 
				{
					
					giveAmount = h_many;
					
					if ((currentAmount + giveAmount) > maxLifeRecover) 
					{
						giveAmount = maxLifeRecover - currentAmount;
					}
					
					
				}
				
				break;
				
			case RecoverUnit.Unit_Minute:

				//calcuate how many life should give
				int mi_many = (int)diff.TotalMinutes / recoverPeriod;
				if (mi_many >= 0) 
				{
					
					giveAmount = mi_many;
					
					if ((currentAmount + giveAmount) > maxLifeRecover) 
					{
						giveAmount = maxLifeRecover - currentAmount;
					}
					
					
				}
				
				break;
				
			case RecoverUnit.Unit_Second:


				//calcuate how many life should give
				int s_many = (int)diff.TotalSeconds / recoverPeriod;
				if (s_many >= 0) 
				{
					
					giveAmount = s_many;
					
					if ((currentAmount + giveAmount) > maxLifeRecover) 
					{
						giveAmount = maxLifeRecover - currentAmount;
					}
					
					
				}
				
				break;
				
			}
			
			//give life to player
			GiveLife(giveAmount);
			Debug.Log("give "+giveAmount+" life to player");
		}
		else
		{
			AnalyzeLifeCountOnGame();
		}
	}

	/// <summary>
	/// Generates the notification.
	/// </summary>
	/// <returns>The notification.</returns>
	/// <param name="message">Message.</param>
	/// <param name="fireDateTime">Fire date time.</param>
	LocalNotification GenerateNotification(string message, DateTime fireDateTime)
	{
		LocalNotification newLN = new LocalNotification ();

		//get project name
		string[] s = Application.dataPath.Split('/');
		string projectName = s[s.Length - 2];

		newLN.alertAction = projectName;
		newLN.applicationIconBadgeNumber = -1;
		newLN.hasAction = true;
		newLN.repeatCalendar = CalendarIdentifier.GregorianCalendar;
		newLN.repeatInterval = CalendarUnit.Day;
		newLN.soundName = LocalNotification.defaultSoundName;
		newLN.alertBody = Localization.Get(message);
		newLN.fireDate = fireDateTime;

		NotificationServices.ScheduleLocalNotification (newLN);

		return newLN;
	}

	/// <summary>
	/// Cancel all notifications.
	/// </summary>
	/// <returns><c>true</c> if this instance cancel all notifications; otherwise, <c>false</c>.</returns>
	void CancelAllNotifications()
	{

		NotificationServices.CancelAllLocalNotifications ();
	}

	/// <summary>
	/// Gets the current life count.
	/// 
	/// max is 3
	/// </summary>
	/// <value>The life count.</value>
	public int LifeCount{ get{ return currentLifeCount; }}
}
