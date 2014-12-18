/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System.Collections;


/// <summary>
/// stretches a clipped UIPanel to match the size of another widget.
/// Takes the widget's anchor into account, if assigned
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Panel Stretch")]
public class UIPanelStretch : MonoBehaviour
{
    /// <summary>
    /// widget to stretch the Panel to
    /// </summary>
    public UIWidget widgetContainer = null;

    /// <summary>
    /// widget's anchor, should be assigned if it has one
    /// </summary>
    public Transform widgetAnchor = null;

    //store references for quick lookup
    Transform widgetTrans;
    Vector4 clipRange = Vector4.zero;
    UIPanel mPanel;
    UIScrollView mView;
    UIAnchor anchor;
    //update count, described below
    int times = 2;


    void Awake()
    {
        //get the panel of this gameobject
        mPanel = GetComponent<UIPanel>();
        mView = GetComponent<UIScrollView>();
        anchor = GetComponentInChildren<UIAnchor>();
        
    }


    void OnEnable()
    {
	    //get widget transform if set
        if (mPanel != null && widgetContainer != null)
            widgetTrans = widgetContainer.cachedTransform;
        //let the anchor update a few times
        if (anchor)
        {
            anchor.runOnlyOnce = false;
            anchor.enabled = true;
        }

        StartCoroutine(Refresh());
    }


    IEnumerator Refresh()
    {
        if (mPanel != null && widgetContainer != null)
        {
	        //create new rect, used for panel's clipping range
            Rect rect = new Rect();
            
	        //set temporary rect size to widget's size
            rect.width = widgetContainer.width;
            rect.height = widgetContainer.height;
            
            //cache clipping range of our panel
            clipRange = mPanel.baseClipRegion;

            //set the panel's clipping range to fit the widget's size:
	        //z = width, w = height
            clipRange.z = rect.width;
            clipRange.w = rect.height;

            //when updating the panel's position and offset, we need to
	        //wait until all other stretch and anchor scripts were executed.
	        //therefore we can't simply put this code in OnEnable() -
	        //here we update the panel's position a few times on every frame,
	        //until other scripts went through and we got the correct screen pos
            for(int i = 0; i < times; i++)
            {
		        //set correct local position,
		        //take anchor into account
		        //but ignore the widget's z position
                Vector3 lp = Vector3.zero;
                if (widgetAnchor != null)
                {
                    lp = widgetAnchor.localPosition;
                    lp += widgetTrans.localPosition;
                }
                else
                    lp = widgetTrans.localPosition;
                transform.localPosition = lp;

		        //set initial starting position of clipped panel,
		        //center offset to match the widget's center
                //assign new clipRange
                clipRange.x = clipRange.y = 0f;
                mPanel.baseClipRegion = clipRange;
                mPanel.clipOffset = new Vector2(0, -(clipRange.w / 2f));
                
                yield return new WaitForEndOfFrame();
            }

            mView.ResetPosition();
            //finally update the anchor one last time
            if (anchor) anchor.runOnlyOnce = true;
        }
    }
}
