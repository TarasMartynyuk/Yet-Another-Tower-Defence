using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


/// <summary>
/// class for managing general game proccesses -
/// enemy spawning and keeping track of active enemies,
/// health and UI control
/// </summary>
public class GameManager : Singleton<GameManager> {

	//UI label
	[SerializeField] private Text healthAmount;
	[SerializeField] GameObject[] UIPanels;
	
	
	private GameState currentState;
	private List<Enemy> enemiesOnScreen;
	private int health;
	//for stats only
	private int enemiesKilled;

#region Properties
	public List<Enemy> EnemiesOnScreen{
		get {return enemiesOnScreen;}
	}

	public int Health{
		get{return health;}
	}

	public GameState CurrentState{
		get{return currentState;}
		set{currentState = value;}
	}
#endregion

	protected override void  Awake()
	{
		base.Awake();
		Assert.IsNotNull(healthAmount);
		Assert.IsTrue(UIPanels[0].tag == "startMenu");
		Assert.IsTrue(UIPanels[1].tag == "gameMenu");
		Assert.IsTrue(UIPanels[4].tag == "endGameMenu");
		Assert.IsTrue(UIPanels[5].tag == "pauseMenu");
	}
	
	void Start () {

		enemiesOnScreen = new List<Enemy>();
		SetGlobalValuesToInitialState();

		currentState = GameState.StartingMenu;
		showPanelWithTag("startMenu");
		

	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			transferToPauseState();
		}
	}
	

#region  methods to manage enemies

	public void RegisterEnemy(Enemy enemy){
		enemiesOnScreen.Add(enemy);
	}

	public void unregisterEnemy(Enemy enemy)
	{
		enemiesOnScreen.Remove(enemy);
		Destroy(enemy.gameObject);
	}

	public void EnemyKilled(Enemy enemy){
		unregisterEnemy(enemy);
		enemiesKilled++;
	}

	public void EnemyEscaped(Enemy enemy)
	{
		health -= enemy.ExitReachedDamage;
		if(health <= 0)
		{
			transferToEndGameState(false);
		} 
		else 
		{
			reevaluateTextLabel();
		}	
		unregisterEnemy(enemy);
	}


	public void destroyAllEnemies(){
		foreach(Enemy en in enemiesOnScreen){
			Destroy(en.gameObject);
		}
		enemiesOnScreen.Clear();
	}

	
	#endregion

#region  methods to manage currentState
	public void transferToGameState()
	{
		SoundManager.Instance.PlaySoundsOnGameStart();
		StartCoroutine(WaveManager.Instance.SpawnLevel());

		currentState = GameState.Game;
		showPanelWithTag("gameMenu");
		UIPanels[1].SetActive(false);
	}

	private void transferToPauseState()
	{
		showPanelWithTag("pauseMenu");
		currentState = GameState.Pause;
		pauseAllEnemiesAnimations();
	}

	public void continueGameAfterPause()
	{
		currentState = GameState.Game;
		showPanelWithTag("gameMenu");
		continueAllEnemiesAnimations();
	}

	/// <summary>
	/// transfers game to EndGame state,
	/// If haveWon is true, shows congrats message, othervise - gameOver one
	/// destroys all objects created during the game,
	/// </summary>
	/// <param name="haveWon"></param>
	public void transferToEndGameState(bool haveWon){

		StopAllCoroutines();
		destroyAllEnemies();
		TowerManager.Instance.ClearAllObjectsCreatedDuringGame();

		currentState = GameState.EndGame;
		showPanelWithTag("endGameMenu");
		UIPanels[4].transform.Find("EndGamePanel/EnemiesKilledValueLabel").gameObject.GetComponent<Text>().text = enemiesKilled.ToString();
		UIPanels[4].transform.Find("FlagBanner/WonOrGameOverText").gameObject.GetComponent<Text>().text = 
			haveWon? "You won!" : "You lost!";

		SoundManager.Instance.StopPlayingLevelSound();

		if(haveWon)
		{
			SoundManager.Instance.PlayWonSound();
		}
		else
		{
			SoundManager.Instance.PlayGameOverSound();
		}
	}

	public void transferToStartingMenuState(){
		
		SetGlobalValuesToInitialState();
		TowerManager.Instance.SetGlobalValuesToInitialState();
		showPanelWithTag("startMenu");
	}

	public void quitApp(){
		Application.Quit();
	}

	#endregion


#region pause support

	public  Coroutine Sync(float seconds){
        return StartCoroutine(PauseRoutine(seconds)); 
    }

	/// <summary>
	/// pauses itself while GameState is Pause
	/// then waits for *seconds* amount of sec
	/// </summary>
	/// <param name="seconds"></param>
	/// <returns></returns>
	private IEnumerator PauseRoutine(float seconds){
        while (GameManager.Instance.CurrentState == GameState.Pause) {
            yield return new WaitForFixedUpdate(); 
        }
        yield return new WaitForSeconds(seconds);
	}

	private void pauseAllEnemiesAnimations(){
		foreach (Enemy enemy in enemiesOnScreen)
		{
			enemy.Anim.enabled = false;
		}
	}

	private void continueAllEnemiesAnimations(){
		foreach (Enemy enemy in enemiesOnScreen)
		{
			enemy.Anim.enabled = true;
		}
	}

	#endregion

#region Utility methods
	

	private void showPanelWithTag(String panelsTag){
		foreach (GameObject panel in UIPanels)
		{
				panel.SetActive(panel.tag == panelsTag);
		}

	}

	private void reevaluateTextLabel(){
		healthAmount.text = health.ToString();
	}

	private void SetGlobalValuesToInitialState(){
		enemiesKilled = 0;
		health = GameConstants.InitialHealth;
		reevaluateTextLabel();
	}

	#endregion

}
