using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveButton : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent OnPressed;
    public UnityEvent OffPressed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        OnPressed.Invoke();
        //if (!isActive) return;
        //else
        //{
        //    child_cam.depth = 5;
        //    Debug.Log("Shop is open!");
        //    isActive = false;
        //}

    }
    private void OnTriggerExit(Collider other)
    {
        OffPressed.Invoke();
        //if (!isActive) return;
        //else
        //{
        //    child_cam.depth = 5;
        //    Debug.Log("Shop is open!");
        //    isActive = false;
        //}

    }
}
