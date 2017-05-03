/********************************************************************************
 *    Project   : Awesomium.NET (Awesomium.Unity)
 *    File      : WebUIComponentHandler.cs
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
 *    Sample script that demonstrates how to handle the events of a 
 *    WebUIComponent.
 *    
 *    
 ********************************************************************************/

using System;
using UnityEngine;
using Awesomium.Core;
using Awesomium.Core.Data;
using Awesomium.Unity;

/// <summary>
/// Script that allows handling events of a WebUIComponent.
/// Add this script to the same game object you added a WebUIComponent.
/// </summary>
public class WebUIComponentHandler : WebUIScript
{
    // We override this to assign a WebSession to the WebUIComponent,
    // before it goes live. The component will go live after Start.
    protected override void Awake()
    {
        if ( !webUI )
            return;

        // Get or create a WebSession that synchronizes to disk.
        WebSession session = 
            WebCore.Sessions[ @"./Data" ] ?? 
            WebCore.CreateWebSession( @"./Data", new WebPreferences() { SmoothScrolling = true } );
        // Assign the WebSession to the WebUIComponent.
        webUI.WebSession = session;
        UnityEngine.Debug.Log( "WebSession Assigned." );
    }
    
    // Use this for initialization.
    protected override void Start()
    {
        UnityEngine.Debug.Log( "WebUIComponentHandler Started." );

        // Set a handler for the DocumentReady event.
        if ( webUI )
            webUI.DocumentReady += OnDocumentReady;
    }

    // We do not need to handle IWebView.ShowCreatedWebView. WebUIScript handles this and
    // we can simply override OnShowCreatedWebView.
    protected override void OnShowCreatedWebView( WebUIComponent sender, ShowCreatedWebViewEventArgs e )
    {
        // Note that, if you do not override OnShowCreatedWebView, 
        // IWebView.ShowCreatedWebView will be canceled by default.
        e.Cancel = true;

        // For this sample, we simply load the page to the same view,
        // if the target URL is from the same domain.
        if ( ( e.TargetURL != null ) && ( String.Compare( e.TargetURL.Host, sender.Source.Host, true ) == 0 ) )
        {
            Debug.Log( String.Format( "Navigating to: {0}", e.TargetURL ) );
            webUI.Source = e.TargetURL;
        }
    }

    private void OnDocumentReady( object sender, UrlEventArgs e )
    {
        //
    }
}
