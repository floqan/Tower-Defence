using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerList : MonoBehaviour
{

    #region Singelton
    public static TowerList instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance on towerList found!");
            return;
        }

        instance = this;
    }
    #endregion

    public List<Building> towes = new List<Building>();

    public int getLength()
    {
        return towes.Count;
    }

    public Building GetBuilding(int i)
    {
        return towes[i];
    }

    public void AddBuilding(Building building)
    {
        towes.Add(building);
    }
}
