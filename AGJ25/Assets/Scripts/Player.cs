using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float forward_speed = 5f;
    public float horizontal_speed = 5f;
    public Rigidbody rb;
    private float horizontalInput;

    // Update is called once per frame
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }
    void FixedUpdate()
    {
        Vector3 forwardMove = transform.forward * forward_speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * horizontal_speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
        
    }
}
