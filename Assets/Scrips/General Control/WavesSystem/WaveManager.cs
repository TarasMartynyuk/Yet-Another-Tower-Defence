using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System;
using System.IO;

/// <summary>
/// spawns waves of enemies
/// and keeps track of them
/// </summary>
public class WaveManager : Singleton<WaveManager> {

	[SerializeField] private GameObject spawnPoint;
	
	[SerializeField] private GameObject[] enemyPrefabs;
	[SerializeField] private Text waveBanner;
	private WaveData waveData;

	public WaveData WaveData { get{return waveData;}}
	
	protected override void Awake()
	{
		base.Awake();
		
		Assert.IsNotNull(spawnPoint);
		Assert.IsNotNull(enemyPrefabs);
		Assert.IsNotNull(waveBanner);		

		waveData = new WaveData();
		try
		{
			waveData = WaveData.LoadFromFile(waveData);
		}
		catch (Exception e)
		{
			print(e.Message);
		}
		
		if(waveData.Waves.Count == 0)
		{
			waveData.AddNewWave();
		}

	} 

	
	#region  save & load

		public void SaveWaves()
		{
			waveData.SaveWavesToFile();
		}
		
	#endregion
	
	public IEnumerator SpawnLevel()
	{

		for (int i = 0; i < waveData.Waves.Count; i++)
		{
			waveBanner.text = "Wave " + (i+1) + " / " +  waveData.Waves.Count;
			yield return StartCoroutine(SpawnWave(waveData.Waves[i]));
			
		}

		//start checking if all enemies are killed - if so - won

		yield return CheckIfAllEnemiesKilled();
	}



	/// <summary>
	/// spawns 1 wave of enemies
	/// </summary>
	/// <returns></returns>
	private IEnumerator SpawnWave(Wave wave)
	{

		Group currentGroup;

		for(int i = 0; i < wave.Groups.Count; i++)
		{
			//spawn a group
			currentGroup = wave.Groups[i];

			for(int j = 0; j < currentGroup.EnemyCount; j++)
			{
				yield return StartCoroutine(spawnEnemy(currentGroup.Type));
			}

			//pause before next group
			yield return GameManager.Instance.Sync(wave.Pauses[i]);
		}
	}


	/// <summary>
	/// spawns an enemy of type defined by enemyType
	/// </summary>
	private IEnumerator spawnEnemy(EnemyType type)
	{
		GameObject createdEnemy = Instantiate(enemyPrefabs[(int) type]) as GameObject;
		createdEnemy.transform.position = spawnPoint.transform.position;
		GameManager.Instance.RegisterEnemy(createdEnemy.GetComponent<Enemy>());

		yield return GameManager.Instance.Sync((type == EnemyType.Skeleton? 0.8f : GameConstants.SpawnDelay));
		
	}

	private IEnumerator CheckIfAllEnemiesKilled()
	{
		WaitForSeconds pause = new WaitForSeconds(1);

		while(GameManager.Instance.EnemiesOnScreen.Count != 0)
		{
			yield return pause;
		}

		GameManager.Instance.transferToEndGameState(true);

	}
	
}
