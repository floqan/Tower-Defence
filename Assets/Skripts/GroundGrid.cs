using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GroundGrid : MonoBehaviour
{

    public float size;
    public int dimensionX;
    public int dimensionZ;
    public Vector3 offset;
    private Vector3 selected;
    public bool showGrid = false;


    public GridPlatz[,] gridSlots;
    public List<KeyValuePair<int, int>> targets;
    public int[,] steps;

    public void initGrid()
    {
        //init variables
        steps = new int[dimensionX, dimensionZ];
        gridSlots = new GridPlatz[dimensionX, dimensionZ];
        targets = new List<KeyValuePair<int, int>>();
        for (int i = 0; i < dimensionX; i++)
        {
            for (int x = 0; x < dimensionZ; x++)
            {
                gridSlots[i, x] = new GridPlatz();
            }
        }

        //Setup for Environment
        for (int x = 0; x < dimensionX; x++)
        {
            for (int z = 0; z < dimensionZ; z++)
            {
                Vector3 pos = getNearestGridPoint(new KeyValuePair<int, int>(x, z));
                RaycastHit hit;
                if (Physics.Raycast(pos, Vector3.up, out hit, size, LayerMask.GetMask("Environment")))
                {
                    if(hit.transform.tag == "Target")
                    {
                        targets.Add(new KeyValuePair<int, int>(x,z));
                    }
                    gridSlots[x, z].besetzt = true;
                }
            }
        }

        updateGrid();
    }
    
    public virtual void updateGrid()
    {
        steps = new int[dimensionX, dimensionZ];
            List<KeyValuePair<int, int>> queue1 = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, int>> queue2 = new List<KeyValuePair<int, int>>();
            bool take1 = true;
            bool[,] explored = new bool[dimensionX,dimensionZ];
        foreach(KeyValuePair<int, int> pair in targets) {
            steps[pair.Key, pair.Value] = int.MaxValue;
            explored[pair.Key, pair.Value] = true;
            queue1.Add(pair);
            
        }    
            int counter = 1;
            KeyValuePair<int, int> tmp;
            while (queue1.Count > 0 || queue2.Count > 0)
            {
                if (take1) {
                    while (queue1.Count > 0)
                    {
                        KeyValuePair<int, int> node = queue1[0];
                        queue1.RemoveAt(0);
                        explored[node.Key, node.Value] = true;
                        if (node.Key > 0)
                        {
                            tmp = new KeyValuePair<int, int>(node.Key - 1, node.Value);
                            updateStep(tmp, queue2, counter, explored);
                        }
                        if (node.Key < dimensionX - 1)
                        {
                            tmp = new KeyValuePair<int, int>(node.Key + 1, node.Value);
                            updateStep(tmp, queue2, counter, explored);
                        }
                        if (node.Value > 0)
                        {
                            tmp = new KeyValuePair<int, int>(node.Key, node.Value - 1);
                            updateStep(tmp, queue2, counter, explored);
                        }
                        if (node.Value < dimensionZ - 1)
                        {
                            tmp = new KeyValuePair<int, int>(node.Key, node.Value + 1);
                            updateStep(tmp, queue2, counter, explored);
                        }
                    }
                }
                else
                {
                    while (queue2.Count > 0)
                    {
                        KeyValuePair<int, int> node = queue2[0];
                        queue2.RemoveAt(0);
                        explored[node.Key, node.Value] = true;
                        if (node.Key > 0)
                        {
                            tmp = new KeyValuePair<int, int>(node.Key - 1, node.Value);
                            updateStep(tmp, queue1, counter, explored);
                        }
                        if (node.Key < dimensionX - 1)
                        {
                            tmp = new KeyValuePair<int, int>(node.Key + 1, node.Value);
                            updateStep(tmp, queue1, counter, explored);
                        }
                        if (node.Value > 0)
                        {
                            tmp = new KeyValuePair<int, int>(node.Key, node.Value - 1);
                            updateStep(tmp, queue1, counter, explored);
                        }
                        if (node.Value < dimensionZ - 1)
                        {
                            tmp = new KeyValuePair<int, int>(node.Key, node.Value + 1);
                            updateStep(tmp, queue1, counter, explored);
                        }
                    }
                }

                counter++;
                take1 = !take1;
            }
    }

    private void updateStep(KeyValuePair<int,int> tmp, List<KeyValuePair<int,int>> queue, int counter, bool[,] explored)
    {
        if (!explored[tmp.Key, tmp.Value])
        {
            if (!gridSlots[tmp.Key, tmp.Value].besetzt)
            {
                if (steps[tmp.Key, tmp.Value] > counter || steps[tmp.Key, tmp.Value] == 0)
                {
                    steps[tmp.Key, tmp.Value] = counter;
                }
                if (!queue.Contains(tmp))
                {
                    queue.Add(tmp);
                }
            }
            else
            {
                steps[tmp.Key, tmp.Value] = int.MaxValue;
                explored[tmp.Key, tmp.Value] = true;
            }
        }
    }

    public void setBuildingOnGrid(Building building)
    {
        int x = (int) ((building.Mesh.transform.position.x - offset.x) / size);
        int y = (int) ((building.Mesh.transform.position.z - offset.z) / size);

        gridSlots[x, y].besetzt = true;
        gridSlots[x, y].Gebäude = building;
    }

    public void removeBuildingOnGrid(int x, int y)
    {
        gridSlots[x, y].besetzt = false;
        gridSlots[x, y].Gebäude = null;
    }

    public void setSelected(Vector3 selected)
    {
        this.selected = selected;
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.color = new Color(144f/256f, 238f / 256f, 144f / 256f, 0.5f);
            
        offset = this.transform.position;

            for (int i = 0; i <= dimensionX; i++)
            {
                Gizmos.DrawLine(offset + new Vector3(size * i, 0, 0), offset + new Vector3(size * i, 0, size * dimensionZ));
            }

            for (int i = 0; i <= dimensionZ; i++)
            {
                Gizmos.DrawLine(offset + new Vector3(0, 0, size * i), offset + new Vector3(size * dimensionX, 0, size * i));
            }
            /*
            for(int x = 0; x < dimensionX; x++)
            {
                for (int y = 0; y < dimensionZ; y++)
                {
                    if (gridSlots[x, y].besetzt)
                    {
                        Vector3[] verts = new Vector3[]
                        {
                            new Vector3(x * size + offset.x , 0, y * size + offset.y + size),
                            new Vector3(x * size + offset.x , 0, y * size + offset.y),
                            new Vector3(x * size + offset.x + size, 0, y * size + offset.y),
                            new Vector3(x * size + offset.x + size, 0, y * size + offset.y + size)
                        };
                        Handles.DrawSolidRectangleWithOutline(verts, new Color(0.8f, 0, 0), new Color(0.8f, 0, 0));
                    }
                    if (!gridSlots[x, y].isBauenErlaubt())
                    {
                        Vector3[] verts = new Vector3[]
                        {
                            new Vector3(x * size + offset.x , 0, y * size + offset.y + size),
                            new Vector3(x * size + offset.x , 0, y * size + offset.y),
                            new Vector3(x * size + offset.x + size, 0, y * size + offset.y),
                            new Vector3(x * size + offset.x + size, 0, y * size + offset.y + size)
                        };
                        Handles.DrawSolidRectangleWithOutline(verts, new Color(0, 0, 0.8f), new Color(0, 0, 0.8f));
                    }

                }
             }*/
                
        if(showGrid)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(selected - new Vector3(0.5f * size,0,-0.5f * size - 3*size), selected - new Vector3(0.5f * size, 0, 0.5f * size + 3 * size));
            Gizmos.DrawLine(selected - new Vector3(0.5f * size + size, 0, -0.5f * size - 3 * size), selected - new Vector3(0.5f * size + size, 0, 0.5f * size + 3 * size));


            Gizmos.DrawLine(selected + new Vector3(0.5f * size, 0, -0.5f * size - 3 * size), selected + new Vector3(0.5f * size, 0, 0.5f * size + 3 * size));
            Gizmos.DrawLine(selected + new Vector3(0.5f * size + size, 0, -0.5f * size - 3 * size), selected + new Vector3(0.5f * size + size, 0, 0.5f * size + 3 * size));
        }
    }

    internal Vector3 getNextGoal(Enemy enemy)
    {
        return Vector3.zero;
        //throw new NotImplementedException();
    }

    public Vector3 getNearestGridPoint(Vector3 point)
    {
        Vector3 gridPoint;
        point -= offset;
        gridPoint.x = Mathf.Floor(point.x / size) * size + offset.x;
        gridPoint.y = offset.y;
        gridPoint.z = Mathf.Floor(point.z / size) * size + offset.z;
        
        return gridPoint + new Vector3(0.5f * size,0, 0.5f * size);
    }

    public KeyValuePair<int, int> getGridKoordinates(Vector3 pos)
    {
        pos -= offset;
            pos /=  size;
        return new KeyValuePair<int,int>((int)pos.x, (int)pos.z);
    }

    public Vector3 getNearestGridPoint(KeyValuePair<int, int> node)
    {
        Vector3 pos = new Vector3(node.Key * size + offset.x + 0.5f * size, 0, node.Value * size + offset.z + 0.5f * size);
        pos.y = TerrainData.instance.getTerrainHeight(pos);
        return pos;
    }

    void onClick()
    {
        Debug.Log("Clicked");
    }
}
