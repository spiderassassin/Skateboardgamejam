using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public float forward_speed = 5f;
    public float horizontal_speed = 5f;
    public Rigidbody rb;
    private float horizontalInput;
    bool alive = true;
    

    // Update is called once per frame
    private void Update()
    {
        
        horizontalInput = Input.GetAxis("Horizontal");
        if(transform.position.y < -5)
        {
            Die();
        }
    }
    void FixedUpdate()
    {
        if(alive == true)
        {
            Vector3 forwardMove = transform.forward * forward_speed * Time.fixedDeltaTime;
            Vector3 horizontalMove = transform.right * horizontalInput * horizontal_speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + forwardMove + horizontalMove);
        }
        
        
    }

    public void Die()
    {
        alive = false;
        //Load the game over scene
        SceneManager.LoadScene(2);
    }
}
