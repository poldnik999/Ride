using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class UI_StartIcon : MonoBehaviour
{
    public GameObject ProfileUnit;
    public GameObject AddUserPanel;
    public GameObject ContentPanel;
    public GameObject UIToHide;
    public TextMeshProUGUI UserInputValue;

    private Vector3 startPos = new Vector3(0, -48);
    private DataTable userTable;
    
    // Start is called before the first frame update
    void Start()
    {
        UIToHide.SetActive(false);
        userTable = Database.GetTable("SELECT * FROM User;");
        PlaceProfileUnits(userTable.Rows.Count, userTable) ;

        AddUserPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Debug.Log("enter!!");
            OnClickEnter();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIToHide.SetActive(false);
        }
    }
    // Открытие формы для создания профиля
    public void OnClickCreate()
    {
        AddUserPanel.SetActive(true);
    }

    // Добавление пользователя в БД
    public void OnClickAdd()
    {
        Database.ExecuteQueryWithAnswer("INSERT INTO User (user_nickname, balance_free, High_score, player_level, player_experience) VALUES ('" + UserInputValue.text + "', 50, 0, 1, 0);");
        DataTable table = Database.GetTable("SELECT user_id FROM User WHERE user_nickname = '"+ UserInputValue.text + "';");
        Database.ExecuteQueryWithAnswer("INSERT INTO Player_inventories (user_id, item_id) VALUES ('" +table.Rows[0][0].ToString() + "', 2);");
        AddUserPanel.SetActive(false);
        
        userTable = Database.GetTable("SELECT * FROM User;");
        PlaceProfileUnits(userTable.Rows.Count, userTable);
    }
    public void OnClickEnter()
    {
        UIToHide.SetActive(true);
    }
    private void ClearUserPanels()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("UserProfileUnit");
        foreach(GameObject obj in objects)
            Destroy(obj);
    }
    private void PlaceProfileUnits(int RowCount, DataTable table)
    {
        ClearUserPanels();
        GameObject testPlaceUserUnit;
        Vector3 unitPos = startPos;
        List<Vector3> unitsPos = new List<Vector3>();
        unitsPos.Add(startPos);
        for (int i = 1; i < RowCount; i++)
        {
            unitPos = new Vector3(startPos.x, unitPos.y - 72);
            unitsPos.Add(unitPos);
        }
        for (int j = 0; j < unitsPos.Count; j++)
        {
            testPlaceUserUnit = Instantiate(ProfileUnit, unitsPos[j], Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z));
            testPlaceUserUnit.GetComponent<UserProfileUnit>().userID = table.Rows[j][0].ToString();
            testPlaceUserUnit.GetComponent<UserProfileUnit>().userNickname = table.Rows[j][1].ToString();
            testPlaceUserUnit.GetComponent<UserProfileUnit>().userLevel = table.Rows[j][4].ToString();
            
            testPlaceUserUnit.transform.SetParent(ContentPanel.transform, false);
        }
        Debug.Log(gameObject.transform.GetChild(0).name);
    }
}
