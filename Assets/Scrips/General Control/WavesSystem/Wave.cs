using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// represents a wave of enemies-
/// a bunch of groups created in defined order,
/// with defined pauses between creation
/// </summary>
[Serializable]
public class Wave {

	[SerializeField] private List<Group> groups;
	[SerializeField] private List<float> pauses;
	private String name;

#region Properties
	public List<Group> Groups{
		get{return groups;}
	}

	public List<float> Pauses{
		get{return pauses;}
	}

	public String Name{
		get{return name;}
	}
#endregion

	public Wave()
	{
		name = "some wave";
		groups = new List<Group>();
		pauses = new List<float>();
	}

	public void AddNewGroup()
	{
		groups.Add(new Group(this));
		pauses.Add(0f);
	}
	

	public override String ToString()
	{
		return name + " - is at index : " + WaveManager.Instance.WaveData.Waves.IndexOf(this);
	}


}
