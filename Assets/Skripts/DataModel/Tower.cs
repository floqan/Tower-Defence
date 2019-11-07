using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    public float attackRadius;
    public float rotationTime;
    public float attackSpeed;
    public GameObject bullet;

    public Tower()
    {
    }

    public Tower(Tower tower) : base(tower)
    {
        attackRadius = tower.attackRadius;
        rotationTime = tower.rotationTime;
        attackSpeed = tower.attackSpeed;
        bullet = tower.bullet;
    }
        
}
