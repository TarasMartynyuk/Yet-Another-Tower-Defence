using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Projectile {
	
	private MortarTower shootingTower;
	private bool hitGround = false;

	public MortarTower ShootingTower{
		set{shootingTower = value;}
	}
	  void Start()
    {   
        this.GetComponent<Rigidbody2D>().velocity = BallisticVel(target.transform);
    }

    /// <summary>
    /// will invoke when hit groundTile only
    /// </summary>
    /// <param name="other"></param>
	void OnCollisionEnter2D(Collision2D other)
	{	
		OnHit();
	}
	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "enemy"){
			other.gameObject.GetComponent<Enemy>().sufferDamage(attackDamage, false);
		}
		
	}
	
	protected override void OnHit(){
		
		if(hitGround)
			return;
		StartCoroutine(destroyIfFlownOutOfRange());
		SoundManager.Instance.PlayRockSound();
		hitGround = true;
	}

	private Vector3 BallisticVel(Transform target) {

            Vector3 dir = target.position - transform.position; // get target direction
            float h = dir.y;  // get height difference
            dir.y = 0;  // retain only the horizontal direction
            float dist = dir.magnitude ;  // get horizontal distance
            dir.y = dist;  // set elevation to 45 degrees
            dist += h;  // correct for different heights
            float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude);
            return vel * dir.normalized;  // returns Vector3 velocity
    }

	private IEnumerator destroyIfFlownOutOfRange(){
		
        while(true){
            if(!shootingTower.isInRange(transform.position)){
                break;
            }
			yield return new WaitForFixedUpdate();
        }
        Destroy(this.gameObject);

    }

	override public void PlayProjectileShotSound()
	{
		SoundManager.Instance.PlayRockShotSound();
	}
	

}
