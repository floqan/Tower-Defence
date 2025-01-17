﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeModel : MonoBehaviour
{

    public GameObject[] models;
    
    private GameObject currentModel;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }

    public void ChangeTargetModel(int state)
    {
        if (state < models.Length)
        {
            Destroy(currentModel);
            currentModel = Instantiate(models[state], transform.position, Quaternion.Euler(-9, 9, 17.5f));
            currentModel.transform.parent = transform;
            currentModel.transform.localScale = new Vector3(10, 10, 10);
        }
    }
}
