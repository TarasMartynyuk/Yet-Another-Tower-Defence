using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public abstract class Tower : MonoBehaviour {

	[SerializeField] private float  attackCooldown;
	[SerializeField] private int  towerCost;
	[SerializeField] private Sprite viewFromBackSprite;
	
	//fucking NULLABLE!
	protected Enemy targetEnemy;
	private float timeElapsedFromShot;
	protected BuildTile towersBuildSite;

#region  properties
	public int Cost{
		get{ return towerCost;}
	}

	public Sprite ViewFromBackSprite{
		get{return viewFromBackSprite;}
	}

	public BuildTile TowersBuildSite{
		get{return towersBuildSite;}
		set{towersBuildSite = value;}
	}

#endregion

#region Unity init methods
	/// <summary>
	/// 
	/// Awake is called when the script instance is being loaded.
	/// </summary> <summary>
	/// </summary>
	virtual protected void Awake()
	{
		Assert.IsNotNull(viewFromBackSprite);
	}
#endregion

	  // Update is called once per frame
	  protected  void Update () {

		switch(GameManager.Instance.CurrentState)
		{

			case GameState.Game:
				timeElapsedFromShot += Time.deltaTime;
				
				if (timeElapsedFromShot >= attackCooldown)
				{
					
					if(SearchForEnemyAndFire())
					{
						timeElapsedFromShot = 0;

					}
				
				}

				break;
		}
	}

	/// <summary>
	/// method called when tower is ready to fire
	/// searches for a target and attacks it
	/// returns true if target was found and tower fired
	/// and false otherwise
	/// </summary>
	/// <returns></returns>
	abstract protected  bool SearchForEnemyAndFire();
	
	public void DestroyTower(){
		towersBuildSite.Occupied = false;
		Destroy(this.gameObject);
	}






}
