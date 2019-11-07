using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singelton 
    public static GameManager instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Achtung, es wurden mehrere GameManager erstellt");
        }
        instance = this;
    }

    #endregion

    public GroundGrid grid;
    public PlayerController player;
    public BottomUI ui;
    private Inventory inventory;

    // ---------- PARAMETER ----------
    private int currentGold;
    public int maxGold;
    private float currentHealth;
    private int currentWave;
    private bool gridUpdated;

    // ---------- Enemies ----------
    public List<GameObject> passiveEnemies = new List<GameObject>();
    public Queue<Enemy> enemies = new Queue<Enemy>();
    public List<GameObject> enemySpawns = new List<GameObject>();

    internal void refreshGrid()
    {
        if (!gridUpdated)
        {
            grid.updateGrid();
            gridUpdated = true;
        }
    }

    public float spawnTime;
    private float time = 0;

    public int getCurrentGold()
    {
        return currentGold;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public int getCurrentWave()
    {
        return currentWave;
    }

    public void setMaxGold(int maxGold)
    {
        this.maxGold = maxGold;
    }

    private void Start()
    {
        inventory = Inventory.instance;
        RefillHealth();
        ui.UpdateStatsUI();
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= spawnTime)
        {
            createNewEnemy();
        }
    }

    private void LateUpdate()
    {
        gridUpdated = false;
    }

    private void createNewEnemy()
    {
        time = 0;
        if (enemies.Count > 0) {
            Debug.Log("Spawning Enemy");
            Enemy enemy = enemies.Dequeue();
            enemy.Mesh = Instantiate(enemy.Mesh, enemySpawns[0].transform.position + new Vector3(0,0,20), Quaternion.identity);
            enemy.Mesh.GetComponent<EnemyBehaviour>().enemy = enemy;
        }
    }

    public virtual void InstantiateBuilding(Building building, int buildingNumber)
    {
        if (canAffordBuilding(building)) {
            player.InstantiateSelectedBuilding(building, buildingNumber);
        }
        else
        {
            Debug.Log("Nicht genug Geld zum Kauf von " + building.Name);
        }
        
    }

    public void setBuildingFinal(Building building)
    {
        
        DecreaseCost(building);
        if (!(building is Plant))
        {
            grid.setBuildingOnGrid(building);
            grid.updateGrid();
        }
        ui.UpdateStatsUI();
    }

    public bool DecreaseCost(Building building)
    {
        if (!canAffordBuilding(building))
        {
            return false;
        }
        else
        {
            currentGold -= building.GoldKosten;
            foreach(KeyValuePair<int,int> kosten in building.AddKosten)
            {
                inventory.removeItemsFromInventory(kosten.Key, kosten.Value);
            }
            ui.UpdateStatsUI();
            return true;
        }
    }

    public bool canAffordBuilding(Building building )
    {
        if (building.GoldKosten > currentGold) return false;
        foreach (KeyValuePair<int, int> kosten in building.AddKosten)
        {
            if (kosten.Value > inventory.getItemCount(kosten.Key))
            {
            Debug.Log("Nicht genug " + Statics.getItemName(kosten.Key) + "zum kaufen");
                return false;
            }
        }
        return true;
    }

    public void IncreaseCurrentGold(int gold)
    {
        this.currentGold += gold;
        if(getCurrentGold() > maxGold)
        {
            currentGold = maxGold;
        }
    }

    public bool DecreaseCurrentHealth(float currentHealth)
    {
        if (this.currentHealth - currentHealth <= 0)
        {
            this.currentHealth = 0;
            ui.UpdateStatsUI();
            return false;
        }
        else
        {
            this.currentHealth -= currentHealth;
            ui.UpdateStatsUI();
            return false;
        }
    }

    public void IncreaseCurrentHealth(float health)
    {
        this.currentHealth += health;
        if(this.currentHealth > 1)
        {
            this.currentHealth = 1;
        }
    }

    public void RefillHealth()
    {
        currentHealth = 1.0f;
        Update();
    }

    public void IncreaseCurrentWave()
    {
        currentWave++;
        ui.UpdateStatsUI();
    }

}
