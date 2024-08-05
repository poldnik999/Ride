using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class CarMovement : MonoBehaviour
{
    private Rigidbody _rb;
    public float moveSpeed = 10;
    public float debug = 240;
    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MovementLogic();
    }
    private void MovementLogic()
    {
        //_rb.velocity = new Vector3(0, 0, 5);
        Vector3 movement;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //movement = new Vector3(0, 0, debug * moveSpeed);
        movement = new Vector3(0, 0, 1100);
        if (Input.GetKey(KeyCode.W))
            _rb.MovePosition(transform.position + new Vector3(0, 0, Time.deltaTime * moveSpeed/5));
        if (Input.GetKey(KeyCode.S))
            _rb.MovePosition(transform.position - new Vector3(0, 0, Time.deltaTime * moveSpeed / 5));
        //_rb.AddForce(movement);
    }
}
