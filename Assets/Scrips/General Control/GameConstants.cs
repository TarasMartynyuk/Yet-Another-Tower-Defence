using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	StartingMenu, Game, Pause, EndGame
}

public class GameConstants
{
	//spawn consts
	public const int MaxEnemiesOnScreen = 8;
	public const int EnemiesPerSpawn = 8;
	public const float SpawnDelay = 1.3f;

	public const float ArcherTowerVertOffset = 0.25f;

	//resources
	public const int InitialHealth = 10;
	public const int InitialGold = 150;
	
	//enemy
	//in seconds
	public const float ActiveAfterDeath = 2;
	
	
	
}
