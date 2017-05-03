/********************************************************************************
 *    Project   : Awesomium.NET (Awesomium.Unity - AwesomiumSample)
 *    File      : WebUIBarHandler.cs
 *    Version   : 1.7.0.0 
 *    Date      : 08/08/2013
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
 *    Sample script used with all bars in the AwesomiumSample. Demonstrates 
 *    communication between the bars and the browser plane, using Awesomium's
 *    JavaScript Interoperation API.
 *    
 *    
 ********************************************************************************/

using System;
using UnityEngine;
using Awesomium.Core;
using Awesomium.Core.Data;
using Awesomium.Unity;

#region BarType
// Indicates the type of bar the script is
// assigned to.
public enum BarType
{
    Navigation,
    Menu,
    Status,
    Popup
}
#endregion

#region OpenDialog
enum OpenDialog
{
    None,
    Examples,
    Help
}
#endregion

/// <summary>
/// A subclass of WebUIScript that allows handling events of a WebUIComponent.
/// This script is added to all GUI WebUIComponents of the AwesomiumSample scene.
/// </summary>
public class WebUIBarHandler : WebUIScript
{
    #region Fields
    private static WebSession session;

    private WebUIComponent webBrowser;
    private WebUIComponent webUIPopup;
    private WebUIBrowserHandler webBrowserScript;
    [SerializeField]
    private BarType barType;
    #endregion


    #region Overrides
    // We override this to assign a WebSession to the WebUIComponent,
    // before it goes live. The component will go live after Start.
    protected override void Awake()
    {
        if ( Application.isEditor )
            return;

        if ( !webUI )
            return;

        // Get or create an in-memory WebSession.
        if ( session == null )
        {
            session = WebCore.CreateWebSession( new WebPreferences() { SmoothScrolling = true } );
            UnityEngine.Debug.Log( "Created In-Memory Session" );
        }

        if ( session.GetDataSource( "media" ) == null )
        {
            // Create and add a ResourceDataSource. This will be used to load assets
            // from the resources assembly (AwesomiumSampleResources) available with this sample.
            ResourceDataSource dataSource = new ResourceDataSource(
                ResourceType.Embedded,
                typeof( AwesomiumSampleResources.Loader ).Assembly );
            session.AddDataSource( "media", dataSource );

            UnityEngine.Debug.Log( "Added DataSource" );
        }

        // Assign the WebSession to the WebUIComponent.
        webUI.WebSession = session;
        UnityEngine.Debug.Log( String.Format( "WebSession Assigned to {0}.", barType ) );
    }

    // Use this for initialization.
    protected override void Start()
    {
        if ( Application.isEditor )
            return;

        UnityEngine.Debug.Log( String.Format( "WebUIBarHandler for {0} Started.", barType ) );

        // Check if there's a WebUIComponent available
        // in this GameObject.
        if ( !webUI )
            return;

        // Set a handler for the DocumentReady event.
        webUI.DocumentReady += OnDocumentReady;

        if ( !WebBrowser )
            return;

        // Handle some browser events depending on the kind
        // of bar we are.
        switch ( barType )
        {
            case BarType.Navigation:
                WebBrowser.AddressChanged += OnAddressChanged;
                break;

            case BarType.Menu:
                break;

            case BarType.Status:
                WebBrowser.TargetURLChanged += OnTargetURLChanged;
                WebBrowser.Crashed += OnCrashed;
                break;

            case BarType.Popup:
                break;
        }
    }

    // We do not need to handle IWebView.ShowCreatedWebView. WebUIScript handles this and
    // we can simply override OnShowCreatedWebView.
    protected override void OnShowCreatedWebView( WebUIComponent sender, ShowCreatedWebViewEventArgs e )
    {
        // Bars do not contain links and do not open popups.
        e.Cancel = true;
    }
    #endregion

    #region Properties
    // Gets the main browser component.
    private WebUIComponent WebBrowser
    {
        get
        {
            if ( webBrowser == null )
            {
                GameObject browserPlaneObj = GameObject.Find( "WebBrowserPlane" );

                if ( browserPlaneObj )
                    webBrowser = browserPlaneObj.GetComponent<WebUIComponent>();
            }

            return webBrowser;
        }
    }

    // Gets the popup WebUIComponent.
    private WebUIComponent Popup
    {
        get
        {
            if ( webUIPopup == null )
            {
                GameObject webUIPopupObj = GameObject.Find( "WebUIPopup" );

                if ( webUIPopupObj )
                    webUIPopup = webUIPopupObj.GetComponent<WebUIComponent>();
            }

            return webUIPopup;
        }
    }

    // Gets the WebUIScript we have attached to the browser's GameObject.
    private WebUIBrowserHandler WebBrowserScript
    {
        get
        {
            if ( webBrowserScript == null )
            {
                GameObject browserPlaneObj = GameObject.Find( "WebBrowserPlane" );

                if ( browserPlaneObj )
                    webBrowserScript = browserPlaneObj.GetComponent<WebUIBrowserHandler>();
            }

            return webBrowserScript;
        }
    }
    #endregion

    #region Event Handlers
    private void OnAddressChanged( object sender, UrlEventArgs e )
    {
        if ( !webUI.IsLive || !webUI.IsDocumentReady )
            return;

        // Update the URL in the address box.
        webUI.ExecuteJavascript( "updateURL('" + e.Url.ToString() + "')" );
    }

    private void OnTargetURLChanged( object sender, UrlEventArgs e )
    {
        if ( !webUI.IsLive || !webUI.IsDocumentReady )
            return;

        // Update the status text.
        webUI.ExecuteJavascript( "updateStatus('" + ( e.Url.IsBlank() ? String.Empty : e.Url.ToString() ) + "')" );
    }

    private void OnCrashed( object sender, CrashedEventArgs e )
    {
        if ( !webUI.IsLive || !webUI.IsDocumentReady )
            return;

        // Update the status text.
        webUI.ExecuteJavascript( "updateStatus('CRASHED!')" );
    }

    private void OnDocumentReady( object sender, UrlEventArgs e )
    {
        if ( WebBrowser )
            webUI.ExecuteJavascript( "updateURL('" + WebBrowser.Source.ToString() + "')" );

        // Create a global JavaScript object.
        JSObject webUIManager = webUI.CreateGlobalJavascriptObject( "webUIManager" );

        if ( webUIManager == null )
            return;

        using ( webUIManager )
        {
            // Add some methods and bind to them, depending on the kind
            // of bar we are. JavaScript in the bar's page calls these methods.
            switch ( barType )
            {
                case BarType.Navigation:
                    webUIManager.Bind( "navigateTo", false, OnNavigate );
                    webUIManager.Bind( "toggleBrowser", false, OnNavigate );
                    webUIManager.Bind( "goBack", false, OnNavigate );
                    webUIManager.Bind( "goForward", false, OnNavigate );
                    webUIManager.Bind( "stop", false, OnNavigate );
                    break;

                case BarType.Menu:
                    webUIManager.Bind( "examples", false, OnMenu );
                    webUIManager.Bind( "config", false, OnMenu );
                    webUIManager.Bind( "help", false, OnMenu );
                    webUIManager.Bind( "exit", false, OnMenu );
                    break;

                case BarType.Status:
                    break;

                case BarType.Popup:
                    webUIManager.Bind( "navigateTo", false, OnNavigate );
                    break;
            }
        }
    }

    // Handler of JavaScript methods called by the Navigation bar.
    private void OnNavigate( object sender, JavascriptMethodEventArgs e )
    {
        if ( !WebBrowser )
            return;

        switch ( e.MethodName )
        {
            case "navigateTo":
                if ( e.Arguments.Length == 0 )
                    return;

                string url = e.Arguments[ 0 ];

                if ( String.IsNullOrEmpty( url ) )
                    return;

                WebBrowser.Source = url.ToUri();

                if ( Popup && Popup.Visible )
                {
                    if ( WebBrowserScript )
                        WebBrowserScript.RestoreOpacity();
                    else
                        WebBrowser.Visible = true;

                    Popup.Visible = false;
                }

                WebBrowser.Focus();

                break;

            case "toggleBrowser":
                if ( !WebBrowserScript )
                    return;

                if ( WebBrowserScript.IsFading || ( Popup && Popup.Visible ) )
                    return;

                WebBrowserScript.FadeInOut();

                break;

            case "goBack":
                if ( WebBrowser.CanGoBack() )
                    WebBrowser.GoBack();
                break;

            case "goForward":
                if ( WebBrowser.CanGoForward() )
                    WebBrowser.GoForward();
                break;

            case "stop":
                if ( WebBrowser.IsLoading )
                    WebBrowser.Stop();
                break;
        }
    }

    // Handler of JavaScript methods called by the Menu bar.
    private void OnMenu( object sender, JavascriptMethodEventArgs e )
    {
        switch ( e.MethodName )
        {
            case "examples":
                TogglePopup( "examples" );
                break;

            case "config":
                webUI.Visible = !webUI.Visible;
                break;

            case "help":
                TogglePopup( "help" );
                break;

            case "exit":
                Application.Quit();
                break;
        }
    }

    // Toggles the popup window.
    private void TogglePopup( string page )
    {
        if ( WebBrowserScript.IsFading || !Popup || !WebBrowser )
            return;

        string assetPath = String.Format( "asset://media/{0}.html", page );

        if ( Popup.Visible && Popup.Source.ToString() == assetPath )
        {
            if ( WebBrowserScript )
                WebBrowserScript.RestoreOpacity();
            else
                WebBrowser.Visible = true;

            Popup.Visible = false;
            WebBrowser.Focus();
            return;
        }

        if ( !Popup.Visible )
        {
            if ( WebBrowserScript )
                WebBrowserScript.Transparent();
            else
                WebBrowser.Visible = false;
        }

        Popup.Source = assetPath.ToUri();
        Popup.Visible = true;
    }
    #endregion
}
