using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// bound to a certain pause in a certain wave
/// shows current pause value,
/// processes user input and changes the boun pause value
/// </summary>
public class PauseInputField : InputField {

	private float pauseValue;
	private int pauseIndex;
	///must set explicitly
	private WavePanel parentWavePanel;

#region Properties
	
	public float PauseValue
	{
		set{pauseValue = value;}
	}

	public int PauseIndex
	{
		set{pauseIndex = value;}
	}


	public WavePanel ParentWavePanel
	{
		set{parentWavePanel = value;}
	}
#endregion

	protected override void Start()
	{
		base.Start();
		this.onEndEdit.AddListener(OnInputSubmit);
	}

	public void UpdateText()
	{
		this.text = pauseValue.ToString();
	}

	private void OnInputSubmit(string input)
	{
		float floatValue;
		float.TryParse(input, out floatValue);
		pauseValue = floatValue;

		parentWavePanel.changePause(pauseValue, pauseIndex);
	}
	
	
}