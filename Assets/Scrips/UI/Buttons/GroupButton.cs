using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupButton : Button {

	private Group group;
	private int groupIndex;
	private Text buttonText;

	public Group Group
	{
		get{return group;}
		set{group = value;}
	}

	public int GroupIndex
	{	
		get{return groupIndex;}
		set{groupIndex = value;}
	}

	protected override void Awake()
	{	
		base.Awake();
		buttonText = GetComponentInChildren<Text>();
	}

	public void UpdateText()
	{
		buttonText.text = group.GroupName;

	}
}
