/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// adjusts the collider of a widget at start,
/// slightly delayed to allow for runtime updates
/// </summary>
[AddComponentMenu("NGUI/UI/Update Collider")]
public class UIUpdateCollider : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        NGUITools.UpdateWidgetCollider(gameObject, true);
    }
}
