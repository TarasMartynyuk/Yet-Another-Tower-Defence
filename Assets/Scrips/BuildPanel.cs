using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class BuildPanel : MonoBehaviour 
{

	[SerializeField] private TowerButton[] towerButtons;
	
	// Use this for initialization
	void Start () 
	{
		gameObject.SetActive(false);
		
	}
	
	public void updateTowerButtonsStatus()
	{
		foreach (TowerButton butt in towerButtons)
		{
			butt.updateTowerButtonStatus();
		}

	}

}
