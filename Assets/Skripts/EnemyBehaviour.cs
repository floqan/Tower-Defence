using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public Enemy enemy = new Enemy();
    GroundGrid grid;
    bool targetReached;
    public float attackTime;
    private float currentTime;

    private void Awake()
    {
        targetReached = false;

    }


    // Start is called before the first frame update
    void Start()
    {
        //enemy = GameManager.instance.enemies.Dequeue();
        targetReached = false;
        currentTime = attackTime;
        grid = FindObjectOfType<GroundGrid>();
        setNextGoal(true);
        enemy.CurrentHitpoints = enemy.MaxHitpoint;
        enemy.LastGoal = transform.position;
        KeyValuePair<int, int> pos = grid.getGridKoordinates(enemy.LastGoal);
        grid.gridSlots[pos.Key, pos.Value].enemyCounter++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetReached)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, enemy.NextGoal, enemy.Velocity * Time.deltaTime);
            transform.position = new Vector3(newPos.x, TerrainData.instance.getTerrainHeight(transform.position), newPos.z);

            if (isGoalReached())
            {
                if (isTargetReached())
                {
                    targetReached = true;
                    KeyValuePair<int, int> pos = grid.getGridKoordinates(enemy.LastGoal);
                    grid.gridSlots[pos.Key, pos.Value].enemyCounter--;
                }
                else {
                    setNextGoal(false);
                } }
        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackTime) {
                Debug.LogWarning("Attack-Animation not implemented yet.");
                currentTime = 0;
                attack();
            }
        }
    }

    public void decreaseHealth(int damage) 
    {
        enemy.CurrentHitpoints -= damage;
        if(enemy.CurrentHitpoints < 1)
        {
            die();
        }
    }

    private void die()
    {
        
        Destroy(gameObject);
    }

    void setNextGoal(bool init)
    {
        int left, up, right, down;
        left = right = up = down = int.MaxValue;
        KeyValuePair<int, int> node = grid.getGridKoordinates(transform.position);
        
        if (node.Key > 0)
        {
            left = grid.steps[node.Key - 1, node.Value];
        }
        if (node.Key < grid.dimensionX - 1)
        {
            right = grid.steps[node.Key + 1, node.Value];
        }
        if (node.Value > 0)
        {
            down = grid.steps[node.Key, node.Value - 1];
        }
        if (node.Value < grid.dimensionZ - 1)
        {
            up = grid.steps[node.Key, node.Value + 1];
        }
        int way = 0;
        //links kleiner als rechts
        if (left <= right)
        {
            //links kleiner als oben
            if(left <= up)
            {
                //links kleiner als unten
                if(left <= down)
                {
                    way = 1;
                }
                //unten kleiner als links
                else
                {
                    way = 3;
                }
            }
            //oben kleiner als links
            else
            {
                if(up <= down)
                {
                    way = 4;
                }
                else
                {
                    way = 3;
                }
            }
        }
        //rechts kleiner als links
        else
        {
            //rechts kleiner als oben
            if(right <= up)
            {
                //rechts kleiner als unten
                if(right <= down)
                {
                    way = 2;
                }
                //unten kleiner als rechts
                else
                {
                    way = 3;
                }
            }
            //oben kleiner als rechts
            else
            {
                if(down <= up)
                {
                    way = 3;
                }
                else
                {
                    way = 4;
                }
            }
        }
        KeyValuePair<int, int> unlock = grid.getGridKoordinates(enemy.LastGoal);
        if (!init)
        {
            grid.gridSlots[unlock.Key, unlock.Value].enemyCounter--;
        }
        enemy.LastGoal = enemy.NextGoal;
        Vector3 goal = Vector3.zero;
        switch (way)
        {
            case 1: 
                goal = grid.getNearestGridPoint(new KeyValuePair<int, int>(node.Key - 1, node.Value));
                break;
            case 2:
                goal = grid.getNearestGridPoint(new KeyValuePair<int, int>(node.Key + 1, node.Value));
                break;
            case 3:
                goal = grid.getNearestGridPoint(new KeyValuePair<int, int>(node.Key, node.Value - 1));
                break;
            case 4:
                goal = grid.getNearestGridPoint(new KeyValuePair<int, int>(node.Key, node.Value + 1));
                break;
        }
        enemy.NextGoal = new Vector3(goal.x, TerrainData.instance.getTerrainHeight(goal), goal.z);
        unlock = grid.getGridKoordinates(enemy.NextGoal);
        grid.gridSlots[unlock.Key, unlock.Value].enemyCounter++;
    }

    bool isTargetReached()
    {
        KeyValuePair<int,int> pos = grid.getGridKoordinates(transform.position);
        return grid.targets.Contains(new KeyValuePair<int, int>(pos.Key + 1, pos.Value)) ||
            grid.targets.Contains(new KeyValuePair<int, int>(pos.Key - 1, pos.Value)) ||
            grid.targets.Contains(new KeyValuePair<int, int>(pos.Key, pos.Value + 1)) ||
            grid.targets.Contains(new KeyValuePair<int, int>(pos.Key, pos.Value - 1));


    }

    bool isGoalReached()
    {
        return Vector3.Distance(transform.position, enemy.NextGoal) < 0.0001f;
    }

    private void attack()
    {
        GameManager.instance.DecreaseCurrentHealth(enemy.Damage);
    }
}
