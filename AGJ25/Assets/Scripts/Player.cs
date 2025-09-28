using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Player : MonoBehaviour
{
    public float forward_speed = 5f;
    public int score = 0;
    public float horizontal_speed = 5f;
    public Rigidbody rb;
    private float horizontalInput;
    bool alive = true;
    public TextMeshProUGUI scoreText;
    

    // Update is called once per frame
    private void Update()
    {
        scoreText.text = "WHEELS: " + score.ToString();
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
