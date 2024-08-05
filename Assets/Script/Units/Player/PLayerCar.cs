using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PLayerCar : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    public GameObject carCollider;
    public float playerHP = 100;
    public float ScoringTime = 10; // Множитель начисления очков в зависимости от времени
    public float lifeSeconds = 50;
    public float enginePower = 0f;
    public float bulletArmor = 0f;
    public float obtacleArmor = 0f;
    public Slider fuelSlider;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI HighScoreText;
    [SerializeField]
    private float _currentHealth;
    [SerializeField]
    public static float score;
    public static float HighScore;
    private float maxLifeSeconds = 50;
    //private BoxCollider _bc;
    List<Transform> childComponent = new List<Transform>();
    private DataTable userTable;
    private string activeUserID;
    private int userBalance;

    private float Timer;
    void Start()
    {
        Timer = 0;
        score = 0;
        _currentHealth = playerHP;

        activeUserID = UserHolder.DataHolder.Name;
        userTable = Database.GetTable("SELECT * FROM User WHERE [User].user_id = '" + activeUserID + "';");
        HighScore = Convert.ToInt32(userTable.Rows[0][3].ToString());
        enginePower = ConfirmCarParameter("engine");
        bulletArmor = ConfirmCarParameter("bulletArmor");
        obtacleArmor = ConfirmCarParameter("obtacleArmor");
        //for(int i = 0;i< inventoryTable.Rows.Count; i++)
        //{

        //}
        //_bc = carCollider.GetComponent<BoxCollider>();
        gameObject.GetComponent<CarTest>().moveSpeed = gameObject.GetComponent<CarTest>().moveSpeed * (1 + enginePower / 100);
    }
    private float ConfirmCarParameter(string update_type)
    {
        DataTable statsTable = new DataTable();
        statsTable = Database.GetTable("SELECT [Car_update_stat].stats FROM Player_car, Car_update_stat, Car_upgrade_types " +
            "WHERE [Player_car].car_update_id = [Car_update_stat].id " +
            "AND [Car_update_stat].update_type = [Car_upgrade_types].id " +
            "AND [Car_upgrade_types].update_type = '"+ update_type + "'");
        if (statsTable.Rows.Count > 0)
            return Convert.ToInt32(statsTable.Rows[0][0].ToString());
        else
            return 0;
    }
    public void ApplyDamage(float damageVaue)
    {
        _currentHealth -= damageVaue;
        if (_currentHealth <= 0)
        {
            DataHolder.Death = true;
            DataHolder.DeathReason = "Enemy";
            Database.ExecuteQueryWithAnswer("UPDATE User SET player_level = " + LevelSystem.DataHolder.Level + ", player_experience = " + LevelSystem.DataHolder.LevelExp + " WHERE user_id = " + activeUserID + ";");
            //Destroy(gameObject);
            Debug.Log("Вы погибли");
            //carCollider.AddComponent<Rigidbody>();
            try
            {
                carCollider.AddComponent<MeshCollider>().convex = true;
                carCollider.AddComponent<Rigidbody>().mass = 1000;
                carCollider.transform.localPosition = new Vector3(0, 1, 0);


                Debug.Log(carCollider.transform.childCount);
                if (carCollider.transform.childCount > 0)
                {

                    for (int i = 0; i < carCollider.transform.childCount; i++)
                    {
                        Transform child1 = carCollider.transform.GetChild(i);
                        childComponent.Add(child1);

                        child1.AddComponent<MeshCollider>().convex = true;
                        child1.AddComponent<Rigidbody>().mass = 1000;

                    }
                }
            }
            catch { }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obtacle")
        {
            BalanceHolder.DataHolder.Balance = 5;
            BalanceHolder.DataHolder.Salary = 5;
            LevelSystem.DataHolder.LevelExp = 15;
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log("Ты врезался");
                damageable.ApplyDamage(1000);
                ApplyDamage(10 * (1 - obtacleArmor / 100));
            }
        }

            
        if(collision.gameObject.tag == "Fuel")
        {
            score += 5;
            LevelSystem.DataHolder.LevelExp = 15;
            lifeSeconds = maxLifeSeconds;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "BonusHeal")
        {
            score += 5;
            LevelSystem.DataHolder.LevelExp = 15;
            _currentHealth = playerHP;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "BonusDamage")
        {
            LevelSystem.DataHolder.LevelExp = 15;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "BonusSpeed")
        {
            LevelSystem.DataHolder.LevelExp = 15;
            
            gameObject.GetComponent<CarTest>().moveSpeed += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Money")
        {
            score += 5;
            LevelSystem.DataHolder.LevelExp = 20;
            BalanceHolder.DataHolder.Balance = 50;
            BalanceHolder.DataHolder.Salary = 50;
            //userTable = Database.GetTable("SELECT * FROM User WHERE [User].user_id = '" + activeUserID + "';");
            //userBalance = Convert.ToInt32(userTable.Rows[0][2].ToString()) + 50;
            //Database.ExecuteQueryWithAnswer("UPDATE User SET balance_free = " + userBalance.ToString() + " WHERE user_id = " + activeUserID + ";");
            Destroy(collision.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(_currentHealth > 0)
        {
            hpText.text = _currentHealth.ToString();
            scoreText.text = score.ToString();
            HighScoreText.text = HighScore.ToString();
            lifeSeconds -= Time.deltaTime;
            if (lifeSeconds < 0)
            {
                DataHolder.Death = true;
                DataHolder.DeathReason = "Fuel";
                ApplyDamage(10000);
            }
                
            fuelSlider.value = lifeSeconds;

            // Cчет игрока
            Timer += Time.deltaTime;
            score = Mathf.Round(Timer) * ScoringTime;
            if (score > HighScore)
            {
                HighScore = score;
            }
        }
        
    }
    public static class DataHolder
    {
        private static string deathReason;
        private static bool death;
        public static string DeathReason
        {
            get
            {
                if(deathReason == "Fuel")
                {
                    return "У вас закончилось топливо";
                }
                else if (deathReason == "Enemy")
                {
                    return "Вас уничтожили";
                }
                else
                {
                    return "Вас уничтожили, но мы не знаем как";
                }
                    
            }
            set
            {
                deathReason = value;
            }
        }
        public static bool Death
        {
            get
            {
                return death;
            }
            set
            {
                death = value;
            }
        }
    }
}
