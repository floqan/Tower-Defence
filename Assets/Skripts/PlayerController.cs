using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private bool selecting;
    GroundGrid grid;
    private Building currentSelected;

    public bool MouseOverUI;
    
    // ---------- CAMERA ----------
    //Define Border
    public bool border;
    public int borderX = 50;
    public int borderZ = 50;
    public int panBorder = 10;

    public float rotationSpeed = 2000f;

    public float moveSpeed = 30f;
    public float dragSpeed = 50f;

    public float scrollSpeed = 30f;
    public float scrollVelocity= 0f;
    public float scrollTime = 0.5f;

    public float targetHeight = 60f;
    public float minHeight = 30f;
    public float minScrollHeight = 10f;
    public float maxHeight = 150f;
    private float intervall;

    public float factor;

    public float startX = 0f;
    public float startZ = 0f;

    private bool draging = false;

    public void InstantiateSelectedBuilding(Building building, int buildingNumber)
    {
        if (!selecting)
        {
           
            if(building is Tower)
            {
                currentSelected = new Tower(building as Tower);
                currentSelected.Mesh = Instantiate(building.Mesh);
                currentSelected.Mesh.GetComponent<TowerBehaviour>().tower = (Tower)TowerList.instance.towes[buildingNumber];
                grid.showGrid = true;
            }

            if (building is Plant)
            {
                currentSelected = new Plant(building as Plant);
                currentSelected.Mesh = Instantiate(((Plant)building).Meshes[0]);
            }

            if (building is Field)
            {
                currentSelected = new Field(building as Field);
                currentSelected.Mesh = Instantiate(building.Mesh);
                grid.showGrid = true;
            }

            currentSelected.Mesh.name = building.Name;
            selecting = true;
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(Vector3.up, 0f, Space.World);
        transform.position = new Vector3(startX, targetHeight, startZ);
        intervall = (maxHeight - minHeight) / 3;
        grid = FindObjectOfType<GroundGrid>();
        selecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }

        Vector3 newPos = Vector3.zero;
        float ratio = (int)((transform.position.y - minHeight) / intervall) + 1;
        //drag with mouse
        if (Input.GetMouseButton(2))
        {
            draging = true;
            newPos -= new Vector3(Input.GetAxis("Mouse X") * Time.deltaTime * ratio * dragSpeed,
                0f,
                Input.GetAxis("Mouse Y") * Time.deltaTime * dragSpeed * ratio);
        }
        else
        {
            draging = false;
        }
        if (!draging)
        {
            //move forward
            if (Input.GetKey("w") || (border ? Input.mousePosition.y > Screen.height - panBorder : false) || Input.GetKey("up"))
            {
                newPos.z += moveSpeed * Time.deltaTime * ratio;
            }
            //move backward
            if (Input.GetKey("s") || (border ? Input.mousePosition.y < panBorder : false) || Input.GetKey("down"))
            {
                newPos.z -= moveSpeed * Time.deltaTime * ratio;
            }
            //move left
            if (Input.GetKey("a") || (border ? Input.mousePosition.x < panBorder : false )|| Input.GetKey("left"))
            {
                newPos.x -= moveSpeed * Time.deltaTime * ratio;
            }
            //move right
            if (Input.GetKey("d") || (border ? Input.mousePosition.x > Screen.width - panBorder : false) || Input.GetKey("right"))
            {
                newPos.x += moveSpeed * Time.deltaTime * ratio;
            }
        }
        
        //Rotate offset according to actual rotation
        float rotY = transform.rotation.eulerAngles.y;
        newPos = Quaternion.AngleAxis(rotY, Vector3.up) * newPos;

        //Apply offset
        newPos += transform.position;
        newPos.x = Mathf.Clamp(newPos.x, 0, borderX);
        newPos.z = Mathf.Clamp(newPos.z, 0, borderZ);

        
        //Rotate
        if (Input.GetAxis("Rotation") != 0) {
            rotY += Input.GetAxis("Rotation") * Time.deltaTime * rotationSpeed;
        }

        //Zoom
        targetHeight += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * -1f;
        if (targetHeight < minScrollHeight) targetHeight = minScrollHeight;
        targetHeight = Mathf.Clamp(targetHeight, minHeight, maxHeight);
        if (scrollVelocity > 0.001f || scrollVelocity < -0.001f || Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            newPos.y = Mathf.SmoothDamp(newPos.y, targetHeight, ref scrollVelocity, scrollTime);
        }
        float rotX = transform.rotation.eulerAngles.x;
        factor = (newPos.y - minHeight) / (maxHeight - minHeight);
        rotX = factor * 60;
        //Set final rotation and location
        transform.SetPositionAndRotation(newPos, Quaternion.Euler(rotX, rotY, 0));

        if (Input.GetKey("p"))
        {
            Debug.Log("DebugPoint");
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selecting)
            {
                Destroy(currentSelected.Mesh);
                cancelSelection();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsMouseOverUI())
            {
                //Während der Gebäudeplatzierung
                if (selecting)
                {
                    KeyValuePair<int, int> pos = grid.getGridKoordinates(currentSelected.Mesh.transform.position);
                    if (currentSelected is Plant)
                    {
                        if(grid.gridSlots[pos.Key, pos.Value].Gebäude is Field && (grid.gridSlots[pos.Key, pos.Value].Gebäude.Mesh.GetComponent<PlantBehaviour>().isFree()))
                        {
                            grid.gridSlots[pos.Key, pos.Value].Gebäude.Mesh.GetComponent<PlantBehaviour>().plantPlant((Plant)currentSelected);
                            GameManager.instance.setBuildingFinal(currentSelected);
                            cancelSelection();
                        }
                    }
                    else
                    {
                        if (grid.gridSlots[pos.Key, pos.Value].isBauenErlaubt())
                        {
                            if (currentSelected is Tower) currentSelected.Mesh.GetComponent<TowerBehaviour>().state = TowerBehaviour.State.ready;
                            GameManager.instance.setBuildingFinal(currentSelected);
                            cancelSelection();
                        }
                    }
                }
                //Normaler Klick
                else
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit))
                    {
                        switch (hit.transform.root.tag)
                        {
                            case ("Field"):hit.transform.root.GetComponent<PlantBehaviour>().tryHarvest();
                                break;
                            case ("Merchant"):
                                break;
                        }
                    }
                }
            }
        }

        //Positionierung des ausgewählten Gebäudes
        if (selecting && !IsMouseOverUI())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 70.0f, LayerMask.GetMask("Grid")))
            {
                Vector3 offset = Vector3.zero;
                if(currentSelected is Plant)
                {
                    KeyValuePair<int, int> gridPos = grid.getGridKoordinates(hit.point);
                    if (!grid.gridSlots[gridPos.Key, gridPos.Value].isField())
                    {
                        currentSelected.Mesh.gameObject.SetActive(false);
                    }
                    else
                    {
                        offset = new Vector3(0, grid.gridSlots[gridPos.Key, gridPos.Value].Gebäude.Mesh.GetComponent<PlantBehaviour>().getHeight(), 0);
                        currentSelected.Mesh.gameObject.SetActive(true);
                    }
                }

                Vector3 pos = grid.getNearestGridPoint(hit.point) + offset;
                grid.setSelected(pos);
                currentSelected.Mesh.transform.position = pos;
            }
        }
    }

    private void cancelSelection()
    {
        selecting = false;
        grid.showGrid = false;
        currentSelected = null;
    }
    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}
