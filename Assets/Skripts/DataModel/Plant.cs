using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Building
{

    public GameObject[] Meshes;
    public int plantNumber;
    public float growTime = 0;
    public int crop;

    public Plant()
    {

    }

    public Plant(Plant plant) : base(plant)
    {
        Meshes = plant.Meshes;
        growTime = plant.growTime;
        crop = plant.crop;
        plantNumber = plant.plantNumber;
    }
}
