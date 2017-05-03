/********************************************************************************
 *    Project   : Awesomium.NET (Awesomium.Unity - AwesomiumSample)
 *    File      : CameraDrag.cs
 *    Version   : 1.7.0.0 
 *    Date      : 8/28/2013
 *    Author    : Perikles C. Stephanidis (perikles@awesomium.com)
 *    Copyright : ©2013 Awesomium Technologies LLC
 *    
 *    This code is provided "AS IS" and for demonstration purposes only,
 *    without warranty of any kind.
 *     
 *-------------------------------------------------------------------------------
 *
 *    Notes     :
 *
 *    Sample script for dragging the camera using the right mouse button.
 *    
 *    
 ********************************************************************************/

using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    void Update()
    {
        if ( Input.GetMouseButtonDown( 1 ) )
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if ( !Input.GetMouseButton( 1 ) )
            return;

        Vector3 pos = Camera.main.ScreenToViewportPoint( Input.mousePosition - dragOrigin );
        Vector3 move = new Vector3( pos.x * dragSpeed, 0, pos.y * dragSpeed );
        transform.Translate( move, Space.World );
    }
}