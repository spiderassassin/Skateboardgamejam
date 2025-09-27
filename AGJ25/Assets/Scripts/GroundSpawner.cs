using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;
    Vector3 nextSpawnPoint;
    public float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<10; i++)
        {
            SpawnTile();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
    }

    public void SpawnTile()
    {
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;

    }
}
