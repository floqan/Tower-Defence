using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData : MonoBehaviour
{

    #region Singelton 
    public static TerrainData instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Achtung, es wurden mehrere TerrainData erstellt");
        }
        instance = this;
    }

    #endregion

    private Terrain terrain;

    private void Start()
    {
        terrain = gameObject.GetComponent<Terrain>();
        if (terrain)
        {
            Debug.Log("TerrainData created successfully");
        }
    }

    public float getTerrainHeight(float x, float y)
    {
        return getTerrainHeight(new Vector3(x, 0, y));
    }

    public float getTerrainHeight(Vector3 pos)
    {
        return terrain.SampleHeight(pos);
    }

    public float getTerrainHeight(int x, int y)
    {
        GroundGrid grid = FindObjectOfType<GroundGrid>();
        if (grid)
        {
            return getTerrainHeight(grid.getNearestGridPoint(new KeyValuePair<int, int>(x, y)));
        }
        else
        {
            Debug.LogError("Grid not found");
            return 0;
        }
    }
}
