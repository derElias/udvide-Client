using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.IO;
public class SimpleCloudHandlerJson : MonoBehaviour, ICloudRecoEventHandler
{

    private CloudRecoBehaviour mCloudRecoBehaviour;
    private ObjectTracker mImageTracker;
    private bool mIsScanning = false;
    private static string mTargetMetadata = "";
    private string path;
    private string jsonString;

    public ImageTargetBehaviour ImageTargetTemplate;
    private GameObject Text;
    public GameObject mBundleInstance = null;



    void Start()

    {


        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

        if (mCloudRecoBehaviour)
        {
            mCloudRecoBehaviour.RegisterEventHandler(this);
        }

    }


    public void OnInitialized()
    {
        Debug.Log("Cloud Reco initialized");
    }

    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
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

        path = Application.streamingAssetsPath + "/json1.json";
        jsonString = File.ReadAllText(path);


        Room Raum1 = JsonUtility.FromJson<Room>(jsonString);
        Text = new GameObject();
        Text.AddComponent<TextMesh>();

        Text.GetComponent<TextMesh>().text = "Room : " + Raum1.Raum + "\n" + "Name : " + Raum1.Name + "\n" + "E-Mail : " + Raum1.EMail + "\n" + "Tel. Nr : " + Raum1.Tel + "\n" + "Sprechzeiten :" + Raum1.Hours;
        Text.GetComponent<TextMesh>().richText = true;
        Text.GetComponent<TextMesh>().characterSize = 10;
        Text.transform.rotation = Quaternion.Euler(90, 0, 0);


        mTargetMetadata = targetSearchResult.MetaData;
       // StartCoroutine(GetTextFromWWW(mTargetMetadata));
        mCloudRecoBehaviour.CloudRecoEnabled = false;




        if (ImageTargetTemplate)
        {

            //mCloudRecoBehaviour.CloudRecoEnabled = true; /*Disables the Restart Scanning Button */ 
            mImageTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            ImageTargetBehaviour imageTargetBehaviour = (ImageTargetBehaviour)mImageTracker.TargetFinder.EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);

        }

    }

    void OnGUI()
    {

        GUI.Box(new Rect(100, 200, 300, 50), "Metadata: " + mTargetMetadata);
        if (!mIsScanning)
        {
            if (GUI.Button(new Rect(100, 400, 300, 50), "Restart Scanning"))
            {
                mCloudRecoBehaviour.CloudRecoEnabled = true;
            }
        }
    }
    [System.Serializable]
    public class Room
    {
       public string Raum, Art, Name, EMail, Tel, Hours ;
      

    }

    /*
     Creates a GameObject with a TextMesh that is using the Image Metadata 
     Image Metadata must be a a link with a .txt 
     for example https://test.000webhostapp.com/test.txt
         */
    /* IEnumerator GetTextFromWWW(string url)
     {

         WWW www = new WWW(url);

         yield return www;

         if (www.error == null)
         {




         }

     }*/


    // Update is called once per frame
    void Update () {
		
	}
}
