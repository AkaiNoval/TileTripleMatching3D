using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DESCRIPTION:
/// NOTE: If the amountToSpawn is too high, you need to consider increase the spawnRadius or decrease the spawnCollisionCheckRadius
/// </summary>
public class TileSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject tileToSpawn;
    [SerializeField] int amountToSpawn;
    [SerializeField] float spawnRadiusX;
    [SerializeField] float spawnRadiusY;
    [Range(0f,10f)]
    [SerializeField] float spawnRadiusZ;
    [SerializeField] bool shouldDetectCollisionWhenSpawning;

    const int maxSpawnAttempts = 100;
    const float spawnCollisionCheckRadius = 1;

    private void Start()
    {
        int objectsSpawned = 0;

        for (int i = 0; i < amountToSpawn; i++)
        {
            bool foundValidSpawnPoint = TrySpawnTile();

            if (!foundValidSpawnPoint)
            {
                Debug.LogWarning("Max spawn attempts reached without finding a valid spawn point. Consider adjusting spawnCollisionCheckRadius or maxSpawnAttempts.");
                break;
            }
        }

        if (objectsSpawned == amountToSpawn)
        {
            Debug.Log($"Have spawned {objectsSpawned} of tiles");
        }
    }

    private bool TrySpawnTile()
    {
        int spawnAttempts = 0;

        while (spawnAttempts < maxSpawnAttempts)
        {
            //Vector3 spawnPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            Vector3 spawnPoint = GetRandomSpawnPoint(spawnRadiusX, spawnRadiusY, spawnRadiusZ);
            if (!shouldDetectCollisionWhenSpawning || !Physics.CheckSphere(spawnPoint, spawnCollisionCheckRadius))
            {
                float randomYRotation = RandomTileRotation(0f, 360f);
                float randomXRotation = RandomTileRotation(-20, 20);
                float randomZRotation = RandomTileRotation(-20, 20);
                Quaternion rotation = Quaternion.Euler(randomXRotation, randomYRotation, randomZRotation);
                var spawnedTile = Instantiate(tileToSpawn, spawnPoint, rotation);
                spawnedTile.transform.parent = transform;
                spawnedTile.name = "Tile";
                return true;
            }

            spawnAttempts++;
        }
        return false;
    }
    float RandomTileRotation(float minRotation, float maxRotation) => Random.Range(minRotation, maxRotation);
    private Vector3 GetRandomSpawnPoint(float radiusX, float radiusY, float radiusZ)
    {
        float randomX = Random.Range(-radiusX, radiusX);
        float randomY = Random.Range(-radiusY, radiusY);
        float randomZ = Random.Range(-radiusZ, radiusZ);

        Vector3 randomPoint = transform.position + new Vector3(randomX, randomY, randomZ);

        return randomPoint;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadiusX * 2, spawnRadiusY * 2, spawnRadiusZ * 2));
    }
}
