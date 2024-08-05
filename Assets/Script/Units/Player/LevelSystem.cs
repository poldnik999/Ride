using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static double levelReq;
    private DataTable userTable;
    private string activeUserID;
    private static bool start = true;
    void Start()
    {
        DataHolder.StartScr = true;
        activeUserID = UserHolder.DataHolder.Name;
        userTable = Database.GetTable("SELECT * FROM User WHERE [User].user_id = '" + activeUserID + "';");
        DataHolder.Level = Convert.ToInt32(userTable.Rows[0][4].ToString());
        DataHolder.LevelExp = Convert.ToInt32(userTable.Rows[0][5].ToString());
        start = false;
    }
    public static class DataHolder
    {
        private static int level = 0;
        private static int levelExp;


        public static int LevelExp
        {
            get
            {
                levelReq = 100 * Math.Pow(1.3, Level);
                if (levelExp > levelReq)
                {
                    StartScr = false;
                    Level = 1;
                    levelExp -= (int)levelReq;
                }
                return levelExp;
            }
            set
            {
                levelExp += value;
            }
        }
        public static int Level
        {
            get
            {
                return level;
            }
            set
            {
                if(!StartScr)
                    level += value;
                else
                    level = value;
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
