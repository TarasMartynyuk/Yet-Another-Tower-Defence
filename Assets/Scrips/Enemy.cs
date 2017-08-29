using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField] private Transform[] waypoints;
	[SerializeField] private HealthBar healthBar;
	private float navigationUpdateInterval = 0.01f;
	[SerializeField] private float velocity = 0f;
	[SerializeField] private int health = 0;
	[SerializeField] private float armor = 0.5f;
	[SerializeField] private int goldFromKill;
	[SerializeField] private int exitReachedDamage;

	
	
	private int currWaypointIndex = 0;
	private Transform enemyLocation;
	private Animator anim;
	//time elapsed from last navigation update
	private float navigationUpdateElapsedTime;
	private bool isDead;
	private bool isOnFire;
	

#region Properties
		
	public bool IsOnFire{
		get{return isOnFire;}
	}
	public bool IsDead{
		get{return isDead;}
	}

	public int Health{
		get{return health;}
	}

	public float Armor{
		get{return armor;}
	}

	public Animator Anim{
		get{return anim;}
	}

	public int ExitReachedDamage
	{
		get {return exitReachedDamage;}
	}

#endregion

	// Use this for initialization
	void Start () {
		enemyLocation = GetComponent<Transform>();
		anim = GetComponent<Animator>();
	}

	void Update () {
		switch(GameManager.Instance.CurrentState) {

			case GameState.Game:

				if(isDead){
					return;
				}
				navigationUpdateElapsedTime += Time.deltaTime;
				if(navigationUpdateElapsedTime >= navigationUpdateInterval){
			
						enemyLocation.position = Vector2.MoveTowards(enemyLocation.position, 
						waypoints[currWaypointIndex].position, navigationUpdateElapsedTime * velocity);
						navigationUpdateElapsedTime = 0;
			
				}

				break;

				
		}
	}

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{	
		if(other.tag == "checkpoint")
		{

			currWaypointIndex++;

		} 
		else if(other.tag == "villageEntrance")
		{

			currWaypointIndex++;

		} 
		else if (other.tag == "Finish")
		{
			//hit the mapEnd checkpoint
			GameManager.Instance.EnemyEscaped(this);
		}
	}
	
	#region taking damage
		
	/// <summary>
	/// applies damage to this enemy, considering its armor property 
	/// if armorPierced parameter is false
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="armorPierced"></param>
	public void sufferDamage(int damage, bool armorPierced){

		health -= armorPierced? damage : (int) (damage * (1 - armor));
		
		if(health <= 0)
		{
			healthBar.gameObject.SetActive(false);
			StartCoroutine(die());
		} 
		else 
		{
			healthBar.ReevaluateBar();
			anim.Play("Hurt");
		}
	}
	
	public void sufferPeriodicalDamage(int strikes, int strikeDamage, float pauseBetweenStrikes, bool armorPierced)
	{
		StartCoroutine(periodicalDamage(strikes, strikeDamage, pauseBetweenStrikes, armorPierced));
	}

	public void refreshBurningDebuff(int strikes, int strikeDamage, float pauseBetweenStrikes, bool armorPierced)
	{
		StopCoroutine("periodicalDamage");
		sufferPeriodicalDamage(strikes, strikeDamage, pauseBetweenStrikes, armorPierced);
	}
	#endregion
	
	/// <summary>
	/// damages target @strikes times for @strikeDamage amount of damage,
	/// waiting @pauseBetweenStrikes seconds between each strike
	/// if @armorPierced is true each strike ignores armor
	/// </summary>
	/// <param name="strikes"></param>
	/// <param name="strikeDamage"></param>
	/// <param name="pauseBetweenStrikes"></param>
	/// <param name="armorPierced"></param>
	/// <returns></returns>
	private IEnumerator periodicalDamage(int strikes, int strikeDamage, float pauseBetweenStrikes, bool armorPierced){
		isOnFire = true;
		for (int i = 0; i < strikes; i++)
		{	
			if(isDead)
			{
				break;
			}
			sufferDamage(strikeDamage, armorPierced);
			yield return GameManager.Instance.Sync(pauseBetweenStrikes);
		}
		isOnFire = false;
		
	}
	
	private IEnumerator die(){
		
		isDead = true;
		TowerManager.Instance.GoldAmount += goldFromKill;
		anim.Play("Dying");
		SoundManager.Instance.PlayDeathSound();
		yield return GameManager.Instance.Sync(GameConstants.ActiveAfterDeath);

		GameManager.Instance.EnemyKilled(this);
		
	}
	
}
