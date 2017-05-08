using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class link : MonoBehaviour
{

    protected bool checkPackageAppIsPresent(string package)
    {
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        //take the list of all packages on the device
        AndroidJavaObject appList = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);
        int num = appList.Call<int>("size");
        for (int i = 0; i < num; i++)
        {
            AndroidJavaObject appInfo = appList.Call<AndroidJavaObject>("get", i);
            string packageNew = appInfo.Get<string>("packageName");
            if (packageNew.CompareTo(package) == 0)
            {
                return true;
            }
        }
        return false;
    }



    // Use this for initialization
    public void OpenFHWS()
    {

        Application.OpenURL("https://www.fhws.de/"); //Open FHWS Homepage 

    }



    public void OpenFacebook()
    {

        if (checkPackageAppIsPresent("com.facebook.katana"))
        {
            Application.OpenURL("fb://www.facebook.com/FHWSiCampus/"); //there is Facebook app installed so let's use it
        }
        else
        {
            Application.OpenURL("https://www.facebook.com/FHWSiCampus/"); // no Facebook app - use built-in web browser
        }



    }
     
}
