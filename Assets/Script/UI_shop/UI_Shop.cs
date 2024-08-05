using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Shop : MonoBehaviour
{
    public GameObject _rotate_obj;
    public GameObject _shop_icon;
    private DataTable Inventory;
    private DataTable table = Database.GetTable("SELECT * FROM Shop,Items WHERE [Shop].item_id = [Items].item_id AND item_type = 'hub_update';");
    // Start is called before the first frame update
    string str = "";
    void Start()
    {
        switch (_rotate_obj.name)
        {
            case "Gun_shop_cam":
                table = Database.GetTable("SELECT * FROM Shop,Items WHERE [Shop].item_id = [Items].item_id AND item_type = 'guns';");
                break;
            case "Car_update_cam":
                table = Database.GetTable("SELECT * FROM Shop,Items WHERE [Shop].item_id = [Items].item_id AND (item_type = 'car_update' OR item_type = 'car_colors');");
                break;
        }

        for (int i = 0; i < table.Rows.Count; i++)
        {
            for (int j = 0; j < table.Columns.Count; j++)
                str += table.Rows[i][j].ToString() + "  ";
            Debug.Log(str);
            str = "";
        }
        Open_visual_shop(table.Rows.Count, _shop_icon);
    }

    void Open_visual_shop(int rows_count,GameObject parent_icon)
    {
        
        GameObject icon;
        GameObject product_image;
        GameObject product_name;
        GameObject submit_button;
        GameObject parent_canvas = gameObject;
        Inventory = Database.GetTable("SELECT * FROM Player_Inventories");

        Vector3 rotate = _rotate_obj.transform.rotation.eulerAngles;
        Vector3 parent_canvas_pos = parent_canvas.transform.position;
        for (int i = 0; i < rows_count; i++)
        {
            
            icon = Instantiate(parent_icon, parent_canvas_pos, Quaternion.Euler(rotate.x,rotate.y,rotate.z), parent_canvas.transform);
            icon.transform.SetParent(parent_canvas.transform, false);

            product_image = icon.transform.GetChild(0).gameObject;
            product_name = icon.transform.GetChild(1).gameObject;
            submit_button = icon.transform.GetChild(2).gameObject;

            

            UnityEngine.UI.Image sprite_img = product_image.GetComponent<UnityEngine.UI.Image>();
            sprite_img.sprite = Resources.Load<Sprite>("Image " + (i).ToString());

            TextMeshProUGUI name = product_name.GetComponent<TextMeshProUGUI>();
            name.text = table.Rows[i][3].ToString();

            GameObject button_text = submit_button.transform.GetChild(0).gameObject;
            TextMeshProUGUI button_name = button_text.GetComponent<TextMeshProUGUI>();
            button_name.text = table.Rows[i][5].ToString();
            for (int j = 0; j < Inventory.Rows.Count; j++)
            {
                if (Inventory.Rows[j][2].ToString() == table.Rows[i][2].ToString())
                {
                    UnityEngine.UI.Button subBut = submit_button.GetComponent<UnityEngine.UI.Button>();
                    button_name.text = "Blocked";
                    subBut.interactable = false;
                    
                }
                
            }
            RectTransform rect_icon = icon.GetComponent<RectTransform>();
            rect_icon.offsetMin = new Vector2(10 + 140 * i,rect_icon.offsetMin.y);
            rect_icon.offsetMax = new Vector2(120 + 140 * i, rect_icon.offsetMax.y);
            //rect_icon.rotation = Quaternion.identity;

        }
        
    }
}
