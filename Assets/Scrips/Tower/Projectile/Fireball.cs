using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : StraightFlyingProjectile {

	[SerializeField] private int burnDamage;
	[SerializeField] private int numberOfBurnStrikes;
	[SerializeField] private float pauseBetweenBurns;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	protected override void OnHit()
	{
		target.sufferDamage(attackDamage, true);
		if(!target.IsOnFire)
		{
			target.sufferPeriodicalDamage(numberOfBurnStrikes, burnDamage, pauseBetweenBurns, true);
		} 
		else 
		{
			target.refreshBurningDebuff(numberOfBurnStrikes, burnDamage, pauseBetweenBurns, true);
		}

		Destroy(gameObject);
	}

	public override  void PlayProjectileShotSound()
	{
		SoundManager.Instance.PlayFireballSound();
	}

}
