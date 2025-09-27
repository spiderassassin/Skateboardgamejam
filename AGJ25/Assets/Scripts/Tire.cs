using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tire : MonoBehaviour
{
    Player player;
    public float turnSpeed = 90f;

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
            player.score++;
            Destroy(gameObject);

        }
    }
}
