using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawnController : MonoBehaviour
{
    public GameObject heartPrefab;
    public GameObject player;
    public float spawnDistance = 15f;
    public float spawnInterval = 5f;

    private int maxHearts = 3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnHeart());
    }

    IEnumerator SpawnHeart()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            GameObject[] heartsInScene = GameObject.FindGameObjectsWithTag("Life");

            if (heartsInScene.Length >= maxHearts)
            {
                continue;
            }

            Vector3 randomDirection = Random.onUnitSphere;
            randomDirection.z = 0;

            Vector3 spawnPosition = player.transform.position + randomDirection.normalized * spawnDistance;

            Instantiate(heartPrefab, spawnPosition, Quaternion.identity);
        }
    }
}