using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTest : MonoBehaviour
{
    // Start is called before the first frame update
    public WheelCollider[] wheel_col;
    public Transform[] wheels;
    public float moveSpeed = 40;
    float angle = 30;
    void Update()
    {
        
        for (int i = 0; i < wheel_col.Length; i++)
        {
            wheel_col[i].motorTorque = Input.GetAxis("Vertical") * moveSpeed;
            if (i == 0 || i == 1)
            {
                wheel_col[i].steerAngle = Input.GetAxis("Horizontal") * angle;
            }
            var pos = transform.position;
            var rot = transform.rotation;
            wheel_col[i].GetWorldPose(out pos, out rot);
            wheels[i].position = pos;
            wheels[i].rotation = rot;
            wheels[i].rotation *= Quaternion.Euler(0f, 180f, 0f);


        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var i in wheel_col)
                {
                    i.brakeTorque = 2000;
                }
            }
            else
            {   //reset the brake torque when another key is pressed
                foreach (var i in wheel_col)
                {
                    i.brakeTorque = 0;
                }

            }
        }
    }
}
