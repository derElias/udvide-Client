/********************************************************************************
 *    Project   : Awesomium.NET (Awesomium.Unity)
 *    File      : WebCoreInitializer.cs
 *    Version   : 1.7.0.0 
 *    Date      : 4/20/2013
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
 *    Sample script that demonstrates how to explicitly initialize the 
 *    WebCore specifying custom configuration.
 *    
 *    
 ********************************************************************************/

using UnityEngine;
using Awesomium.Core;
using System.Collections;

/// <summary>
/// Script that allows initialization of the WebCore.
/// Add this script to any object, but no more than once.
/// </summary>
public class WebCoreInitializer : MonoBehaviour {
	
	// Awake is called only once during the lifetime
	// of the script instance and always before any Start functions
	// are called. This is the best place to initialize our WebCore.
	void Awake()
	{
		if ( !WebCore.IsInitialized )
			WebCore.Initialize( new WebConfig() { LogLevel = LogLevel.Verbose } );
	}
}
