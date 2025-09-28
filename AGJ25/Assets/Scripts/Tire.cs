using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tire : MonoBehaviour
{
    Player player;
    public float turnSpeed = 90f;
    public AudioSource wheel;
    public GameObject wheelrender;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }
        if (other.transform.tag == "Player")
        {
            wheel.Play();
            transform.GetChild(0).gameObject.SetActive(false);
            //wheelrender.SetActive(false);

            player.score++;
            Destroy(gameObject,2);

        }
    }
}
