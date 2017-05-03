/********************************************************************************
 *    Project   : Awesomium.NET (Awesomium.Unity - AwesomiumSample)
 *    File      : WebUIBrowserHandler.cs
 *    Version   : 1.7.0.0 
 *    Date      : 4/20/2013
 *    Author    : Perikles C. Stephanidis (perikles@awesomium.com)
 *    Copyright : �2013 Awesomium Technologies LLC
 *    
 *    This code is provided "AS IS" and for demonstration purposes only,
 *    without warranty of any kind.
 *     
 *-------------------------------------------------------------------------------
 *
 *    Notes     :
 *
 *    Sample script that demonstrates how to handle the events of a 
 *    WebUIComponent. This script adds a fade in/fade out feature to a
 *    WebUIComponent that renders with a Renderer. We use it for our
 *    main browser component in the AwesomiumSample scene.
 *    
 *    For a skeleton implementation of WebUIScript, see the 
 *    WebUIComponentHandler script available with the SDK.
 *    
 *    
 ********************************************************************************/

using System;
using UnityEngine;
using Awesomium.Core;
using Awesomium.Unity;
using System.Collections;

public class WebUIBrowserHandler : WebUIScript
{
    #region Fields
    private Color solidColor;
    private Color fadedColor;
    private Color transparentColor;
    private Color restoreColor;
    private bool browserInitialized;
    private bool fading;
    private bool faded = true; // The browser is initially faded.

    // The name of a preference setting used to save
    // the last loaded page.
    private const string LastURL = "LastURL";
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

        // Get or create a WebSession that synchronizes to disk.
        WebSession session = 
            WebCore.Sessions[ @"./Data" ] ??
            WebCore.CreateWebSession( @"./Data", new WebPreferences() { SmoothScrolling = true } );
        // Assign the WebSession to the WebUIComponent.
        webUI.WebSession = session;
        Debug.Log( "WebSession Assigned." );

        // Attempt to get the last URL setting. If one exists, load it;
        // otherwise, load the default.
        webUI.Source = PlayerPrefs.HasKey( LastURL ) && ( !String.IsNullOrEmpty( PlayerPrefs.GetString( LastURL ) ) ) ?
            PlayerPrefs.GetString( LastURL ).ToUri() :
            "http://www.awesomium.com".ToUri();
    }

    // Use this for initialization.
    protected override void Start()
    {
        if ( Application.isEditor )
            return;

        Debug.Log( "WebUIBrowserHandler Started." );

        if ( !webUI )
            return;

        // Set handlers for some events.
        webUI.LoadingFrameComplete += OnLoadingFrameComplete;
        webUI.AddressChanged += OnAddressChanged;
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
    #endregion

    #region Methods
    // Fades in or out the browser component.
    public void FadeInOut()
    {
        if ( fading )
            return;

        restoreColor = faded ? solidColor : fadedColor;

        if ( faded )
            StartCoroutine( FadeInOut( fadedColor, solidColor, 0.2f, false ) );
        else
            StartCoroutine( FadeInOut( solidColor, fadedColor, 0.2f, true ) );
    }

    // Fades out to transparent.
    public void Transparent()
    {
        if ( fading )
            return;

        StartCoroutine( FadeInOut( restoreColor, transparentColor, 0.6f, faded ) );
    }

    // Restores previous opacity.
    public void RestoreOpacity()
    {
        if ( fading )
            return;

        StartCoroutine( FadeInOut( transparentColor, restoreColor, 0.6f, faded ) );
    }

    private IEnumerator FadeInOut( Color fromColor, Color toColor, float duration, bool isFaded )
    {
        fading = true;

        for ( float t = 0.0f; t < duration; t += Time.deltaTime )
        {
            webUI.GetComponent<Renderer>().material.color = Color.Lerp( fromColor, toColor, t / duration );
            yield return null;
        }

        fading = false;
        faded = isFaded;
    }
    #endregion

    #region Properties
    public bool IsFading
    {
        get
        {
            return fading;
        }
    }

    public bool IsFaded
    {
        get
        {
            return faded;
        }
    }
    #endregion

    #region Event Handlers
    private void OnLoadingFrameComplete( object sender, FrameEventArgs e )
    {
        if ( browserInitialized )
            return;

        if ( !e.IsMainFrame )
            return;

        // Get some fixed FadeIn/FadeOut colors.
        Color matCol = webUI.GetComponent<Renderer>().material.color;
        solidColor = new Color( matCol.r, matCol.g, matCol.b, 1.0f );
        fadedColor = new Color( matCol.r, matCol.g, matCol.b, 0.9f );
        transparentColor = new Color( matCol.r, matCol.g, matCol.b, 0.0f );
        restoreColor = fadedColor;
        // Assign the faded color as initial color.
        webUI.GetComponent<Renderer>().material.color = fadedColor;
        // Focus the Browser component.
        webUI.Focus();
        // Prevent re-entrancy.
        browserInitialized = true;

        Debug.Log( "Browser Component Initialized." );
    }

    private void OnAddressChanged( object sender, UrlEventArgs e )
    {
        // The loaded URL has been updated.
        Debug.Log( String.Format( "Persisting Last URL: {0}", e.Url ) );
        // By default Unity writes these preferences to disk 
        // on Application Quit.
        PlayerPrefs.SetString( LastURL, e.Url.AbsoluteUri );
    }
    #endregion
}
