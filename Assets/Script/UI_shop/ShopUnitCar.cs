using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUnitCar : MonoBehaviour
{
    public string name;
    public string description;
    public string price;
    public string lvl_requirement;
    public string img_name;
    public string stat;
    public string item_type;
    private int userBalance;
    private DataTable userTable;
    private DataTable shopTable;
    private DataTable inventoryTable;

    public GameObject _name;
    public GameObject _description;
    public GameObject _stat;
    private GameObject _priceButton;
    
    private string activeUserID;
    // Start is called before the first frame update
    void Start()
    {
        activeUserID = UserHolder.DataHolder.Name;
        shopTable = Database.GetTable("SELECT [CarUpdateShop].id, name FROM CarUpdateShop, Car_update_stat WHERE [CarUpdateShop].car_update_id = [Car_update_stat].id;");
        userTable = Database.GetTable("SELECT * FROM User WHERE [User].user_id = '" + activeUserID + "';");
        inventoryTable = Database.GetTable("SELECT * FROM Player_car WHERE [Player_car].user_id = '" + activeUserID + "';");
        _priceButton = gameObject.transform.GetChild(1).gameObject;
        _priceButton.GetComponentInChildren<TextMeshProUGUI>().text = price;
        compareWithInventory(shopTable, inventoryTable);
        gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + img_name);
    }

    // Update is called once per frame
    void Update()
    {
        //userBalance = Convert.ToInt32(userTable.Rows[0][2].ToString());
    }
    public void OnClick()
    {
        Debug.Log("User clicked on shop item!");
        _name.GetComponent<TextMeshProUGUI>().text = name;
        _description.GetComponent<TextMeshProUGUI>().text = description;
        if(item_type == "1")
            _stat.GetComponent<TextMeshProUGUI>().text = "Ñêîðîñòü : +"+stat + "%";
        if (item_type == "2")
            _stat.GetComponent<TextMeshProUGUI>().text = "Ëîá. áðîíÿ : +" + stat + "%";
        if (item_type == "3")
            _stat.GetComponent<TextMeshProUGUI>().text = "Ïóë. áðîíÿ : +" + stat + "%";
    }
    public void OnClickPurchase()
    {
        shopTable = Database.GetTable("SELECT [CarUpdateShop].id, name FROM CarUpdateShop, Car_update_stat WHERE [CarUpdateShop].car_update_id = [Car_update_stat].id;");
        userTable = Database.GetTable("SELECT * FROM User WHERE [User].user_id = '" + activeUserID + "';");
        userBalance = Convert.ToInt32(userTable.Rows[0][2].ToString()) - Convert.ToInt32(price);
        if (canBuy(Convert.ToInt32(price)))
        {
            Debug.Log("SOLD!!");
            purchaseItem(shopTable, price);
            inventoryTable = Database.GetTable("SELECT * FROM Player_car WHERE [Player_car].user_id = '" + activeUserID + "';");
            Debug.Log("compare with user inventory...");
            compareWithInventory(shopTable, inventoryTable);
        }
        else
            Debug.Log("Not enough money!!");

    }
    private void purchaseItem(DataTable table, string price)
    {
        string shopItemId = "";
        for (int i = 0; i < table.Rows.Count; i++)
        {
            if (name == table.Rows[i][1].ToString())
            {
                shopItemId = table.Rows[i][0].ToString();
                break;
            }

        }
        //ÇÀÏÐÎÑÛ Ê ÁÄ ÍÀ ÄÎÁÀÂËÅÍÈÅ ÇÀÏÈÑÈ Â ÒÀÁËÈÖÓ ÈÍÂÅÍÒÀÐß ÈÃÐÎÊÀ
        Database.ExecuteQueryWithAnswer("INSERT INTO Player_car (user_id, car_update_id) VALUES ("+ activeUserID + ", " + shopItemId + ");");
        Database.ExecuteQueryWithAnswer("UPDATE User SET balance_free = " + userBalance.ToString() + " WHERE user_id = "+ activeUserID + ";");

    }
    private bool canBuy(int price)
    {
        if (userBalance >= price)
            return true;
        else
            return false;
    }

    private void compareWithInventory(DataTable shopTable, DataTable inventoryTable)
    {

        for (int i = 0; i < shopTable.Rows.Count; i++)
        {
            for (int j = 0; j < inventoryTable.Rows.Count; j++)
            {
                if (inventoryTable.Rows[j][2].ToString() == shopTable.Rows[i][0].ToString() && name == shopTable.Rows[i][1].ToString())
                {
                    Debug.Log(shopTable.Rows[i][1].ToString());

                    _priceButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sold";
                    _priceButton.GetComponent<Button>().interactable = false;
                }


            }
        }
        Debug.Log("Success");
    }
}
