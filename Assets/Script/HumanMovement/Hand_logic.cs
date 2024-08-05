using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Hand_logic : MonoBehaviour
{
    private Rigidbody _rb;

    public float hand_speed = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CharacterControl();
    }
    private void CharacterControl()
    {
        //Vector3 transform = new Vector3(_target.transform.position.x, _target.transform.position.y, _target.transform.position.z);
        //Vector3 transform_hand = new Vector3(_rb.transform.forward.x, _rb.transform.forward.y, _rb.transform.forward.z);
        ////Debug.Log("  Transform_hand:  "+transform_hand);
        //Vector3 movement = new Vector3(0f, 0f, 1000f);
        
        if (Input.GetMouseButton(0))
        {
            Debug.Log("LKM");
            _rb.AddTorque(0, -1 * hand_speed, 0);
        }
        if (Input.GetMouseButton(1))
        {
            _rb.AddTorque(0, 1* hand_speed, 0);
        }
    }
}
