using UnityEngine;
using System.Collections;
/* This Code extracts a String from an URL  
     */
public class GetURL : MonoBehaviour
{
  
    public static string textFromWWW;
   
    public string url = "https://udvide.000webhostapp.com/test.txt"; // <-- enter your url here


    void Start()
    {
        
        WWW www = new WWW(url);
        StartCoroutine(GetTextFromWWW(www));

    }


    IEnumerator GetTextFromWWW(WWW www)
    {

       
        yield return www;

        if (www.error != null)
        {
            Debug.Log("Ooops, something went wrong...");
        }
        else
        {
            textFromWWW = www.text;
        }
    }
}