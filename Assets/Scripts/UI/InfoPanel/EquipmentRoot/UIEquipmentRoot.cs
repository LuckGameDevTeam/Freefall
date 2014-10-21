using UnityEngine;
using System.Collections;

/// <summary>
/// UI equipment root.
/// 
/// The root of equipment in info panel
/// </summary>
public class UIEquipmentRoot : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Open equipment in info panel.
	/// </summary>
	public virtual void Open()
	{
		gameObject.SetActive (true);
	}

	/// <summary>
	/// Close equipment in info panel.
	/// </summary>
	public virtual void Close()
	{
		gameObject.SetActive (false);
	}
}
