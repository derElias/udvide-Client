using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonTest : MonoBehaviour {

	//Marker History
	static string pathToMarkerHistory;
	static string MarkerHistoryJsonString;

	void Start () {

		addMarkerToHistory ("TestName","test <b>content</b>");
		Debug.Log (getMarkerContent (getAllMarkers(), "TestName"));
	}

	public static void addMarkerToHistory(string name, string text)
	{
		pathToMarkerHistory = Application.streamingAssetsPath + "/markerHistory.json";
		MarkerHistoryJsonString = File.ReadAllText(pathToMarkerHistory);
		MarkerContainer mc = JsonUtility.FromJson<MarkerContainer>(MarkerHistoryJsonString);

		foreach (Marker mark in mc.markers)
		{
			if (mark.name == name)
			{
				mc.markers.Remove(mark);
				break;
			}
		}

		mc.markers.Add(new Marker(name, text));
		string markersjson = JsonUtility.ToJson(mc);
		File.WriteAllText(pathToMarkerHistory, markersjson);
	}

	public static List<Marker> getAllMarkers()
	{
		pathToMarkerHistory = Application.streamingAssetsPath + "/markerHistory.json";
		MarkerHistoryJsonString = File.ReadAllText(pathToMarkerHistory);
		MarkerContainer mc = JsonUtility.FromJson<MarkerContainer>(MarkerHistoryJsonString);

		return mc.markers;
	}

	public static string getMarkerContent(List<Marker> markerList, string markerName)
	{
		foreach (Marker mark in markerList)
		{
			if (mark.name == markerName)
			{
				return mark.getText();
			}
		}
		
		Debug.Log ("There is no Marker with name: " + markerName);
		return null;
	}

}

[System.Serializable]
public struct Marker
{
	public string name;
	public string text;

	public Marker(string name, string text)
	{
		this.name = name;
		this.text = text;
	}

	public string getName()
	{
		return name;
	}

	public string getText()
	{
		return text;
	}

	public void setText(string text)
	{
		this.text = text;
	}
}	

[System.Serializable]
public struct MarkerContainer 
{
	public List<Marker> markers;

	public List<Marker> getMarkers()
	{
		return markers;
	}
}