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
    private string mTargetMetadata = "";
	public ImageTargetBehaviour ImageTargetTemplate;
	
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
	
        mTargetMetadata = targetSearchResult.MetaData;
        mCloudRecoBehaviour.CloudRecoEnabled = false;
		
		
	if (ImageTargetTemplate)
	{
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
	void Update () {
	
	}
}
