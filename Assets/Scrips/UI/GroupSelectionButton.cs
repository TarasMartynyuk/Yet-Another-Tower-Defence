using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GroupSelectionButton : GroupButton {

	private GroupPanel groupPanel;

	public GroupPanel GroupPanel
	{
		set{groupPanel = value;}
	}

	protected override void Start()
	{
		base.Start();
		this.onClick.AddListener(showGroupOngroupPanel);
	}

	private void showGroupOngroupPanel()
	{
		groupPanel.OnSelectedGroupChange(this);
	}

}
