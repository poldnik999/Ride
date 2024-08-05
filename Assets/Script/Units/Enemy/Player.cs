using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    public Unit_HP unit;
    public GameObject playerShoot;
    DataTable table;

    public float moveSpeed;
    public float sideSpeed;

    public float ScoringTime = 10; // Множитель начисления очков в зависимости от времени

    private float Timer;
    public float HighScore;

    public float score;
    private Rigidbody _rb;
    [SerializeField] private float _maxHealth;
    private float _currentHealth;
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _currentHealth = _maxHealth;
        //table = Database.GetTable("SELECT * FROM User;");
        unit = new Unit_HP();
        unit.Name = "Dodge";
        unit.HealthPoint = 100;

        Timer = 0;
        score = 0;
        //HighScore = Convert.ToInt32(table.Rows[0][6].ToString());
    }
    private void MovementLogic()
    {
        //_rb.velocity = new Vector3(0, 0, 5);
        Vector3 movement;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal * sideSpeed, 0.0f, 5f * moveSpeed);
        _rb.AddForce(movement);
    }
    public void ApplyDamage(float damageVaue)
    {
        _currentHealth -= damageVaue;
        if (_currentHealth <= 0)
        {
            unit.lifeStatus = false;
        }
    }
    private void FixedUpdate()
    {
        MovementLogic();
    }
    void Update()
    {
        
        unit.HealthPoint = (int)_currentHealth;
        // Смерть обьекта
        if (unit.lifeStatus == false)
        {
            Debug.Log("Кто-то умер");
            //string query = Database.ExecuteQueryWithAnswer("UPDATE User SET High_score = " + Convert.ToInt32(HighScore.ToString()) + " WHERE user_id = 1;");
            //SceneManager.LoadScene("HubScene");
        }
        

        // Cчет игрока
        Timer += Time.deltaTime;
        score = Mathf.Round(Timer) * ScoringTime;
        if (score > HighScore)
        {
            HighScore = score;
        }
        

        //transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.World);




        // Управление
        //if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * Time.deltaTime * sideSpeed);

        //if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * Time.deltaTime * sideSpeed);
        
        if (Input.GetKey(KeyCode.Escape)) unit.Damage(100);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.ApplyDamage(10);
        }

    }
}
