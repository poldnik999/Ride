using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubSceneChange : MonoBehaviour
{
    private bool isOpen;
    public GameObject ToolTip;
    private bool isActive = false;
    void Start()
    {
        ToolTip.SetActive(isActive);
        //child_cam = gameObject.transform.GetChild(0).GetComponent<Camera>();
    }
    public void OpenScene()
    {
        isOpen = true;
        ToolTip.SetActive(true);
    }
    public void Close()
    {
        isOpen = false;
        ToolTip.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

        if (isOpen)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {

                isActive = true;
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}
