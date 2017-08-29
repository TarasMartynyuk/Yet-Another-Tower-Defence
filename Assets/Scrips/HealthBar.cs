using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	[SerializeField] private Image health;
	[SerializeField] private Enemy enemy;

	private float maxhealth;
	
	
	void Awake()
	{
		maxhealth = enemy.Health;
		health.fillAmount = 1;
	}

	void Start()
	{
		
	}

	public void ReevaluateBar()
	{
		health.fillAmount = enemy.Health / maxhealth;
	}

	
	
}
