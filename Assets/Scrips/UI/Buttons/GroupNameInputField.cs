using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupNameInputField : InputField {

	private Group group;
    private int groupIndex;
    private WavePanel wavePanel;

#region Properties
    public Group Group
    {
        set{group = value;}
    }

    public int GroupIndex
    {
        set{groupIndex = value;}
    }

    public WavePanel WavePanel
    {
        set{wavePanel = value;}
    }
#endregion

    protected override void Awake()
    {
        base.Awake();
        this.onEndEdit.AddListener(OnInputSubmit);
    }

    private void OnInputSubmit(string input)
    {
        wavePanel.RenameGroupAtIndex(input, groupIndex);
        
    }

    public void UpdateText()
    {
        this.text = group.GroupName;
    }

	
}
