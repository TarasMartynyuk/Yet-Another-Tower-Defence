using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StraightFlyingProjectile : Projectile {

	
	[SerializeField] protected float velocity;
	
	//direction finding support
	private Vector3 directionToTarget;
	
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	virtual protected void Update()
	{	
		switch(GameManager.Instance.CurrentState){
			case GameState.Game:

				if(target == null || target.IsDead){
					Destroy(gameObject);
				}

				//recalculate direction to target and arrow pointing rotation 
				directionToTarget = target.transform.position - transform.position;
				float dirAngleToTarget= Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(dirAngleToTarget, Vector3.forward);

				//check if projectile will not reach target at the end of current frame
				float distanceThisFrame = velocity * Time.deltaTime;
				if(distanceThisFrame >= directionToTarget.magnitude){
					OnHit();
				}
				//move
				transform.Translate(directionToTarget.normalized * distanceThisFrame, Space.World);

				break;

			case GameState.EndGame:

				Destroy(gameObject);
				break;
		}
	}

	/// <summary>
	/// Is called when the projectile hits the enemy
	/// destroys projectile gameobject
	/// </summary>
	protected abstract override void OnHit();

}
