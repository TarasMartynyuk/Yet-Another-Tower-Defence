using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


[Serializable]
public class WaveData {

	[SerializeField] private List<Wave> waves;
	private const String savedDataFilename = "waveData.json";
	public List<Wave> Waves
	{
		get{return waves;}
	}



	public WaveData()
	{
		waves = new List<Wave>();
	}

	/// <summary>
	/// loads data from the json file to waveData object
	/// </summary>
	/// <exception cref="IOException"  - when could not find or read from file></exception>
	public static WaveData LoadFromFile(WaveData waveData)
	{
		string jsonData;

		using(TextReader tr = new StreamReader(
			Path.Combine(Application.streamingAssetsPath, savedDataFilename)))
		{
			jsonData = tr.ReadToEnd();
		}

		WaveData result = JsonUtility.FromJson<WaveData>(jsonData);

		if(result == null)
		{
			throw new IOException("could not retreive object form json data file");
		}
		else
		{
			return result;
		}
	}

	public void AddNewWave()
	{	
		Wave newWave = new Wave();
		// create the first group with default settings
		newWave.AddNewGroup();
		
		waves.Add(newWave);
		
	}
	
	public void RemoveWave(Wave wave)
	{	
		waves.Remove(wave);
	}

	

	/// <summary>
	/// returns list of Strings being ordinal numbers of all waves,
	/// or empty list if no waves defined
	/// </summary>
	public List<String> getListOfWaveOrdinals()
	{	
		List<String> waveOrdinals = new List<String>(waves.Count);

		for (int i = 1; i <= waves.Count; i++)
		{	
			
			waveOrdinals.Add(i.ToString());
		}

		return waveOrdinals;
	}

	/// <summary>
	/// writes the current wave structure and data to the file
	/// </summary> 
	/// <exception cref="IOException"  - when could not find or read from file></exception>
	public void SaveWavesToFile()
	{
		string json = JsonUtility.ToJson(this);

		using (StreamWriter swr = new StreamWriter(
			File.Create(Path.Combine(Application.streamingAssetsPath, savedDataFilename))))
		{
			swr.Write(json);
		}
		
	}



}
