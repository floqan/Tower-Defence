using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public Tower tower;
    Transform head;
    Transform target;
    Transform bulletSpawn;
    List<GameObject> enemyInRange;
    float timeTilNextAttack;
    public enum State { placing, ready, shooting, moving }
    public State state;


    private void Awake()
    {
        state = State.placing;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<SphereCollider>().radius = tower.attackRadius;
        head = transform.Find("Head");
        bulletSpawn = head.transform.Find("BulletSpawn");
        enemyInRange = new List<GameObject>();
        InvokeRepeating("findTarget", 0, 0.5f);
        timeTilNextAttack = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeTilNextAttack > 0)
        {
            timeTilNextAttack -= Time.deltaTime;
        }
        if (state != State.placing) { 
            if (target != null)
            {
                Vector3 dir = target.position - head.position;
                Quaternion look = Quaternion.LookRotation(dir);
                Vector3 rotation = Quaternion.Lerp(head.transform.rotation, look, Time.deltaTime * tower.rotationTime).eulerAngles;
                head.rotation = Quaternion.Euler(0, rotation.y, 0);
                float diff = look.eulerAngles.y - head.rotation.eulerAngles.y;
                if (diff < 3 && diff > -3)
                {
                    state = State.shooting;
                }
                else
                {
                    state = State.moving;
                }


                if(timeTilNextAttack <= 0 && state == State.shooting)
                {
                    timeTilNextAttack = 1f / tower.attackSpeed;
                    attack();
                }
            }
            else
            {
                state = State.ready;
            }
        }
    }

    private void attack()
    {
        switch (tower.Mesh.gameObject.tag)
        {
            case "Tower - Normal": attackNormal();
                break;
            case "3":
             break;
        }
    }

    private void attackNormal()
    {
        GameObject bulletGO = Instantiate(tower.bullet, bulletSpawn.position, bulletSpawn.rotation);

        if (bulletGO != null)
        {
            bulletGO.GetComponent<BulletBehaviour>().setTarget(target);
        }
    }

    private void findTarget()
    {
        if(enemyInRange.Count > 0)
        {
            float shortestDistance = Mathf.Infinity;
            for(int i = enemyInRange.Count - 1; i >= 0; i--)
            {
                
                if (enemyInRange[i] != null) {
                    GameObject enemy = enemyInRange[i];
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        target = enemy.transform;
                    }
                }
                else
                {
                    enemyInRange.RemoveAt(i);
                }
            }

            if(shortestDistance > tower.attackRadius)
            {
                target = null;
            }
        }
        else
        {
            target = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

      if(other.gameObject.tag == "Enemy")
        {
            enemyInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            enemyInRange.Remove(other.gameObject);
        }
    }

}
