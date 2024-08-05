using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_puchase : MonoBehaviour
{
    private DataTable Users;
    private DataTable table;

    private string query;
    private void Start()
    {

    }
    public void OnClick()
    {
        
        
        //Обращение к наименованию товара
        GameObject product = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        product = product.transform.GetChild(1).gameObject;
        TextMeshProUGUI productName = product.GetComponent<TextMeshProUGUI>();

        Users = Database.GetTable("SELECT * FROM User;");
        table = Database.GetTable("SELECT * FROM Shop,Items WHERE [Shop].item_id = [Items].item_id AND [Items].item_name = '"+ productName.text+"'; ");

        GameObject button = transform.parent.gameObject;
        GameObject button_text = button.transform.GetChild(0).gameObject;
        TextMeshProUGUI button_name = button_text.GetComponent<TextMeshProUGUI>();

        int price = Convert.ToInt32(button_name.text);
        int balance = Convert.ToInt32(Users.Rows[0][4].ToString());
        if (balance >= price)
        {
            button_name.text = "Blocked";
            button.GetComponent<Button>().interactable = false;
            balance -= price;
            query = Database.ExecuteQueryWithAnswer("INSERT INTO Player_inventories (user_id, item_id) VALUES (1, "+ table.Rows[0][2].ToString() +");");
            query = Database.ExecuteQueryWithAnswer("UPDATE User SET balance_free = " + balance.ToString() + " WHERE user_id = 1;");

            Debug.Log("Успешная покупка");
        }
        else Debug.Log("У вас мало денег");

    }
}
