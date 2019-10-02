using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float focusHeight = 10f;
    public float focusDistance = 20f;

    public float minHeight = 20f;
    public float maxHeigth = 120f;

    public float keyPanSpeed = 50f;
    public float dragPanSpeed = 100f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    public float zoomSpeed = 100f;
    public float zoomTime = 0.1f;
    public float zoomVelocity = 0f;

    public float rotationSpeed = 30f;

    public float targetHeight = 60f;
    public float ratio;
    private float intervall;

    Vector3 focusPosition;

    void Start()
    {
        intervall = (maxHeigth - minHeight) / 3;
        focusPosition = new Vector3(transform.position.x, focusHeight, transform.position.z + focusDistance);
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

        Vector3 versatz = Vector3.zero;
        /*
        targetHeight += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * -1f;
        targetHeight = Mathf.Clamp(targetHeight, minHeight, maxHeigth);
        if (Input.GetAxis("Mouse ScrollWheel") != 0 || zoomVelocity > 0.01f || zoomVelocity < -0.01f)
        {
            versatz.y = Mathf.SmoothDamp(versatz.y, targetHeight, ref zoomVelocity, zoomTime);
        }*/
        float distance = targetHeight - minHeight;
        ratio = (int)(distance / intervall) + 1;

        //Move forward
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            versatz.z += keyPanSpeed * Time.deltaTime * ratio;
        }
        //Move backward
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            versatz.z -= keyPanSpeed * Time.deltaTime * ratio;
        }
        //Move left
        if(Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            versatz.x -= keyPanSpeed * Time.deltaTime * ratio;
        }
        //Move right
        if(Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            versatz.x += keyPanSpeed * Time.deltaTime * ratio;
        }

        
        if(Input.GetMouseButton(2))
        {
            versatz -= new Vector3(Input.GetAxis("Mouse X") * dragPanSpeed * Time.deltaTime * ratio, 0f, Input.GetAxis("Mouse Y")) * dragPanSpeed * Time.deltaTime * ratio;
        }

        float rotY = transform.rotation.y;
        versatz = Quaternion.AngleAxis(rotY, Vector3.up) * versatz;
        transform.position += versatz;
        focusPosition += new Vector3(versatz.x, 0f, versatz.z);
        
            if (Input.GetAxis("Rotation") != 0)
            {
                rotY = Input.GetAxis("Rotation") * Time.deltaTime * rotationSpeed;
                Debug.Log("torY : " + rotY);
                focusPosition += Quaternion.AngleAxis(rotY, Vector3.up) * (focusPosition - transform.position);
            }



        //        transform.position.x = Mathf.Clamp(transform.position.x, -panLimit.x, panLimit.x);
        //      transform.position.z = Mathf.Clamp(transform.position.z, -panLimit.y, panLimit.y);
        /*
        Vector3 diff = new Vector3(versatz.x - transform.position.x,0, versatz.z - transform.position.z);
        focusPosition += diff;
        */


        /*transform.position = versatz;
        //transform.RotateAround(focusPosition, Vector3.up, Time.deltaTime * 20);
        */

        try
        {
            transform.LookAt(focusPosition);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
