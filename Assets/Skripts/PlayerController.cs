using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool border;

    //Define Border
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
    public float maxHeight = 150f;
    private float intervall;

    public float factor;

    public float startX = 0f;
    public float startZ = 0f;

    private bool draging = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(Vector3.up, 90f, Space.World);
        transform.position = new Vector3(startX, targetHeight, startZ);
        intervall = (maxHeight - minHeight) / 3;
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
        newPos.x = Mathf.Clamp(newPos.x, -borderX, borderX);
        newPos.z = Mathf.Clamp(newPos.z, -borderZ, borderZ);

        
        //Rotate
        if (Input.GetAxis("Rotation") != 0) {
            rotY += Input.GetAxis("Rotation") * Time.deltaTime * rotationSpeed;
        }

        //Zoom
        targetHeight += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * -1f;
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
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
            }
        }
    }
}
