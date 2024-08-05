using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUnit : MonoBehaviour
{
    public string name;
    public string description;
    public string price;
    public string lvl_requirement;
    public string img_name;
    public string stat;
    public GameObject _hub;
    public GameObject _name;
    public GameObject _description;
    private GameObject _priceButton;
    // Start is called before the first frame update
    void Start()
    {
        _hub = GameObject.Find("Hub");
        
        _priceButton = gameObject.transform.GetChild(1).gameObject;
        _priceButton.GetComponentInChildren<TextMeshProUGUI>().text = "Взять";
        gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + img_name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        Debug.Log("User clicked on shop item!");
        _name.GetComponent<TextMeshProUGUI>().text = name;
        _description.GetComponent<TextMeshProUGUI>().text = description;
    }
    public void OnClickEquip()
    {
        ClearEquipped();
        GunHolder.DataHolder.Prefab = (GameObject)Resources.Load("Guns/" + img_name);
        _hub.GetComponent<GunHolder>().pressedEquipButton = _priceButton.GetComponent<Button>();
        _priceButton.GetComponentInChildren<TextMeshProUGUI>().text = "Взято";
        _priceButton.GetComponent<Button>().interactable = false;
    }
    public void ClearEquipped()
    {
        //GunHolder.DataHolder.Prefab = null;
        GameObject parent = gameObject.transform.parent.gameObject;
        List<GameObject> childrens = new List<GameObject>();
        for(int i = 0;i< parent.transform.childCount;i++)
        {
            childrens.Add(parent.transform.GetChild(i).gameObject);
        }
        foreach (GameObject child in childrens)
        {
            child.transform.GetChild(1).GetComponent<Button>().interactable = true;
            child.transform.GetChild(1).GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "Взять";
        }
    }
}
