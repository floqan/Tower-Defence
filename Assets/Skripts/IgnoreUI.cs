using UnityEngine;

public class IgnoreUI : MonoBehaviour
{

    PlayerController controller;


    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Over UI");
        controller.MouseOverUI = true;
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse No Longer Over UI");
        controller.MouseOverUI = false;
    }
}
