using UnityEngine;
using System.Collections;
using SIS;
using SimpleJSON;
using System;

public class SISDataSync : MonoBehaviour 
{
	const string syncDateTimeKeyToVal = "SyncDateTime";

	public delegate void OnSyncDataComplete();
	/// <summary>
	/// Event when sync data complete.
	/// </summary>
	public OnSyncDataComplete Evt_OnSyncDataComplete;

	public delegate void OnSyncDataFail();
	/// <summary>
	/// Event when sync data fail.
	/// </summary>
	public OnSyncDataFail Evt_OnSyncDataFail;

	public delegate void OnUploadDataComplete();
	/// <summary>
	/// Event when upload data complete.
	/// </summary>
	public OnUploadDataComplete Evt_OnUploadDataComplete;

	public delegate void OnUploadDataFail();
	/// <summary>
	/// Event when on upload data fail.
	/// </summary>
	public OnUploadDataFail Evt_OnUploadDataFail;

	public delegate void OnAccountLoginFromOtherDevice();
	/// <summary>
	/// Event when account login from other device.
	/// You should put player back to login section
	/// </summary>
	public OnAccountLoginFromOtherDevice Evt_OnAccountLoginFromOtherDevice;

	//last sync date time
	private string lastSyncDateTime;

	/// <summary>
	/// Is syncing data.
	/// </summary>
	private bool isSyncingData = false;

	/// <summary>
	/// Is uploading data.
	/// </summary>
	private bool isUploadingData = false;

	/// <summary>
	/// force pulling data from server.
	/// </summary>
	private bool pull = false;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnEnable()
	{
		ServerSync.SharedInstance.Evt_OnGetDataSuccess += OnGetDataSuccess;
		ServerSync.SharedInstance.Evt_OnGetDataFail += OnGetDataFail;
		ServerSync.SharedInstance.Evt_OnUploadDataSuccess += OnUploadDataSuccess;
		ServerSync.SharedInstance.Evt_OnUploadDataFail += OnUploadDataFailed;
		ServerSync.SharedInstance.Evt_OnInternetNotAvailable += OnInternetNotAvailable;
		ServerSync.SharedInstance.Evt_OnOtherDeviceLogin += OnOtherDeviceLogin;
	}

	void OnDisable()
	{
		ServerSync.SharedInstance.Evt_OnGetDataSuccess -= OnGetDataSuccess;
		ServerSync.SharedInstance.Evt_OnGetDataFail -= OnGetDataFail;
		ServerSync.SharedInstance.Evt_OnUploadDataSuccess -= OnUploadDataSuccess;
		ServerSync.SharedInstance.Evt_OnUploadDataFail -= OnUploadDataFailed;
		ServerSync.SharedInstance.Evt_OnInternetNotAvailable -= OnInternetNotAvailable;
		ServerSync.SharedInstance.Evt_OnOtherDeviceLogin -= OnOtherDeviceLogin;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void TriggerDelegateDelay(System.MulticastDelegate theDel, float delay)
	{
		System.Delegate[] delList = theDel.GetInvocationList ();

		for(int i=0; i<delList.Length; i++)
		{
			System.Delegate del = delList[i];

			del.DynamicInvoke();



		}
	}

	#region Upload data to srever only
	/// <summary>
	/// Uploads client data to server.
	/// Call this method after Sync success
	/// </summary>
	public void UploadData()
	{
		if(isSyncingData || isUploadingData)
		{
			return;
		}

		isUploadingData = true;

		//remember last sync DateTime
		lastSyncDateTime = DBManager.GetPlayerData (syncDateTimeKeyToVal).Value;

		//set synce time
		DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(DateTime.Now.ToString()));
		
		ServerSync.SharedInstance.UploadData(PlayerPrefs.GetString("data"));
	}
	#endregion Upload data to srever only

	#region Sync data from server
	/// <summary>
	/// Syncs client data with server.
	/// Could either be updated from server or update to server
	/// Call this method only once after login to server to update data.
	/// </summary>
	public void SyncData(bool forcePull = false)
	{
		if(isSyncingData)
		{
			return;
		}

		isSyncingData = true;

		pull = forcePull;

		//remember last sync DateTime
		if(!string.IsNullOrEmpty(DBManager.GetPlayerData(syncDateTimeKeyToVal)))
			lastSyncDateTime = DBManager.GetPlayerData (syncDateTimeKeyToVal).Value;
		
		//download data first
		ServerSync.SharedInstance.GetServerData ();

	}
	#endregion Sync data from server

	#region ServerSync callback
	void OnGetDataSuccess(ServerSync syncControl, string data)
	{
		//if server has no data we upload client data to server
		if(string.IsNullOrEmpty(data))
		{
			Debug.Log("Server has no data, upload client to server");

			//set synce time
			DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(DateTime.Now.ToString()));
			
			//upload client data
			ServerSync.SharedInstance.UploadData(PlayerPrefs.GetString("data"));

			return;
		}

		//server has data and client has synced before
		JSONNode jsonData = SimpleJSON.JSON.Parse (data);

		//client has data which not sync before, update from server
		if( string.IsNullOrEmpty(DBManager.GetPlayerData(syncDateTimeKeyToVal)))
		{
			Debug.Log("Client not sync before, update from server");

			PlayerPrefs.SetString("data", data);

			DBManager.GetInstance().Init();

			//set synce time
			DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(DateTime.Now.ToString()));

			if(Evt_OnSyncDataComplete != null)
			{
				TriggerDelegateDelay(Evt_OnSyncDataComplete, 1f);
			}
		}
		else//client has data which has sync before
		{ 
			//forec pulling data from server
			if(pull)
			{
				Debug.Log("Force pulling data from server");

				PlayerPrefs.SetString("data", data);

				DBManager.GetInstance().Init();

				//set synce time
				DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(DateTime.Now.ToString()));

				if(Evt_OnSyncDataComplete != null)
				{
					TriggerDelegateDelay(Evt_OnSyncDataComplete, 1f);
				}
				
				isSyncingData = false;

				return;
			}

			//get datetime for server one
			DateTime serverSyncDateTime = Convert.ToDateTime(jsonData["Player"][syncDateTimeKeyToVal].Value);

			//get datetime for client one
			DateTime clientSyncDateTime = Convert.ToDateTime(DBManager.GetPlayerData(syncDateTimeKeyToVal).Value);

			//compare
			int result = clientSyncDateTime.CompareTo(serverSyncDateTime);

			if(result < 0)//client is earlier than server(server win)
			{
				Debug.Log("Server win, update from server");

				PlayerPrefs.SetString("data", data);

				DBManager.GetInstance().Init();

				//set synce time
				DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(DateTime.Now.ToString()));

				if(Evt_OnSyncDataComplete != null)
				{
					TriggerDelegateDelay(Evt_OnSyncDataComplete, 1f);
				}

				isSyncingData = false;
			}
			else if(result > 0)//client is later than server(client win)
			{
				Debug.Log("Client win, upload client to server");

				//set synce time
				DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(DateTime.Now.ToString()));

				//upload client data
				ServerSync.SharedInstance.UploadData(PlayerPrefs.GetString("data"));

			}
			else//equal but client may still have newest data(upload client data)
			{
				/*
				if(Evt_OnSyncDataComplete != null)
				{
					TriggerDelegateDelay(Evt_OnSyncDataComplete, 1f);
				}

				isSyncingData = false;
				*/
				Debug.Log("Server client is same, upload client to server");

				//set synce time
				DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(DateTime.Now.ToString()));

				//upload client data
				ServerSync.SharedInstance.UploadData(PlayerPrefs.GetString("data"));
			}

		}
	}

	void OnGetDataFail(ServerSync syncControl, int errorCode)
	{
		if(!string.IsNullOrEmpty(DBManager.GetPlayerData(syncDateTimeKeyToVal)))
		{
			//reverse sync time
			DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(lastSyncDateTime));
		}

		isSyncingData = false;

		if(Evt_OnSyncDataFail != null)
		{
			TriggerDelegateDelay(Evt_OnSyncDataFail, 1f);
		}

	}

	void OnUploadDataSuccess(ServerSync syncControl)
	{
		if(isSyncingData)//was excuted as sync
		{
			isSyncingData = false;

			if(Evt_OnSyncDataComplete != null)
			{
				TriggerDelegateDelay(Evt_OnSyncDataComplete, 1f);
			}
		}
		else//was excuted as upload
		{
			isUploadingData = false;

			if(Evt_OnUploadDataComplete != null)
			{
				TriggerDelegateDelay(Evt_OnUploadDataComplete, 1f);
			}
		}
	}

	void OnUploadDataFailed(ServerSync syncControl, int errorCode)
	{
		if(!string.IsNullOrEmpty(DBManager.GetPlayerData(syncDateTimeKeyToVal)))
		{
			//reverse sync time
			DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(lastSyncDateTime));
		}

		if(isSyncingData)
		{
			isSyncingData = false;

			if(Evt_OnSyncDataFail != null)
			{
				TriggerDelegateDelay(Evt_OnSyncDataFail, 1f);
			}
		}
		else
		{
			isUploadingData = false;
			
			if(Evt_OnUploadDataFail != null)
			{
				TriggerDelegateDelay(Evt_OnUploadDataFail, 1f);
			}
		}

	}

	void OnInternetNotAvailable(ServerSync syncControl)
	{
		if(!string.IsNullOrEmpty(DBManager.GetPlayerData(syncDateTimeKeyToVal)))
		{
			//reverse sync time
			DBManager.SetPlayerData(syncDateTimeKeyToVal, new SimpleJSON.JSONData(lastSyncDateTime));
		}

		if(isSyncingData)
		{
			isSyncingData = false;

			if(Evt_OnSyncDataFail != null)
			{
				TriggerDelegateDelay(Evt_OnSyncDataFail, 1f);
			}
		}

		if(isUploadingData)
		{
			isUploadingData = false;

			if(Evt_OnUploadDataFail != null)
			{
				TriggerDelegateDelay(Evt_OnUploadDataFail, 1f);
			}
		}
	}

	void OnOtherDeviceLogin(ServerSync syncControl, int errorCode)
	{
		isSyncingData = false;
		isUploadingData = false;

		if(Evt_OnAccountLoginFromOtherDevice != null)
		{
			TriggerDelegateDelay(Evt_OnAccountLoginFromOtherDevice, 1f);
		}
	}
	#endregion ServerSync callback
}
