using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Game : MonoBehaviour
{
    // Start is called before the first frame update
    private Open_shop sh;
    public List<GameObject> elementsUIToHide = new List<GameObject>();
    public List<GameObject> buttonsUIToHide = new List<GameObject>();
    public GameObject settingPanel;
    public TextMeshProUGUI levelValue;
    public Slider levelExpValue;
    private int pressedEsc;
    void Start()
    {
        settingPanel.SetActive(false);
        pressedEsc = 0;
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            foreach (GameObject element in buttonsUIToHide)
            {
                if(element.name == "MainMenuButton")
                    element.SetActive(false);
            }
        }
        if (SceneManager.GetActiveScene().name == "HubScene")
        {
            foreach (GameObject element in buttonsUIToHide)
            {
                if (element.name == "ReturnButton" || element.name == "HighScorePanel" || element.name == "PlayerStatusPanel")
                    element.SetActive(false);
            }
        }
        
        foreach (GameObject element in elementsUIToHide)
        {
            element.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        levelExpValue.value = LevelSystem.DataHolder.LevelExp / (float)LevelSystem.levelReq; 
        levelValue.text = LevelSystem.DataHolder.Level.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))// && sh.isActive == false)
        {
            settingPanel.SetActive(false);
            pressedEsc += 1;
            if (pressedEsc % 2 != 0)
            {
                foreach (GameObject element in elementsUIToHide)
                {
                    element.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject element in elementsUIToHide)
                {
                    element.SetActive(false);
                    
                }
            }
            
        }
    }
    public void OnSettingsClick()
    {
        settingPanel.SetActive(true);
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
    public void OnReturnClick()
    {
        SceneManager.LoadScene("HubScene");
    }

}
