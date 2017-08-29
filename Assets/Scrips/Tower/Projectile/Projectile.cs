using UnityEngine;
using System;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected int attackDamage;
    protected Enemy target;

    public Enemy Target
	{
		get{return target;}
		set {target = value;}
	}


    /// <summary>
	/// Is called when the projectile hits the enemy
	/// </summary>
	protected abstract void OnHit();

	public abstract void PlayProjectileShotSound();

}