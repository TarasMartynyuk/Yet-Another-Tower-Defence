using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WaveSelectionPanel : MonoBehaviour {

	[SerializeField] private WaveButton waveRemoveButton;
	[SerializeField] private WaveButton waveAddButton;
	
	[SerializeField] private Dropdown waveDropdownList;
	[SerializeField] private WavePanel wavePanel;
	
#region Unity init methods
	
	void Awake()
	{
		Assert.IsNotNull(waveRemoveButton);
		Assert.IsNotNull(waveDropdownList);
		Assert.IsNotNull(wavePanel);
		
	}
	void Start()
	{
		//modify add new wave button's button script
		waveAddButton.onClick.AddListener(WaveManager.Instance.WaveData.AddNewWave);
		waveAddButton.onClick.AddListener(refreshDropdownList);
		

		refreshDropdownList();
		//by default shot the first wave - which exists no matter what in WaveData
		OnWaveSelection(0);

	}

#endregion

	/// <summary>
	/// called when user changes selected value of
	/// dropDownList used for wave selection
	/// updates current shown wave by passing new selected wave to wavePanel
	/// updates wavebutton used for removing the wave's wave reference
	/// shows the first group of new wave in group panel
	/// </summary>
	/// <param name="waveOrdinal"></param>
	public void OnWaveSelection(int waveOrdinal)
	{	
		Wave selectedWave = WaveManager.Instance.WaveData.Waves[waveOrdinal];
		waveRemoveButton.Wave = selectedWave;
		wavePanel.OnSelectedWaveChange(selectedWave);
	}

	public void RemoveWave(WaveButton waveButton)
	{
		int waveIndex = WaveManager.Instance.WaveData.Waves.IndexOf(waveButton.Wave);

		//dont let user delete the last group 
		if(!selectWaveThatIsNextToIndex(waveIndex))
		{
			return;
		}
		
		WaveManager.Instance.WaveData.RemoveWave(waveButton.Wave);
		refreshDropdownList();
	}

	//TODO - optimize
	public void refreshDropdownList()
	{
		waveDropdownList.ClearOptions();
		waveDropdownList.AddOptions(WaveManager.Instance.WaveData.getListOfWaveOrdinals());
	}

	/// <summary>
	/// selects the wave to the right - preferably,
	/// or to the left to the wave at index *waveIndex*
	/// in the WaveData.Instance.Waves list in WaveData.
	/// </summary>
	/// <param name="waveIndex"></param>
	/// <returns>false if imposible to select new wave(only one left)
	/// true otherwise</returns>
	private bool selectWaveThatIsNextToIndex(int waveIndex)
	{	
		//imposible to get next wave if only one left
		if(waveIndex == 0 && WaveManager.Instance.WaveData.Waves.Count == 1)
		{
			return false;
		}

		if(waveIndex + 1 < WaveManager.Instance.WaveData.Waves.Count)
		{
			//check if wave with next index exists
			Wave nextSelectedWave = WaveManager.Instance.WaveData.Waves[waveIndex + 1];

			if(nextSelectedWave == null)
			{	
				//there must be a group with previous index
				nextSelectedWave = WaveManager.Instance.WaveData.Waves[waveIndex - 1];
			}
			
			wavePanel.OnSelectedWaveChange(nextSelectedWave);
		}




		return true;
	}
	
	

	
}
