using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : StraightFlyingProjectile {

	
	
	// // Update is called once per frame
	// override protected void Update () {
	// 	base.Update();
	// }

	override protected void OnHit()
	{
		target.sufferDamage(attackDamage, false);
		Destroy(this.gameObject);

	}

	override public void PlayProjectileShotSound()
	{
		SoundManager.Instance.PlayArrowSound();
	}
}
