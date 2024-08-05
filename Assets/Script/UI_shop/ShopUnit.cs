using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Data;
using System;

public class ShopUnit : MonoBehaviour
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
    private TextMeshProUGUI _balanceValue;
    private GameObject _levelText;
    private string activeUserID;
    private int activeUserLvl;
    // Start is called before the first frame update
    void Start()
    {
        activeUserID = UserHolder.DataHolder.Name;
        activeUserLvl = LevelSystem.DataHolder.Level;
        shopTable = Database.GetTable("SELECT id, item_name FROM Shop,Items WHERE [Shop].item_id = [Items].item_id;");
        userTable = Database.GetTable("SELECT * FROM User WHERE [User].user_id = '" + activeUserID + "';");
        inventoryTable = Database.GetTable("SELECT * FROM Player_Inventories WHERE [Player_Inventories].user_id = '"+ activeUserID + "';");
        _priceButton = gameObject.transform.GetChild(1).gameObject;
        _priceButton.GetComponentInChildren<TextMeshProUGUI>().text = price;
        _levelText = gameObject.transform.GetChild(2).gameObject;
        _levelText.GetComponent<TextMeshProUGUI>().text = lvl_requirement + " lvl";
        compareWithInventory(shopTable, inventoryTable);
        gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + img_name);

        userBalance = Convert.ToInt32(userTable.Rows[0][2].ToString()) - Convert.ToInt32(price);
        canBuy(Convert.ToInt32(price));

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
        if(item_type == "guns")
            _stat.GetComponent<TextMeshProUGUI>().text = "Óðîí: "+ stat + "åä.";
        if (item_type == "bonuses")
            _stat.GetComponent<TextMeshProUGUI>().text = "Áîíóñ: " + stat + "åä.";
        if (item_type == "hub_update")
            _stat.GetComponent<TextMeshProUGUI>().text = "Îïûò: " + stat + "%";
    }
    public void OnClickPurchase()
    {
        shopTable = Database.GetTable("SELECT id, item_name FROM Shop, Items WHERE [Shop].item_id = [Items].item_id;");
        userTable = Database.GetTable("SELECT * FROM User WHERE [User].user_id = '" + activeUserID + "';");
        userBalance = Convert.ToInt32(userTable.Rows[0][2].ToString()) - Convert.ToInt32(price);
        if (canBuy(Convert.ToInt32(price)))
        {
            BalanceHolder.DataHolder.Balance = -Convert.ToInt32(price);
            Debug.Log("SOLD!!");
            purchaseItem(shopTable, price);
            inventoryTable = Database.GetTable("SELECT * FROM Player_Inventories WHERE [Player_Inventories].user_id = '" + activeUserID + "';");
            Debug.Log("compare with user inventory...");
            compareWithInventory(shopTable, inventoryTable);
        }
        else
            Debug.Log("Not enough money!!");

    }
    private void purchaseItem(DataTable table, string price)
    {
        string shopItemId = "";
        for(int i = 0; i < table.Rows.Count; i++)
        {
            if(name == table.Rows[i][1].ToString())
            {
                shopItemId = table.Rows[i][0].ToString();
                break;
            }
                
        }
        //ÇÀÏÐÎÑÛ Ê ÁÄ ÍÀ ÄÎÁÀÂËÅÍÈÅ ÇÀÏÈÑÈ Â ÒÀÁËÈÖÓ ÈÍÂÅÍÒÀÐß ÈÃÐÎÊÀ
        Database.ExecuteQueryWithAnswer("INSERT INTO Player_inventories (user_id, item_id) VALUES ("+ activeUserID + ", " + shopItemId + ");");
        Database.ExecuteQueryWithAnswer("UPDATE User SET balance_free = " + userBalance.ToString() + " WHERE user_id = "+ activeUserID + ";");
        
    }
    private bool canBuy(int price)
    {
        if (userBalance >= price && activeUserLvl >= Convert.ToInt32(lvl_requirement))
            return true;
        else
        {
            _priceButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            _levelText.GetComponent<TextMeshProUGUI>().faceColor = Color.red;
            return false;
        }
            
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
