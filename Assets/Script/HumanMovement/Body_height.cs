using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Body_height : MonoBehaviour
{
    [SerializeField] private Transform _target;
    // Start is called before the first frame update

    private Rigidbody _rb;

    public float Speed = 10f;
    public float rotate_speed = 6f;
    public float start_y = 0;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        start_y += _target.position.y + transform.position.y;
    }

    private void FixedUpdate()
    {
        //Debug.Log(start_y);


        //_rb.transform.position = new Vector3(_rb.position.x, start_y+1, _rb.position.z);
        //if (_rb.position.y < start_y+0.1)
        //    _rb.AddForce(new Vector3(0, 1, 0) * Speed*3);
        MovementLogic();
        RotateLogic();
    }

    private void MovementLogic()
    {

        Vector3 movement;
        float moveHorizontal = -Input.GetAxis("Horizontal");
        float moveVertical = -Input.GetAxis("Vertical");
        if (_rb.position.y < start_y)
            movement = new Vector3(moveHorizontal, 5f, moveVertical);
        else
            movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //_rb.AddForce(movement * Speed);
        _rb.velocity = movement * Speed;

    }
    private void RotateLogic()
    {
        float rotateHorizontal = -Input.GetAxis("Horizontal") * rotate_speed;
        float rotateVertical = -Input.GetAxis("Vertical") * rotate_speed;

        Vector3 rotate_Horizontal = new Vector3(0f, rotateHorizontal, 0f);
        Vector3 rotate_Vertical = new Vector3(0f, rotateVertical, 0f);

        if (transform.forward.x > 0 && transform.forward.x <= 1)
        {
            //Debug.Log("1# Transform.forward: " + transform.forward + "   " + rotateHorizontal);
            //_rb.AddTorque(rotate_Horizontal);
            gameObject.transform.Rotate(-rotate_Vertical, Time.deltaTime * rotate_speed);
        }
        if (transform.forward.x > -1 && transform.forward.x <= 0)
        {
            //Debug.Log("1# Transform.forward: " + transform.forward + "   " + rotateHorizontal);
            //_rb.AddTorque(-rotate_Horizontal);
            gameObject.transform.Rotate(rotate_Vertical, Time.deltaTime * rotate_speed);
        }
        if (transform.forward.z > 0 && transform.forward.z <= 1)
        {
            //_rb.AddTorque(rotate_Vertical);
            gameObject.transform.Rotate(rotate_Horizontal, Time.deltaTime * rotate_speed);
        }
        if (transform.forward.z > -1 && transform.forward.z <= 0)
        {
            //_rb.AddTorque(-rotate_Vertical);
            gameObject.transform.Rotate(-rotate_Horizontal, Time.deltaTime * rotate_speed);
        }
    }
}
