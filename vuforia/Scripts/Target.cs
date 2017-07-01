using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Target
{
	//public cause of JsonUtility
	public string name;
	public string text;

	public Target(string name, string text)
	{
		this.name = name;
		this.text = text;
	}

	public string GetName()
	{
		return name;
	}

	public string GetText()
	{
		return text;
	}

	public void SetText(string text)
	{
		this.text = text;
	}
}	
