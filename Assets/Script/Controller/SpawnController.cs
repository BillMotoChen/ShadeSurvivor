using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public Transform targetObject;
    public float spawnDistance = 20.0f;
    public float moveSpeed = 5.0f;

    public float initialSpawnInterval = 2.0f;
    public float minSpawnInterval = 0.5f;
    public float intervalReductionAmount = 0.1f;
    public float intervalReductionTime = 3.0f;

    private float currentSpawnInterval;

    private GameStatus gameStatus; 

    void Start()
    {
        gameStatus = GameObject.Find("Canvas/GameStatus").GetComponent<GameStatus>();

        currentSpawnInterval = initialSpawnInterval;

        StartCoroutine(SpawnWithChangingInterval());

    }

    IEnumerator SpawnWithChangingInterval()
    {
        while (true)
        {
            SpawnObject();

            yield return new WaitForSeconds(currentSpawnInterval);

            StartCoroutine(ReduceSpawnInterval());
        }
    }

    IEnumerator ReduceSpawnInterval()
    {
        while (currentSpawnInterval > minSpawnInterval)
        {
            yield return new WaitForSeconds(intervalReductionTime);
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - intervalReductionAmount);
        }
    }

    void SpawnObject()
    {
        Vector3 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = targetObject.position + new Vector3(randomDirection.x, randomDirection.y, 0) * spawnDistance;

        GameObject randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
        GameObject spawnedObject = Instantiate(randomObject, spawnPosition, Quaternion.identity);

        Vector3 directionToTarget = (new Vector3(targetObject.position.x, targetObject.position.y, 0) - spawnedObject.transform.position).normalized;
        StartCoroutine(MoveTowardsTarget(spawnedObject, directionToTarget));
    }

    

    IEnumerator MoveTowardsTarget(GameObject obj, Vector3 direction)
    {
        while (obj != null)
        {
            float actualSpeed;

            if (gameStatus.totalTime <= 100f)
            {
                actualSpeed = moveSpeed;
            }
            else
            {
                float multiplyer = gameStatus.totalTime / 100f;
                actualSpeed = moveSpeed * multiplyer;
            }

            obj.transform.position += direction * actualSpeed * Time.deltaTime;

            yield return null;
        }
    }
}
