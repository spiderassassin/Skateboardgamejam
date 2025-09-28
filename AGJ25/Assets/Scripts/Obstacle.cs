using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Player player;
    public bool is_car;
    public AudioSource audioSource;
    public AudioSource audioSource2;
    bool has_played = false;
    public GameObject flavorText;
    
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.FindObjectOfType<Player>();
    }




    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - this.transform.position).magnitude < 5)
        {
            if (has_played == false)
            {
                if (is_car == true)
                {
                    int index = Random.Range(0, 2);

                    if (index == 0)
                    {
                        audioSource.Play();
                        has_played = true;

                    }
                    else
                    {
                        audioSource2.Play();
                        has_played = true;
                    }
                }
                else
                {
                    audioSource.Play();
                    has_played = true;
                    flavorText.SetActive(true);
                }


            }
        }
    }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Player")
            {
                player.Die();
            }
        }
    }


