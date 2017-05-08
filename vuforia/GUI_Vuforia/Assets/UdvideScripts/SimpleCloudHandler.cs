using System;
using UnityEngine;
using System.Collections;
using Vuforia;


/*  This Script handles the Vuforia Cloud Recognition System.
*  Building a GUI to visualize the connection with the Vuforia Server.



*/
public class SimpleCloudHandler : MonoBehaviour, ICloudRecoEventHandler {
	
	private CloudRecoBehaviour mCloudRecoBehaviour;
	private ObjectTracker mImageTracker;
    private bool mIsScanning = false;
    public string textFromWWW;
    private static string mTargetMetadata =  "";


    public ImageTargetBehaviour ImageTargetTemplate;
    public GameObject Text;
    public GameObject mBundleInstance = null;


    void Start () 

	{
        
        
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
       
		if (mCloudRecoBehaviour)
        {
            mCloudRecoBehaviour.RegisterEventHandler(this);
        }
		
	}


public void OnInitialized() 
	{
    Debug.Log ("Cloud Reco initialized");
	}
 
	public void OnInitError(TargetFinder.InitState initError) 
	{
    Debug.Log ("Cloud Reco init error " + initError.ToString());
	}
 
	public void OnUpdateError(TargetFinder.UpdateState updateError)
	{
    Debug.Log ("Cloud Reco update error " + updateError.ToString());
	}
	
	public void OnStateChanged(bool scanning) 
	{
    mIsScanning = scanning;
         
    if (scanning)
    {
          mImageTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            mImageTracker.TargetFinder.ClearTrackables(false);
    }
	}

    

    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
	{
       
        DestroyImmediate(Text);
        mTargetMetadata = targetSearchResult.MetaData;


        WWW www = new WWW(mTargetMetadata);
      // Debug.Log(mTargetMetadata);
        StartCoroutine(WaitForRequest(www));
        Text = new GameObject("text1");
        Text.AddComponent<TextMesh>();

        Text.GetComponent<TextMesh>().text = textFromWWW ;
        //Debug.Log(textFromWWW);
        Text.GetComponent<TextMesh>().richText = true;
        Text.GetComponent<TextMesh>().characterSize = 10;
        Text.transform.rotation = Quaternion.Euler(90,0,0);
        mCloudRecoBehaviour.CloudRecoEnabled = false;
		
		
	if (ImageTargetTemplate)
	{
            mCloudRecoBehaviour.CloudRecoEnabled = true;
    mImageTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
    ImageTargetBehaviour imageTargetBehaviour = (ImageTargetBehaviour)mImageTracker.TargetFinder.EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);
          
        }
		
    }
	

	void OnGUI() {

        GUI.Box (new Rect(100,200,300,50), "Metadata: " + mTargetMetadata); 
        if (!mIsScanning) {
             if (GUI.Button(new Rect(100,400,300,50), "Restart Scanning")) {
                 mCloudRecoBehaviour.CloudRecoEnabled = true;
             }
         }
    }
    IEnumerator WaitForRequest(WWW www)
    {

        // Wait for download to complete
        yield return www;

      textFromWWW = www.text;
        Debug.Log(textFromWWW);
        
        
    }
    void Update () {
	

	}
}
