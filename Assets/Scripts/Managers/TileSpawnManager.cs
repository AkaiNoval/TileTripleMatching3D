using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DESCRIPTION: Spawnable 300 tiles with 10 radius
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
            Vector3 spawnPoint = transform.position + Random.insideUnitSphere * spawnRadius;

            if (!Physics.CheckSphere(spawnPoint, spawnCollisionCheckRadius))
            {
                float randomYRotation = RandomTileRotation(0f, 360f);
                float randomXRotation = RandomTileRotation(-45, 45);
                float randomZRotation = RandomTileRotation(-45, 45);
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
}
