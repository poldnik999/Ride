using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryUnit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ShopList;

    public List<GameObject> AnotherButtons;
    public enum OPTIONS
    {
        All,
        Guns,
        Bonuses,
        Hub,
        CarUpdate,
        InventoryGun
        
    }
    public OPTIONS CategoryFilter;
    void Start()
    {
        if (CategoryFilter == OPTIONS.All)
            gameObject.GetComponent<Button>().interactable = false;

    }
    public void OnClick()
    {
        ShopList.GetComponent<ShopItemList>().shopFilter = (ShopItemList.OPTIONS)CategoryFilter;
        ShopList.GetComponent<ShopItemList>().OnClickSelectCategory();
        foreach(GameObject objects in AnotherButtons)
        {
            objects.GetComponent<Button>().interactable = true;
            objects.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
