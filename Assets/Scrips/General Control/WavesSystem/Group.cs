using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemyType
{
	Skeleton, ArmoredSkeleton, HighRankSkeleton
}

/// <summary>
/// represents a group -
/// some amount of enemies of the same type,
/// that spawn and move one after another, with
/// GameConstants.SpawnDelay time between each other
/// building block for Wave class
/// </summary>
[Serializable]
public class Group {

	
#region Properties

	[SerializeField] private String groupName;
	[SerializeField] private EnemyType type;
	[SerializeField] private int enemyCount;
	
	public EnemyType Type{ 
		get { return type;} 
		set {type = value;} 
	}

	public int EnemyCount{ 
		get { return enemyCount;} 
		set {enemyCount = value;} 
	}


	public String GroupName{ 
		get { return groupName;} 
		set {groupName = value;} 
	}

#endregion

	//Wow a constructor
	public Group(EnemyType type, int enemyCount)
	{
		this.Type = type;
		this.EnemyCount = enemyCount;
	}

	public Group(Wave parentWave)
	{
		GroupName = "Group #" + (parentWave.Groups.Count + 1);
		this.Type = EnemyType.Skeleton;
		this.EnemyCount = 0;
	}

	public Group(){}

	public override String ToString(){
		return GroupName;
	}
	

}
