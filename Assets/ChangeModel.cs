using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeModel : MonoBehaviour
{
    private int state = 0;

    public GameObject[] models;
    
    private GameObject currentModel;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        ChangeKristallModel();
    }

    public void ChangeKristallModel()
    {
        if (state < models.Length)
        {
            Destroy(currentModel);
            currentModel = Instantiate(models[state], transform.position, transform.rotation);
            currentModel.transform.parent = transform;
            currentModel.transform.localScale = new Vector3(10, 10, 10);
            state++;
        }
    }
}
