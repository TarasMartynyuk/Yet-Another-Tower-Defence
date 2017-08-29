using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class GroupPanel : MonoBehaviour {

	[SerializeField] private Dropdown enemyTypeDropdownList;
	[SerializeField] private InputField enemyCountInputField;
	[SerializeField] private GroupButton groupRemoveButton;
	[SerializeField] private GroupNameInputField groupNameInputField;
	
	
	private Group group;

#region Unity init methods
	
	void Awake()
	{
		Assert.IsNotNull(enemyTypeDropdownList);
		Assert.IsNotNull(enemyCountInputField);
		Assert.IsNotNull(groupRemoveButton);
		Assert.IsNotNull(groupNameInputField);
	}

	void Start()
	{	
		List<String> enemyTypeNames = new List<String>(Enum.GetNames(typeof(EnemyType)));
		enemyTypeDropdownList.AddOptions(enemyTypeNames);
	}
#endregion

	//delegating method for buttons
	public void OnSelectedGroupChange(GroupButton selectedButton)
	{
		OnSelectedGroupChange(selectedButton.Group, selectedButton.GroupIndex);
	}

	public void OnSelectedGroupChange(Group newSelectedGroup, int groupIndex)
	{
		group = newSelectedGroup;
		updateGroupNameInputField(group, groupIndex);

		updateDropdownSelectedValue();

		UpdateInputFieldShownValue();

		groupRemoveButton.Group = group;

	}

#region Input  control methods
	

	/*ok, the thing should be checked to contain ONLY integers on the editor stage
	just have some faith*/
	public void OnInputFieldValueEntered(String value)
	{	
		int intValue;
		int.TryParse(value, out intValue);
		group.EnemyCount = intValue;
	}

	void updateDropdownSelectedValue()
	{
		enemyTypeDropdownList.value = (int)(group.Type);
	}

	public void OnEnemyTypeDropdownValueChanged(int optionIndex)
	{
		group.Type = (EnemyType)optionIndex;
	}

#endregion

	private void updateGroupNameInputField(Group newSelectedGroup, int groupIndex)
	{
		groupNameInputField.Group = newSelectedGroup;
		groupNameInputField.GroupIndex = groupIndex;
		groupNameInputField.UpdateText();
	}
	private void UpdateInputFieldShownValue()
	{	
		enemyCountInputField.text = group.EnemyCount.ToString();
	}

}
