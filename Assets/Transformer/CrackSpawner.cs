using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackSpawner : MonoBehaviour
{
    int maxCracks = 10;
    public GameObject crackPrefab;
    Queue<GameObject> crackQueue = new Queue<GameObject>();
    
    float lastSpawned = 0;
    float spawnDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < maxCracks; i++)
        {
            GameObject crack = GameObject.Instantiate(crackPrefab);
            crack.transform.position = new Vector3(0,-10,0);
            crack.SetActive(false);
            crackQueue.Enqueue(crack);            
        }
    }


    private void OnTriggerEnter(Collider other)
    {        
        if (Time.time > lastSpawned + spawnDelay)
        {
            SpawnCrack();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if(Time.time > lastSpawned + spawnDelay)
        {
            SpawnCrack();
        }
    }



    private void SpawnCrack()
    {
        GameObject crack;
        crack = crackQueue.Dequeue();
        crack.transform.position = transform.position;
        crack.SetActive(true);
        crack.transform.rotation = Quaternion.Euler(0, Random.value*360, 0);
        crackQueue.Enqueue(crack);
    }
}
