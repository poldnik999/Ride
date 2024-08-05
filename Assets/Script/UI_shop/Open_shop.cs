using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_shop : MonoBehaviour
{
    private Camera child_cam;
    public bool isActive = false;
    private int depth_cam;
    // Start is called before the first frame update
    void Start()
    {
        child_cam = gameObject.transform.GetChild(0).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        

        if (Input.GetMouseButton(0))
        {
            isActive = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = false;
            child_cam.depth = 1;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!isActive) return;
    //    else
    //    {
    //        child_cam.depth = 5;
    //        Debug.Log("Shop is open!");
    //        isActive = false;
    //    }

    //}
}
