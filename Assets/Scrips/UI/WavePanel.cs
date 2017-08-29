using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WavePanel : MonoBehaviour {

	[SerializeField] GroupPanel groupPanel;
	//prefab
	[SerializeField] GroupSelectionButton groupSelectionButton;
	//prefab
	[SerializeField] PauseInputField pauseInputField;
	///used only to give input field a reference to this object
	[SerializeField] GroupNameInputField groupNameInputField;
	
	

	private Wave wave;
	//TODO google for const Vector3
	private readonly Vector3 scaleEqualizer = new Vector3(1.0f, 1.0f, 1.0f);
	//child of wave panel - has a layout to handle button addition
	private Transform buttonPanel;
	private List<GroupSelectionButton> groupButtons;
	private List<PauseInputField> pauseInputFields;
	

	public Wave Wave
	{
		get{return wave;}
		set{wave = value;}
	}

	public GroupPanel GroupPanel
	{
		get{return groupPanel;}
	}

#region Unity init methods
	
	void Awake()
	{
		Assert.IsNotNull(groupPanel);
		Assert.IsNotNull(groupSelectionButton);
		Assert.IsNotNull(pauseInputField);
		groupButtons = new List<GroupSelectionButton>();
		pauseInputFields = new List<PauseInputField>();
		
		buttonPanel = this.gameObject.transform.Find("ButtonPanel");
		groupNameInputField.WavePanel = this;
	}

#endregion

	//NON-NULL
	public void OnSelectedWaveChange(Wave newSelectedWave)
	{ 
		wave = newSelectedWave;
		
		RemoveAllButtons();
		
		//add buttons for every group and pause of this wave
		for (int i = 0; i < wave.Groups.Count; i++)
		{	
			addGroupButton(wave.Groups[i], i);
			addPauseInputField(wave.Pauses[i], i);
		}

		//show the first group of selected wave on groupPanel
		groupPanel.OnSelectedGroupChange(newSelectedWave.Groups[0], 0);

	}

	public void AddNewGroup()
	{	
		if(wave.Groups.Count >= 6)
		{
			return;
		}

		wave.AddNewGroup();
		int lastElementIndex = wave.Groups.Count - 1;
		addGroupButton(wave.Groups[lastElementIndex], lastElementIndex);
		addPauseInputField(wave.Pauses[lastElementIndex], lastElementIndex);
	}

	public void RemoveGroup(GroupButton groupButton)
	{
		int groupIndex = wave.Groups.IndexOf(groupButton.Group);

		//dont let user delete the last group 
		if(groupIndex == 0 && wave.Groups.Count == 1)
		{
			return;
		}

	#region substitute the currently selected group in the groupPanel(which will be deleted) by another
	
		GroupSelectionButton nextSelectedButton;
		
		//check if group with next index exists
		if(groupIndex + 1 < groupButtons.Count && 
			(nextSelectedButton = groupButtons[groupIndex + 1]) != null)
		{
			//just wait 
		}
		else //there must be a group with previous index
		{
			nextSelectedButton = groupButtons[groupIndex - 1];
		}

		groupPanel.OnSelectedGroupChange(nextSelectedButton);
	#endregion

		GroupSelectionButton btn = groupButtons[groupIndex];
		groupButtons.Remove(btn);
		Destroy(btn.gameObject);

		
		PauseInputField pauseBtn = pauseInputFields[groupIndex];
		pauseInputFields.Remove(pauseBtn);
		Destroy(pauseBtn.gameObject);

		wave.Groups.RemoveAt(groupIndex);

	}

	public void RenameGroupAtIndex(string newName, int groupIndex)
	{
		wave.Groups[groupIndex].GroupName = newName;
		groupButtons[groupIndex].UpdateText();
	}


#region AddingButtons and InputFields
	
	private void addGroupButton(Group group, int groupIndex)
	{

		//instantiate a prefab and add it to buttonPanel - layout manager will do the rest
		GameObject createdGroupButton = Instantiate(groupSelectionButton.gameObject) as GameObject;
		createdGroupButton.transform.SetParent(buttonPanel);
		createdGroupButton.transform.localScale = scaleEqualizer;

		GroupSelectionButton groupButtonScriptObj = createdGroupButton.GetComponent<GroupSelectionButton>();
		groupButtons.Add(groupButtonScriptObj);
		groupButtonScriptObj.Group = group;
		groupButtonScriptObj.GroupPanel = groupPanel;
		groupButtonScriptObj.GroupIndex = groupIndex;
		

		groupButtonScriptObj.UpdateText();

	}

	private void addPauseInputField(float pause, int pauseListIndex)
	{
		//instantiate a prefab and add it to buttonPanel - layout manager will do the rest
		GameObject createdPauseButton = Instantiate(pauseInputField.gameObject) as GameObject;
		createdPauseButton.transform.SetParent(buttonPanel);
		createdPauseButton.transform.localScale = scaleEqualizer;

		//pass all needed data and references so that pausebutton is able to change corresponding pause
		PauseInputField pauseInputFieldScriptObj = createdPauseButton.GetComponent<PauseInputField>();
		pauseInputFields.Add(pauseInputFieldScriptObj);
		pauseInputFieldScriptObj.PauseValue = pause;
		pauseInputFieldScriptObj.PauseIndex = pauseListIndex;
		pauseInputFieldScriptObj.ParentWavePanel = this;

		pauseInputFieldScriptObj.UpdateText();
	}

	private void RemoveAllButtons()
	{
		List<Transform> children = new List<Transform>();
		foreach (Transform child in buttonPanel)
		{
			children.Add(child);
		}
		foreach (Transform child in children)
		{
			Destroy(child.gameObject);
		}
		groupButtons.Clear();
		pauseInputFields.Clear();
	}

#endregion

	public void changePause(float newValue, int pauseListIndex)
	{
		wave.Pauses[pauseListIndex] = newValue;
	}
	
}
