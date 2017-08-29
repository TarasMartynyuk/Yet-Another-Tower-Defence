using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class MortarTower : Tower 
{

    [SerializeField] private Rock projectile;

    private bool isAttacking;
    //bounding rectangle of the buildsite this tower is placed on  
    private Bounds bounds;
    private float tileWidth;
    
    
#region Unity init methods
    protected override void Awake()
    {
        base.Awake();
        Assert.IsNotNull(projectile);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        bounds = towersBuildSite.gameObject.GetComponent<SpriteRenderer>().bounds;
        tileWidth = bounds.size.x;
    }
#endregion
    override protected  bool SearchForEnemyAndFire(){
        //is ready to fire
        //tracking enemies
        //if targetting enemy - check if he is still in range and alive
        if (targetEnemy != null)
        {
            if (!isInRange(targetEnemy.transform.position) || targetEnemy.IsDead)
            {
                //enemy left radius of attak or got killed
                targetEnemy = null;
            }
        }
        //if targeted enemy was out of range, or wasn't defined - find new target enemy
        if (targetEnemy == null)
        {
            targetEnemy = GetSecondNearestEnemyInRange();
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
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(isAttacking)
        {
           AttackTargetEnemy();
           isAttacking = false;
        }
    }

    private void AttackTargetEnemy()
    {
        Rock newProjectile = Instantiate(projectile) as Rock;
        newProjectile.transform.position = this.transform.position;
        newProjectile.Target = targetEnemy;
        newProjectile.PlayProjectileShotSound();
        newProjectile.ShootingTower = this;
    }

    private Enemy GetSecondNearestEnemyInRange(){

        Enemy currNearestEnemy = null;
        Enemy currSecondNearestEnemy = null;
        
        float currSmallestDistance = float.PositiveInfinity;
        
        float distanceToCurrEnemy;


        foreach (Enemy enemy in GameManager.Instance.EnemiesOnScreen)
        {
            //don't consider enemies out of range
            if(!isInRange(enemy.transform.position) || enemy.IsDead){
                continue;
            }

            distanceToCurrEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if(distanceToCurrEnemy < currSmallestDistance){
                
                //check if this is the first valid enemy found
                if(currNearestEnemy != null){
                    //if not - save the latest found enemy
                    currSecondNearestEnemy = currNearestEnemy;
                }

                currNearestEnemy = enemy;
                currSmallestDistance = distanceToCurrEnemy;
            }
        }

        return currSecondNearestEnemy == null? currNearestEnemy : currSecondNearestEnemy;
    }

    public bool isInRange(Vector3 position){
                //not too close in x distance
        return (position.x <= bounds.min.x - tileWidth) &&
            //not too far in x distance
            (position.x >= bounds.min.x - tileWidth * 6) &&
            //only shoot if target is in front - not lower or higher
            (position.y <= bounds.max.y) &&
            (position.y >= bounds.min.y);

    }

    



}