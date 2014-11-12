/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;


/// <summary>
/// simple script that, attached to a UIButton,
/// will load the assigned scene based on its name
/// </summary>
[AddComponentMenu("NGUI/Interaction/Button Scene Load")]
public class UIButtonScene : MonoBehaviour
{
    /// <summary>
    /// name of the scene to load
    /// </summary>
    public string sceneName;


    void OnClick()
    {
        if (!string.IsNullOrEmpty(sceneName))
            Application.LoadLevel(sceneName);
    }
}
