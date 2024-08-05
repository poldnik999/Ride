using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Color = UnityEngine.Color;

public class Gun_shoot : MonoBehaviour
{
    public Camera camera;
    public Transform aimTransform;
    public Vector3 AimPoint { get; set; }
    public Vector3 AimNormal { get; set; }
    public string gunName;
    public Transform player_rotate;
    public Transform gunTransform;

    public GameObject Bullet;
    private Quaternion targetRotation;
    

    // Перезарядка и кол-во патронов в обойме
    private int AmmoCount = 7;      // Кол-во патронов
    private float ReloadTime = 1;    // Время перезарядки
    private float RateOfFire = 0.1f; // Скорострельность
    public bool isActiveReload = false;
    private bool isActiveShoot = false;
    public int AmmoLeft;
    private float Timer;
    void Start()
    {
        Debug.Log(GunHolder.DataHolder.Prefab);
        if(GunHolder.DataHolder.Prefab == null)
            GunHolder.DataHolder.Prefab = (GameObject)Resources.Load("Guns/minigun");
        AmmoLeft = AmmoCount;
        GameObject gun = Instantiate(GunHolder.DataHolder.Prefab, gameObject.transform.position, GunHolder.DataHolder.Prefab.transform.rotation);
        Debug.Log(gun);
        AmmoCount = gun.GetComponent<GunStats>().AmmoCount;
        ReloadTime = gun.GetComponent<GunStats>().ReloadTime;
        RateOfFire = gun.GetComponent<GunStats>().RateOfFire;
        gun.transform.SetParent(gameObject.transform);
        //player_rotate = gameObject.transform.GetChild(0) ;
        aimTransform = gameObject.transform.GetChild(0).GetChild(0).FindChild("Spread");
        gunTransform = gameObject.transform.GetChild(0).FindChild("GunPoint");
    }

    // Update is called once per frame
    void Update()
    {
        LookOnCursor();
        //Aiming();
        
    }
    private void FixedUpdate()
    {
        isActiveShoot = false;
        if (Input.GetKey(KeyCode.Mouse0))   isActiveShoot = true;
        if (Input.GetKey(KeyCode.R))        isActiveReload = true;

        if (isActiveReload)
        {
            Timer += Time.deltaTime;
            if (Timer > ReloadTime)
            {
                AmmoLeft = AmmoCount;
                Timer = 0;
                isActiveReload = false;
            }
        }
        else if (isActiveShoot)
        {
            Timer += Time.deltaTime;
            if (Timer > RateOfFire)
            {
                //aimTransform.position = new Vector3(0f, 0f, 0f);
                Shoot();
                
                Timer = 0;
            }
        }
    }

    void Shoot()
    {
        //aimTransform.position = new Vector3(0f, 0f, 0f);
        Debug.Log("Aim spread:  " + aimTransform.position);
        Debug.Log("Player rotate:  " + player_rotate.position);
        
        if (AmmoLeft > 0)
        {
            CarTest carTest = new CarTest();
            Vector3 spread = aimTransform.parent.GetComponent<BoxCollider>().size;
            Debug.Log(spread);
            if (Random.Range(0f, 1f) >= 0.5f)
                aimTransform.localPosition = new Vector3(Random.Range(0, spread.x/2), 0f, 0f);
            else
                aimTransform.localPosition = new Vector3(-Random.Range(0, spread.x/2), 0f, 0f);
            //Debug.Log(aimTransform.localPosition);
            GameObject bullet = Instantiate(Bullet, gunTransform.position, gunTransform.rotation);
            bullet.transform.rotation = targetRotation;
            //Vector3 vct = aimTransform.position - player_rotate.position;
            Vector3 vct = aimTransform.position - gunTransform.position;
            bullet.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(vct * (40f + carTest.moveSpeed * 6), aimTransform.position);
            
            Debug.DrawRay(vct, aimTransform.localPosition, Color.red, 3f);
            Destroy(bullet, 4);
            AmmoLeft--;

            
        }
    }
    void LookOnCursor()
    {       //заставляет персонажа следить за курсором мышки 		
        Plane playerPlane = new Plane(Vector3.up, player_rotate.position);
        Ray ray = camera.ScreenPointToRay (Input.mousePosition);
        float hitdist = 0;
        if (playerPlane.Raycast (ray, out hitdist)) 
        {
            Vector3 targetPoint = ray.GetPoint (hitdist);
            targetRotation = Quaternion.LookRotation (targetPoint - player_rotate.position);
            player_rotate.rotation = Quaternion.Normalize(targetRotation);
            //player_rotate.rotation *= Quaternion.Euler(90f, 0f, 0f);
            gunTransform.rotation = Quaternion.Normalize(targetRotation);
            //Player_rotate.rotation = Quaternion.Slerp (Player_rotate.rotation, targetRotation, 100 * Time.deltaTime); 
        } 	
    }
}
