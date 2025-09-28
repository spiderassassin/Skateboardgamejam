using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
     Player player;
    public AudioSource audioSource;
    bool has_played = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }



   
    // Update is called once per frame
    void Update()
    {
        if((player.transform.position - this.transform.position).magnitude < 5)
        {
            if(has_played == false)
            {
                audioSource.Play();
                has_played = true;
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            player.Die();
        }
    }
}
