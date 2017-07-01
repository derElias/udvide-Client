using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetContainer 
{
	//public cause of JsonUtility
	public List<Target> targets;

	public TargetContainer(List<Target> targets)
	{
		this.targets = targets;
	}

	public List<Target> GetTargets()
	{
		return targets;
	}

	public void SetMarkers(List<Target> targets)
	{
		this.targets = targets;
	}
		
}