using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGrid : MonoBehaviour
{

    public float size;
    public int dimensionX;
    public int dimensionZ;
    public Vector3 start;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = start;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        
    }

    public Vector3 getNearestGridPoint(Vector3 point)
    {
        Vector3 gridPoint;
        gridPoint.x = Mathf.Floor(point.x / size) * size + start.x;
        gridPoint.y = Mathf.Floor(point.y / size) * size + start.y;
        gridPoint.z = Mathf.Floor(point.z / size) * size + start.z;
        
        return gridPoint;
    }

    void onClick()
    {
        Debug.Log("Clicked");
    }
}
