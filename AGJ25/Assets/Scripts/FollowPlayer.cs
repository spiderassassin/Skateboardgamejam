using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = transform.position = player.position + offset;
        targetPos.x = 0;
        transform.position = targetPos;

    }
}
