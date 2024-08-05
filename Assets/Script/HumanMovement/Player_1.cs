using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_1 : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public float moveSpeed = 2;
    public float sideSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        

    }
    public float Speed = 10f;
    public float JumpForce = 300f;

    //что бы эта переменна€ работала добавьте тэг "Ground" на вашу поверхность земли
    private bool _isGrounded;
    private Rigidbody _rb;

    // обратите внимание что все действи€ с физикой 
    // необходимо обрабатывать в FixedUpdate, а не в Update
    void FixedUpdate()
    {
        MovementLogic();
        JumpLogic();
    }

    private void MovementLogic()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        _rb.AddForce(movement * Speed);
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * JumpForce);

                // ќбратите внимание что € делаю на основе Vector3.up 
                // а не на основе transform.up. ≈сли персонаж упал или 
                // если персонаж -- шар, то его личный "верх" может 
                // любое направление. ¬лево, вправо, вниз...
                // Ќо нам нужен скачек только в абсолютный вверх, 
                // потому и Vector3.up
            }
        }
    }
}
