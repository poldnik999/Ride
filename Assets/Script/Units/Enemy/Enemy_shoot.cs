using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

public class Enemy_shoot : MonoBehaviour
{
    //public Camera camera;
    public Transform player_rotate;
    private GameObject playerPosition;
    public GameObject Bullet;

    private Quaternion targetRotation;
    private Transform aimTransform;
    private Transform gunTransform;
    // Перезарядка и кол-во патронов в обойме
    private int AmmoCount = 9999;      // Кол-во патронов
    private float ReloadTime = 1;    // Время перезарядки
    private float RateOfFire = 0.1f; // Скорострельность
    public bool isActiveReload = false;

    [SerializeField]
    private bool isActiveShoot = false;
    public int AmmoLeft;
    private float Timer;
    [SerializeField]
    private float distancePlayer;
    void Start()
    {
        AmmoLeft = AmmoCount;
        GameObject gun = Instantiate((GameObject)Resources.Load("Guns/1911"), gameObject.transform.position, gameObject.transform.rotation);
        ReloadTime = gun.GetComponent<GunStats>().ReloadTime;
        RateOfFire = 1f;
        gun.transform.SetParent(gameObject.transform);
        playerPosition = GameObject.Find("Car");
        //player_rotate = gameObject.transform.GetChild(0) ;
        aimTransform = gameObject.transform.GetChild(0).GetChild(0).Find("Spread");
        gunTransform = gameObject.transform.GetChild(0).Find("GunPoint");
    }

    // Update is called once per frame
    void Update()
    {
        var heading = playerPosition.transform.position - gameObject.transform.position;
        var distance = heading.magnitude;
        distancePlayer = distance;
        LookOnPlayer();
        //Aiming();

    }
    private void FixedUpdate()
    {
        isActiveShoot = false;
        if (distancePlayer < 2) 
            isActiveShoot = true;
        else 
            isActiveShoot = false;

        if (isActiveShoot)
        {
            Timer += Time.deltaTime;
            if (Timer > RateOfFire)
            {
                Shoot();
                Timer = 0;
            }
        }
    }

    void Shoot()
    {

        Debug.Log("Enemy shooting!!");
        Vector3 spread = aimTransform.parent.GetComponent<BoxCollider>().size;
        Debug.Log(spread);
        if (Random.Range(0f, 1f) >= 0.5f)
            aimTransform.localPosition = new Vector3(Random.Range(0, spread.x / 2), 0f, 0f);
        else
            aimTransform.localPosition = new Vector3(-Random.Range(0, spread.x / 2), 0f, 0f);
        //Debug.Log(aimTransform.localPosition);
        GameObject bullet = Instantiate(Bullet, gunTransform.position, gunTransform.rotation);
        bullet.transform.rotation = targetRotation;
        //Vector3 vct = aimTransform.position - player_rotate.position;
        Vector3 vct = aimTransform.position - gunTransform.position;
        bullet.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(vct * 30f, aimTransform.position);

        //Debug.DrawRay(vct, aimTransform.localPosition, Color.red, 3f);
        Destroy(bullet, 8);
    }
    void LookOnPlayer()
    {       //заставляет персонажа следить за курсором мышки 		
        Plane playerPlane = new Plane(Vector3.up, player_rotate.position);
        float hitdist = 0;
        Vector3 targetPoint = playerPosition.transform.position;
        targetRotation = Quaternion.LookRotation(targetPoint - player_rotate.position);
        player_rotate.rotation = Quaternion.Normalize(targetRotation);
        //player_rotate.rotation *= Quaternion.Euler(90f, 0f, 0f);
        //gunTransform.rotation = Quaternion.Normalize(targetRotation);
    }
}
