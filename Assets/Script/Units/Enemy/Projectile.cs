using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Unit_HP bullet;
    public bool isEnemy = false;
    [SerializeField]
    private float speed;
    public int Damage = 10;
    private Vector3 firePoint;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
        bullet = new Unit_HP();
        bullet.Name = "bullet";
        bullet.HealthPoint = Damage;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move();
    }

    void Move()
    {
        
        _rb.AddForce(transform.forward * speed);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Obtacle")
        {
            BalanceHolder.DataHolder.Balance = 5;
            BalanceHolder.DataHolder.Salary = 5;
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log("Ты попал");
                damageable.ApplyDamage(15);
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.tag == "Player" && isEnemy)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log("В тебя попали");
                damageable.ApplyDamage(5);
                Destroy(gameObject);
            }
        }

    }
}
