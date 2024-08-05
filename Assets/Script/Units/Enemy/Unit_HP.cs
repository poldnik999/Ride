using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageable
{
    void ApplyDamage(float damageValue);
}
public class Unit_HP
{
    // Значения по умолчанию
    private string name = "Unit";
    private int healthPoint = 100;
    public bool lifeStatus = true; // Статус жизни обьекта

    public int HealthPoint
    {
        // Если хп объекта < 0, то у него меняется статус на "мертвый"
        set
        {
            if (healthPoint < 0)
                lifeStatus = false;
            else
                healthPoint = value;
        }
        get { return healthPoint; }
    }
    public string Name
    {
        set { name = value; }
        get { return name; }
    }
    public void Damage(int dmg)
    {
        this.HealthPoint -= dmg;
    }

    // В скрипте в методе update проверяй статус жизни объекта, если он мертв, то уничтожай объект через Destroy()

    // Работает примерно так:                                // Еще попробовал поставить хп пуле, чтобы хп пули = урону пули
    //Unit_HP Unit_01;                                       // При столкновении пули с кубом, у куба отнимается столько хп, сколько имеет пуля  
    //void Start()                                           // Unit_HP bullet = collision.gameObject.GetComponent<Projectile>().bullet;     // Projectile это твой скрипт пули
    //{                                                      // Unit_01.Damage(bullet.healthPoint); 
    //    Unit_01 = new Unit_HP();                           // Можно через GetComponent получать данные из другого скрипта, если они указанны как public
    //    Unit_01.healthPoint = 120;
    //    Unit_01.Name = "Cube";
    //}
    //void Update()
    //{
    //    if (Unit_01.lifeStatus == false)
    //        Destroy(gameObject);
    //}
}

