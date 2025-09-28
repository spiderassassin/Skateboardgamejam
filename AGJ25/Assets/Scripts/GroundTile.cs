using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    public GameObject dogPrefab;
    public GameObject catPrefab;
    public GameObject bananaPrefab;
    public GameObject canPrefab;
    public GameObject carBeepPrefab;
    public GameObject carHonkPrefab;
    public GameObject ratPrefab;
    public GameObject snakePrefab;
    public List<GameObject> prefabs = new List<GameObject>();
    public GameObject coinPrefab;
    // Start is called before the first frame update
    void Start()
    {
        prefabs.Add(dogPrefab);
        prefabs.Add(catPrefab);
        prefabs.Add(bananaPrefab);
        prefabs.Add(canPrefab);
        prefabs.Add(carBeepPrefab);
        prefabs.Add(carHonkPrefab);
        prefabs.Add(ratPrefab);
        prefabs.Add(snakePrefab);

        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        if (groundSpawner.time > 1)
        {
            SpawnObstacle();
        }
        SpawnCoins();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject,2);
    }

    void SpawnObstacle()
    {
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        int obstacleIndex = Random.Range(0, 8);
        Instantiate(prefabs[obstacleIndex], spawnPoint.position, Quaternion.identity, transform);

    }

    void SpawnCoins()
    {
        GameObject temp = Instantiate(coinPrefab, transform);
        temp.transform.position = GetRandomPointinCollider(GetComponent<Collider>());
    }

    Vector3 GetRandomPointinCollider(Collider collider)
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );
        point.y = 1;
        return point;
    }
}
