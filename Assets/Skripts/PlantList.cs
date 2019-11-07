using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantList : MonoBehaviour
{
    #region Singelton
    public static PlantList instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance on PlantList found!");
            return;
        }

        instance = this;
    }
    #endregion

    public List<Building> plants = new List<Building>();

    public int getLength()
    {
        return plants.Count;
    }

    public Building GetBuilding(int i)
    {
        return plants[i];
    }

    public void AddBuilding(Building building)
    {
        plants.Add(building);
    }
}
