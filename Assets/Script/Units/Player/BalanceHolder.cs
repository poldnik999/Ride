using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class BalanceHolder : MonoBehaviour
{
    private DataTable userTable;
    private string activeUserID;
    private static bool start = true;
    private TextMeshProUGUI _balanceValue;
    // Start is called before the first frame update
    void Start()
    {
        DataHolder.StartScr = true;
        DataHolder.Salary = 0;
        activeUserID = UserHolder.DataHolder.Name;
        userTable = Database.GetTable("SELECT * FROM User WHERE [User].user_id = '" + activeUserID + "';");
        DataHolder.Balance = Convert.ToInt32(userTable.Rows[0][2].ToString());
        DataHolder.StartScr = false;
        _balanceValue = GameObject.FindGameObjectWithTag("BalanceValueText").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        _balanceValue.text = DataHolder.Balance.ToString();
    }
    public static class DataHolder
    {
        private static int userBalance;
        private static int salary;
        public static int Balance
        {
            get
            {
                return userBalance;
            }
            set
            {
                if (StartScr)
                {
                    userBalance = value;
                }
                else
                {
                    userBalance += value;
                }
                    
            }
        }
        public static int Salary
        {
            get
            {
                return salary;
            }
            set
            {
                salary += value;
            }
        }
        public static bool StartScr
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }
    }
}
