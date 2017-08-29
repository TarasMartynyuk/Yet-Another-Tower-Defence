using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class TowerButton : MonoBehaviour {

	//existing objects
	[SerializeField] private GameObject canAffordButton, canNotAffordButton;
	//prefab
	[SerializeField] private Tower tower;
	private int towerCost;
	private bool canAffordTower = false;
	
	public Tower Tower 
	{
		get{return tower;}
	}


	void Awake()
	{
		Assert.IsNotNull(canAffordButton);
		Assert.IsNotNull(canNotAffordButton);
		Assert.IsNotNull(tower);

		//initialize button's child object  references
		towerCost = tower.Cost;
		canAffordButton.GetComponentInChildren<Text>().text = towerCost.ToString();
	}

	public void updateTowerButtonStatus(){
		/*check wether the player building possibilities(in terms of gold) 
		have changed relative to this buttons saved data, and update if so*/
		if((TowerManager.Instance.GoldAmount >= towerCost) != canAffordTower){
			//state changed
			changeAffordnessState();
		}
	}

	/// <summary>
	/// toggle button "is able to build" state 
	/// </summary>
	private void changeAffordnessState(){
		if(canAffordTower){
			canAffordButton.SetActive(false);
			canNotAffordButton.SetActive(true);
			canAffordTower = false;

		} else {

			canNotAffordButton.SetActive(false);
			canAffordButton.SetActive(true);
			canAffordTower = true;			
		}
	}
}
