using System.Collections.Generic;
using UnityEngine;
using System.IO;

//This class covers methodes for the targetHistory.json file 
public static class TargetHistory
{

    //returns the path where the targetHistory.json file is stored
    //file gets created if it doesn't exist yet
	public static string GetHistoryPath()
	{
		string path = (Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Application.dataPath) + "/targetHistory.json";
		if (!File.Exists(path))
		{
			File.Create(path).Close();
			ClearHistory();
		}
		return path;
	}

    //returns the whole text of the targetHistory.json file
	public static string ReadFromHistory()
	{
		return File.ReadAllText(GetHistoryPath());
	}

    
	public static void WriteToHistory(TargetContainer tc)
	{
		File.WriteAllText(GetHistoryPath(), JsonUtility.ToJson(tc));
	}

    
	public static void ClearHistory()
	{
		File.WriteAllText(GetHistoryPath(), "{\"targets\":[]}");
	}

    //returns an Object of MarkerContainer, containing all the targets in the current targetHistory.json file
	public static TargetContainer GetTargetContainer()
	{
		return JsonUtility.FromJson<TargetContainer>(ReadFromHistory());
	}

	//Deletes the previous Target with the same name if it exists and then adds the Target to the History
	//This way the TargetText will be updated in case it changed and the most recently scanned Target is always at the end of the List
	public static void AddTarget(Target mTarget)
	{
		DeleteTarget(mTarget.GetName());
		TargetContainer tc = GetTargetContainer();
		tc.GetTargets().Add(mTarget);
		WriteToHistory(tc);
	}

    //Returns a List with all Targets that are currently stored in the targetHistory.json file
	public static List<Target> GetTargetList()
	{
		return GetTargetContainer().GetTargets();
	}

    //searches for a Target in the targetHistory.json file that has the same name as targetName
    //if such a Target exists, return it
	public static Target GetTarget(string targetName)
	{
		List<Target> targetList = GetTargetList();
		foreach (Target target in targetList)
		{
			if (target.GetName() == targetName)
			{
				return target;
			}
		}
		return null;
	}

    //returns true if a Target with targetName already exists
	public static bool Exists(string targetName)
	{
		if (GetTarget (targetName) != null)
			return true;
		return false;
	}

	//returns the most recently scanned Target
	public static Target GetLastTarget()
	{
		Target[] targets = GetTargetList().ToArray();
		if(targets.Length > 0)
			return targets [targets.Length - 1];
		return null;
	}

	public static void DeleteTarget(string targetName)
	{
		TargetContainer tc = GetTargetContainer();
		if (Exists(targetName))
		{
			foreach (Target target in tc.GetTargets())
			{
				if (target.GetName() == targetName)
				{
					tc.GetTargets().Remove(target);
					WriteToHistory(tc);
					break;
				}
			}
		}
	}
}