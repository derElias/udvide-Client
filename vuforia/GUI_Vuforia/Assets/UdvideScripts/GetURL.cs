using UnityEngine;
using System.Collections;

public class GetURL : MonoBehaviour
{
  
    private string textFromWWW;
    public static string test = "https://udvide.000webhostapp.com/test.txt";
    public string url = test ; // <-- enter your url here

    
    void Start()
    {
        StartCoroutine(GetTextFromWWW());
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 200), textFromWWW);
    }

    IEnumerator GetTextFromWWW()
    {
        WWW www = new WWW(url);

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