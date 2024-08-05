using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isOpen;
    public Camera child_cam;
    public GameObject ToolTip;
    public GameObject UICanvas;
    private bool isActive = false;
    private int depth_cam;
    void Start()
    {
        ToolTip.SetActive(isActive);
        UICanvas.SetActive(isActive);
        //child_cam = gameObject.transform.GetChild(0).GetComponent<Camera>();
    }
    public void Open()
    {
        isOpen = true;
        ToolTip.SetActive(true);
        UICanvas.SetActive(isActive);
    }
    public void Close()
    {
        isOpen = false;
        ToolTip.SetActive(false);
        UICanvas.SetActive(isActive);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (isOpen)
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                
                isActive = true;
                child_cam.depth = 5;
                Debug.Log("Shop is open!");
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isOpen = false;
                isActive = false;
                child_cam.depth = 1;
            }
        }
    }
}
