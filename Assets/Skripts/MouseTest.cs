using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTest : MonoBehaviour
{

    Renderer rend;
    Color originColor;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        originColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        rend.material.color = Color.blue;
        ChangeModel chan = GameObject.FindGameObjectWithTag("Kristall").GetComponent<ChangeModel>();
        chan.ChangeKristallModel();
        
    }

    private void OnMouseOver()
    {
        rend.material.color += new Color(0.1f,0f,0f) * Time.deltaTime; 
    }

    private void OnMouseExit()
    {
        rend.material.color = originColor;
    }
}
