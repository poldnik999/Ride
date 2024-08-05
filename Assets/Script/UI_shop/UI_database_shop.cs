using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System;

public class UI_database_shop : MonoBehaviour
{
    public GameObject _image;
    public GameObject _text_description;
    private DataTable table = Database.GetTable("SELECT * FROM Shop,Items WHERE [Shop].item_id = [Items].item_id;");

    //Отступы описания предметов магазина относительно картинки
    public float _description_indent_right = 140f;
    public float _description_indent_down = -30f;

    //Отступы картинок предметов магазина
    public float _image_indent_right = 90f;
    public float _image_indent_down = 40f;
    void Start()
    {
        //DataTable table = Database.GetTable("SELECT * FROM Shop,Items WHERE [Shop].item_id = [Items].item_id;");
        for(int i=0;i<table.Rows.Count;i++)
        {
            Debug.Log(table.Rows[i][1].ToString() +"  "+ table.Rows[i][2].ToString() + "  " + table.Rows[i][3].ToString() + "  " + table.Rows[i][4].ToString());
        }
        GameObject parent = gameObject;
        Vector3 parent_pos = new Vector3(parent.transform.position.x, parent.transform.position.y, parent.transform.position.z);

        // Кол-во записей в базе данных. Или кол-во товара.
        int num = 10;
        Import_database_shop(table.Rows.Count, parent, parent_pos);

    }
    void Import_database_shop(int count, GameObject parent_obj, Vector3 parent_position)
    {
        GameObject image;
        GameObject text_description;
        Vector2 rect_tranform = new Vector2(0, 0.5f); // Rect Transform (Left-Middle)
        Vector2 pivot = new Vector2(0.5f, 0.5f);

        for (int i = 1;i <= count;i++)
        {
            image = Instantiate(_image, parent_position, Quaternion.identity, parent_obj.transform);
            image.transform.SetParent(parent_obj.transform, false);
            RectTransform rt_image = image.GetComponent<RectTransform>();


            //Задание свойств Rect Transform (Left-Middle) у Image
            rt_image.anchorMin = rect_tranform;
            rt_image.anchorMax = rect_tranform;
            rt_image.pivot = pivot;
            rt_image.anchoredPosition = new Vector2(i * _image_indent_right, _image_indent_down); // Отступ картинок

            //Добавление изображений товара из папки Assets/Resources/
            UnityEngine.UI.Image sprite_img = image.GetComponent<UnityEngine.UI.Image>();
            sprite_img.sprite = Resources.Load<Sprite>("Image " + i);

            text_description = Instantiate(_text_description, image.transform.position, Quaternion.identity, image.transform);
            text_description.transform.SetParent(image.transform, false);
            RectTransform rt_text = text_description.GetComponent<RectTransform>();

            //Задание свойств Rect Transform (Left-Middle) у Text
            rt_text.anchorMin = new Vector2(0f, 0f);
            rt_text.anchorMax = new Vector2(1f, 0f);
            rt_text.pivot = pivot;
            rt_text.anchoredPosition = new Vector2(0, -50); // Отступ текста
            rt_text.sizeDelta = new Vector2(60f, 40f);

            TextMeshProUGUI description = text_description.GetComponent<TextMeshProUGUI>();
            description.text = table.Rows[i-1][3].ToString();
        }
        

        
    }
}
