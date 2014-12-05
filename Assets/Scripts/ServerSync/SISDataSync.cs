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

	//last sync date time
	private string lastSyncDateTime;

	/// <summary>
	/// Is syncing data.
	/// </summary>
	private bool isSyncingData = false;

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
	}

	void OnDisable()
	{
		ServerSync.SharedInstance.Evt_OnGetDataSuccess -= OnGetDataSuccess;
		ServerSync.SharedInstance.Evt_OnGetDataFail -= OnGetDataFail;
		ServerSync.SharedInstance.Evt_OnUploadDataSuccess -= OnUploadDataSuccess;
		ServerSync.SharedInstance.Evt_OnUploadDataFail -= OnUploadDataFailed;
		ServerSync.SharedInstance.Evt_OnInternetNotAvailable -= OnInternetNotAvailable;
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

			Invoke(del.Method.Name, delay);
		}
	}

	#region Upload data to srever only
	public void UploadData()
	{

	}
	#endregion Upload data to srever only

	#region Sync data from server
	public void SyncData()
	{
		if(isSyncingData)
		{
			return;
		}

		isSyncingData = true;

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
			Invoke("UploadData", 0.1f);

			return;
		}

		//server has data and client has synced before
		JSONNode jsonData = SimpleJSON.JSON.Parse (data);

		//client has data which not sync before, update from server
		if( string.IsNullOrEmpty(DBManager.GetPlayerData(syncDateTimeKeyToVal)))
		{
			PlayerPrefs.SetString("data", data);

			DBManager.GetInstance().Init();

			if(Evt_OnSyncDataComplete != null)
			{
				TriggerDelegateDelay(Evt_OnSyncDataComplete, 1f);
			}
		}
		else//client has data which has sync before
		{ 
			DateTime serverSyncDateTime = Convert.ToDateTime(jsonData[syncDateTimeKeyToVal].Value.Replace("\"", ""));
			DateTime clientSyncDateTime = Convert.ToDateTime(DBManager.GetPlayerData(syncDateTimeKeyToVal).Value.Replace("\"", ""));

			int result = clientSyncDateTime.CompareTo(serverSyncDateTime);

			if(result < 0)//client is earlier than server(server win)
			{
				PlayerPrefs.SetString("data", data);

				DBManager.GetInstance().Init();

				if(Evt_OnSyncDataComplete != null)
				{
					TriggerDelegateDelay(Evt_OnSyncDataComplete, 1f);
				}

				isSyncingData = false;
			}
			else if(result > 0)//client is later than server(client win)
			{
				Invoke("UploadData", 0.1f);

			}
			else//equal
			{
				if(Evt_OnSyncDataComplete != null)
				{
					TriggerDelegateDelay(Evt_OnSyncDataComplete, 1f);
				}

				isSyncingData = false;
			}


		}
	}

	void OnGetDataFail(ServerSync syncControl, int errorCode)
	{
	}

	void OnUploadDataSuccess(ServerSync syncControl)
	{
		if(isSyncingData)
		{
		}
		else
		{

		}
	}

	void OnUploadDataFailed(ServerSync syncControl, int errorCode)
	{
	}

	void OnInternetNotAvailable(ServerSync syncControl)
	{

	}
	#endregion ServerSync callback
}
