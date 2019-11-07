using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    Plant plant;
    float growingTime;
    Transform growingPlace;

    enum State {Ready, State0, State1, Grown}
    State state;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("grow", 2.0f, 0.5f);
        state = State.Ready;
        growingTime = 0;
        growingPlace = transform.Find("GrowingPlace");
    }

    public void plantPlant(Plant plant)
    {
        this.plant = plant;
        growingTime = plant.growTime;
        this.plant.Mesh.transform.SetParent(growingPlace);
        //this.plant.Mesh = Instantiate(plant.Meshes[0],growingPlace);
        state = State.State0;
    }

    private void nextGrowingStage()
    {
        switch (state)
        {
            case State.State0:
                state = State.State1;
                Destroy(plant.Mesh);
                plant.Mesh = Instantiate(plant.Meshes[1], growingPlace);
                break;
            case State.State1:
                state = State.Grown;
                Destroy(plant.Mesh);
                plant.Mesh = Instantiate(plant.Meshes[2], growingPlace);
                break;
        }
    }

    private void grow()
    {
        if(state != State.State0 && state != State.State1)
        {
            return;
        }

        if(growingTime > 0)
        {
            growingTime -= 0.5f;
        }

        float result = plant.growTime / 3;
        int stage = Mathf.FloorToInt(growingTime / result);
        if(state == State.State0 && stage == 1)
        {
            nextGrowingStage();
        }
        if(state == State.State1 && stage == 0)
        {
            nextGrowingStage();
        }
        if(growingTime <= 0)
        {
            nextGrowingStage();
        }
        
    }

    public void harvestPlant()
    {
        state = State.Ready;
        Inventory.instance.addItemsToInventory(plant.plantNumber, plant.crop);
        Destroy(plant.Mesh);
        plant = null;

    }

    public void tryHarvest()
    {
        if(state == State.Grown)
        {
            harvestPlant();
        }
    }

    internal float getHeight()
    {
        return growingPlace.position.y;
    }

    internal bool isFree()
    {
        return plant == null;
    }
}
