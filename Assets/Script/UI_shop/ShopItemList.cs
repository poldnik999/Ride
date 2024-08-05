using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopItemList : MonoBehaviour
{
    public enum OPTIONS
    {
        All,
        Guns,
        Bonuses,
        Hub,
        CarUpdate,
        InventoryGun
    }
    public OPTIONS shopFilter;

    public GameObject shopUnit;
    public GameObject NameText;
    public GameObject DescText;
    public GameObject StatText;

    private DataTable table;
    private DataTable invGunTable;
    private string tableName = "Shop";
    private string query;
    private Vector3 startPos= new Vector3(26,-26);
    // Start is called before the first frame update
    void Start()
    {
        //switch (shopFilter)
        //{
        //    case OPTIONS.All:
        //        query = "hub_update";
        //        table = Database.GetTable("SELECT [Items].item_name, [Items].description, [Items].price_free, [Shop].level_requirements, [Items].name, [Items].stats, [Items].item_type " +
        //            "FROM " + tableName + ", Items " +
        //            "WHERE [" + tableName + "].item_id = [Items].item_id;");
        //        PlaceUnits(GetUnitsPos(table), GetComponent<ShopUnit>(), table);
        //        break;
        //    case OPTIONS.Guns:
        //        query = "guns";
        //        table = Database.GetTable("SELECT [Items].item_name, [Items].description, [Items].price_free, [Shop].level_requirements, [Items].name, [Items].stats, [Items].item_type FROM " + tableName + ", Items WHERE [" + tableName + "].item_id = [Items].item_id AND item_type = '" + query + "';");
        //        break;
        //    case OPTIONS.CarUpdate:
        //        query = "car_update";
        //        table = Database.GetTable("SELECT " +
        //            "[Car_update_stat].name, [Car_update_stat].description, [Car_update_stat].price, [CarUpdateShop].level_requirement, [Car_update_stat].img_name, [Car_update_stat].stats, [Car_update_stat].update_type " +
        //            "FROM CarUpdateShop, Car_update_stat " +
        //            "WHERE [CarUpdateShop].car_update_id = [Car_update_stat].id;");
        //        PlaceUnits(GetUnitsPos(table), GetComponent<ShopUnitCar>(), table);
        //        break;
        //    case OPTIONS.InventoryGun:
        //        query = "guns";
        //        tableName = "Player_inventories";
        //        invGunTable = Database.GetTable("SELECT " +
        //            "[Items].item_name, [Items].description, [Items].name, [Items].stats, [Items].item_type " +
        //            "FROM " + tableName + ", Items " +
        //            "WHERE [" + tableName + "].item_id = [Items].item_id " +
        //            "AND item_type = '" + query + "' " +
        //            "AND user_id = '"+UserHolder.DataHolder.Name+"';");
        //        PlaceUnits(GetUnitsPos(invGunTable), GetComponent<GunUnit>(), invGunTable);
        //        break;
        //}
        OnClickSelectCategory();
        //PlaceShopUnits(table.Rows.Count, table);
        //Debug.Log("Shop utems count loaded: " + table.Rows.Count);
    }
    public void OnClickSelectCategory()
    {
        switch (shopFilter)
        {
            case OPTIONS.All:
                table = Database.GetTable("SELECT [Items].item_name, [Items].description, [Items].price_free, [Shop].level_requirements, [Items].name, [Items].stats, [Items].item_type " +
                    "FROM " + tableName + ", Items " +
                    "WHERE [" + tableName + "].item_id = [Items].item_id;");
                PlaceUnits(GetUnitsPos(table), GetComponent<ShopUnit>(), table);
                break;

            case OPTIONS.Guns:
                query = "guns";
                table = Database.GetTable("SELECT [Items].item_name, [Items].description, [Items].price_free, [Shop].level_requirements, [Items].name, [Items].stats, [Items].item_type " +
                    "FROM " + tableName + ", Items " +
                    "WHERE [" + tableName + "].item_id = [Items].item_id " +
                    "AND [Items].item_type = '"+query+"';");
                PlaceUnits(GetUnitsPos(table), GetComponent<ShopUnit>(), table);
                break;
            case OPTIONS.Bonuses:
                query = "bonuses";
                table = Database.GetTable("SELECT [Items].item_name, [Items].description, [Items].price_free, [Shop].level_requirements, [Items].name, [Items].stats, [Items].item_type " +
                    "FROM " + tableName + ", Items " +
                    "WHERE [" + tableName + "].item_id = [Items].item_id " +
                    "AND [Items].item_type = '" + query + "';");
                PlaceUnits(GetUnitsPos(table), GetComponent<ShopUnit>(), table);
                break;
            case OPTIONS.Hub:
                query = "hub_update";
                table = Database.GetTable("SELECT [Items].item_name, [Items].description, [Items].price_free, [Shop].level_requirements, [Items].name, [Items].stats, [Items].item_type " +
                    "FROM " + tableName + ", Items " +
                    "WHERE [" + tableName + "].item_id = [Items].item_id " +
                    "AND [Items].item_type = '" + query + "';");
                PlaceUnits(GetUnitsPos(table), GetComponent<ShopUnit>(), table);
                break;
            case OPTIONS.InventoryGun:
                query = "guns";
                tableName = "Player_inventories";
                invGunTable = Database.GetTable("SELECT " +
                    "[Items].item_name, [Items].description, [Items].name, [Items].stats, [Items].item_type " +
                    "FROM " + tableName + ", Items " +
                    "WHERE [" + tableName + "].item_id = [Items].item_id " +
                    "AND item_type = '" + query + "' " +
                    "AND user_id = '" + UserHolder.DataHolder.Name + "';");
                PlaceUnits(GetUnitsPos(invGunTable), GetComponent<GunUnit>(), invGunTable);
                break;
            case OPTIONS.CarUpdate:
                query = "car_update";
                table = Database.GetTable("SELECT " +
                    "[Car_update_stat].name, [Car_update_stat].description, [Car_update_stat].price, [CarUpdateShop].level_requirement, [Car_update_stat].img_name, [Car_update_stat].stats, [Car_update_stat].update_type " +
                    "FROM CarUpdateShop, Car_update_stat " +
                    "WHERE [CarUpdateShop].car_update_id = [Car_update_stat].id;");
                PlaceUnits(GetUnitsPos(table), GetComponent<ShopUnitCar>(), table);
                break;
        }
    }
    // Метод возвращает список координат в которых будут располагаться ячейки магазина
    private List<Vector3> GetUnitsPos(DataTable table)
    {
        
        Vector3 unitPos = startPos;
        List<Vector3> unitsPos = new List<Vector3>();
        unitsPos.Add(startPos);
        for (int i = 1; i < table.Rows.Count; i++)
        {
            if (i % 4 != 0)
            {
                unitPos = new Vector3(unitPos.x + 56, unitPos.y);
                unitsPos.Add(unitPos);
            }
            else
            {
                unitPos = new Vector3(startPos.x, unitPos.y - 56);
                unitsPos.Add(unitPos);
            }
        }
        return unitsPos;
    }
    private void PlaceUnits(List<Vector3> unitsPos, ShopUnit name, DataTable table)
    {
        ClearUnits();
        GameObject newShopUnit;
        for (int j = 0; j < unitsPos.Count; j++)
        {
            newShopUnit = Instantiate(shopUnit, unitsPos[j], Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z));
            newShopUnit.GetComponent<ShopUnit>()._name = NameText;
            newShopUnit.GetComponent<ShopUnit>()._description = DescText;
            newShopUnit.GetComponent<ShopUnit>()._stat = StatText;

            newShopUnit.GetComponent<ShopUnit>().name = table.Rows[j][0].ToString();
            newShopUnit.GetComponent<ShopUnit>().description = table.Rows[j][1].ToString();
            newShopUnit.GetComponent<ShopUnit>().price = table.Rows[j][2].ToString();
            newShopUnit.GetComponent<ShopUnit>().lvl_requirement = table.Rows[j][3].ToString();
            newShopUnit.GetComponent<ShopUnit>().img_name = table.Rows[j][4].ToString();
            newShopUnit.GetComponent<ShopUnit>().stat = table.Rows[j][5].ToString();
            newShopUnit.GetComponent<ShopUnit>().item_type = table.Rows[j][6].ToString();
            //Назначаем родителя ячейкам списка, чтобы они появлялись на интерфейсе, а не где либо еще
            newShopUnit.transform.SetParent(gameObject.transform, false);
        }
    }
    
    private void PlaceUnits(List<Vector3> unitsPos, ShopUnitCar name, DataTable table)
    {
        ClearUnits();
        GameObject newShopUnit;
        for (int j = 0; j < unitsPos.Count; j++)
        {
            newShopUnit = Instantiate(shopUnit, unitsPos[j], Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z));
            newShopUnit.GetComponent<ShopUnitCar>()._name = NameText;
            newShopUnit.GetComponent<ShopUnitCar>()._description = DescText;
            newShopUnit.GetComponent<ShopUnitCar>()._stat = StatText;

            newShopUnit.GetComponent<ShopUnitCar>().name = table.Rows[j][0].ToString();
            newShopUnit.GetComponent<ShopUnitCar>().description = table.Rows[j][1].ToString();
            newShopUnit.GetComponent<ShopUnitCar>().price = table.Rows[j][2].ToString();
            newShopUnit.GetComponent<ShopUnitCar>().lvl_requirement = table.Rows[j][3].ToString();
            newShopUnit.GetComponent<ShopUnitCar>().img_name = table.Rows[j][4].ToString();
            newShopUnit.GetComponent<ShopUnitCar>().stat = table.Rows[j][5].ToString();
            newShopUnit.GetComponent<ShopUnitCar>().item_type = table.Rows[j][6].ToString();
            //Назначаем родителя ячейкам списка, чтобы они появлялись на интерфейсе, а не где либо еще
            newShopUnit.transform.SetParent(gameObject.transform, false);
        }
    }
    private void PlaceUnits(List<Vector3> unitsPos, GunUnit name, DataTable table)
    {
        ClearUnits();
        GameObject newShopUnit;
        for (int j = 0; j < unitsPos.Count; j++)
        {
            newShopUnit = Instantiate(shopUnit, unitsPos[j], Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z));
            newShopUnit.GetComponent<GunUnit>()._name = NameText;
            newShopUnit.GetComponent<GunUnit>()._description = DescText;

            newShopUnit.GetComponent<GunUnit>().name = invGunTable.Rows[j][0].ToString();
            newShopUnit.GetComponent<GunUnit>().description = invGunTable.Rows[j][1].ToString();
            newShopUnit.GetComponent<GunUnit>().img_name = invGunTable.Rows[j][2].ToString();
            newShopUnit.GetComponent<GunUnit>().stat = invGunTable.Rows[j][3].ToString();
            //Назначаем родителя ячейкам списка, чтобы они появлялись на интерфейсе, а не где либо еще
            newShopUnit.transform.SetParent(gameObject.transform, false);
        }
    }
    private void ClearUnits()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
