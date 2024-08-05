using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, IDamageable
{
    public bool _isObject;
    public bool _isEnemy;
    public bool _isPlayer;
    public int _health = 100;
    private float _currentHealth;

    private List<Transform> childComponent = new List<Transform>();
    void Start()
    {
        _currentHealth = _health;
    }
    private string name = "Unit";
    private int healthPoint = 100;
    public bool lifeStatus = true; // Статус жизни обьекта
    public void ApplyDamage(float damageVaue)
    {
        _currentHealth -= damageVaue;
        if (_isObject && _currentHealth <= 0) 
        { 

        }
        if (_isEnemy && _currentHealth <= 0)
        {
            EnemyDead();
        }
        if (_isPlayer && _currentHealth <= 0)
        {
            ObjectDead();
        }
    }
    public void EnemyDead()
    {
        Debug.Log("Противник мертв");
        gameObject.AddComponent<Rigidbody>();
        try
        {
            if (gameObject.transform.childCount > 0)
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    Transform child1 = gameObject.transform.GetChild(i);
                    childComponent.Add(child1);

                    child1.AddComponent<MeshCollider>().convex = true;
                    child1.AddComponent<Rigidbody>().mass = 10;
                    if (child1.transform.childCount > 0)
                    {
                        for (int j = 0; j < child1.transform.childCount; j++)
                        {
                            Transform child2 = child1.transform.GetChild(j);
                            childComponent.Add(child2);

                            child2.AddComponent<MeshCollider>().convex = true;
                            child2.AddComponent<Rigidbody>().mass = 10;
                        }
                    }

                }
            }
        }
        catch
        {

        }
        StartCoroutine(ExampleCoroutine());
    }
    public void ObjectDead()
    {
        Debug.Log("Объект мертв");
        gameObject.AddComponent<Rigidbody>();
        try
        {
            if (gameObject.transform.childCount > 0)
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    Transform child1 = gameObject.transform.GetChild(i);
                    childComponent.Add(child1);

                    child1.AddComponent<MeshCollider>().convex = true;
                    child1.AddComponent<Rigidbody>().mass = 10;
                    if (child1.transform.childCount > 0)
                    {
                        for (int j = 0; j < child1.transform.childCount; j++)
                        {
                            Transform child2 = child1.transform.GetChild(j);
                            childComponent.Add(child2);

                            child2.AddComponent<MeshCollider>().convex = true;
                            child2.AddComponent<Rigidbody>().mass = 10;
                        }
                    }

                }
            }
        }
        catch
        {

        }
        StartCoroutine(ExampleCoroutine());
    }
    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        if (_currentHealth <= 0)
        {
            yield return new WaitForSeconds(5);
            for (int i = 0; i < childComponent.Count; i++)
            {
                childComponent[i].GetComponent<MeshCollider>().isTrigger = true;

            }
            childComponent.Clear();
        }


        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
