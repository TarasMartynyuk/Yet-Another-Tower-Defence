using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class LinearShootingTower : Tower
{

    [SerializeField] private float attackRadius;
    [SerializeField] private StraightFlyingProjectile projectile;

    //fucking NULLABLE!
    private bool isAttacking;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        Assert.IsNotNull(projectile);
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (isAttacking)
        {
            AttackTargetEnemy();
            isAttacking = false;
        }
    }

    override protected bool SearchForEnemyAndFire()
    {
        //is ready to fire
        //tracking enemies
        //if targetting enemy - check if he is still in range and alive
        if (targetEnemy != null)
        {
            if (Vector2.Distance(transform.position, targetEnemy.transform.position) >= attackRadius || targetEnemy.IsDead)
            {
                //enemy left radius of attak or got killed
                targetEnemy = null;
            }
        }
        //if targeted enemy was out of range, or wasn't defined - find new target enemy
        if (targetEnemy == null)
        {
            targetEnemy = FindTarget();
        }
        //firing at the enemy
        if (targetEnemy != null)
        {
            //shoot
            isAttacking = true;
            return true;
        }
        return false;
    }


    /// <summary>
    /// instantiates a projectile and sets it target
    /// </summary>
    private void AttackTargetEnemy()
    {
        Projectile newProjectile = Instantiate(projectile) as StraightFlyingProjectile;
        newProjectile.transform.position = this.transform.position;
        newProjectile.Target = targetEnemy;
        newProjectile.PlayProjectileShotSound();
        // GetComponent<AudioSource>().Play();
    }


    /// <summary>
    /// Returns Enemy object which furthest down the path and
    /// which is inside its firing range,
    /// or NULL if there aren't any in range
    /// </summary>
    /// <returns> Enemy object or null</returns>
    private Enemy FindTarget()
    {
        Enemy nearestEnemy = null;
        float currBiggestX = float.NegativeInfinity;

        foreach (Enemy enemy in GameManager.Instance.EnemiesOnScreen)
        {
            //dont consider dead enemies or enemies out of range
            if (Vector2.Distance(transform.position, enemy.transform.position) > attackRadius || enemy.IsDead)
            {
                continue;
            }
            if (enemy.transform.position.x > currBiggestX)
            {
                nearestEnemy = enemy;
                currBiggestX = enemy.transform.position.x;
            }

        }


        return nearestEnemy;
    }


}
