using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 /* NOTE: If the amountToSpawn is too high,and you get a warning. 
  * You need to consider increase the spawnRadius or turning off the spawnCollisionCheckRadius*/
public class TileSpawnManager : Singleton<TileSpawnManager>
{
    [SerializeField] Tile tileToSpawn;
    [SerializeField] LevelDataSO levelDataSO;
    //TODO: SHOULD CLEAR ALL THE TILE WHEN CHOOSING ANOTHER SCENE
    List<Tile> allActiveTile = new List<Tile>();
    const int MAX_SPAWN_ATTEMPTS = 100;
    const float SPAWN_COLLISION_CHECK_RADIUS = 0.5f;

    public LevelDataSO LevelDataSO 
    { 
        get => levelDataSO;
        set 
        {
            InitTile(value);
            levelDataSO = value; 
        }
    }

    private void InitTile(LevelDataSO levelDataSO)
    {
        int objectsSpawned = 0;
        int amountToSpawn = 0;

        foreach (var tileData in levelDataSO.tileSpawnDatas)
        {
            amountToSpawn += tileData.setAmount * 3;
        }
        Debug.Log(amountToSpawn);

        List<TileDataSO> tilesData = CacheTripleTileData(levelDataSO);
        for (int i = 0; i < amountToSpawn; i++)
        {
            bool foundValidSpawnPoint = TrySpawnTile(levelDataSO, tilesData);
            if (!foundValidSpawnPoint)
            {
                Debug.LogWarning("Max spawn attempts reached without finding a valid spawn point. Consider turning off spawnCollisionCheckRadius.");
                break;
            }
            tilesData.RemoveAt(0);
        }

        if (objectsSpawned == amountToSpawn)
        {
            Debug.Log($"Have spawned {objectsSpawned} of tiles");
        }
    }

    private bool TrySpawnTile(LevelDataSO levelData, List<TileDataSO> tilesData)
    {
        int spawnAttempts = 0;

        float spawnRadiusX = levelData.spawnRadiusX;
        float spawnRadiusY = levelData.spawnRadiusY;
        float spawnRadiusZ = levelData.spawnRadiusZ;

        bool shouldDetectCollisionWhenSpawning = levelData.shouldDetectCollisionWhenSpawning;

        // Check if there are any TileSpawnData left
        if (tilesData.Count == 0) return false;

        while (spawnAttempts < MAX_SPAWN_ATTEMPTS)
        {
            //Vector3 spawnPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            Vector3 spawnPoint = GetRandomSpawnPoint(spawnRadiusX, spawnRadiusY, spawnRadiusZ);
            if (!shouldDetectCollisionWhenSpawning || !Physics.CheckSphere(spawnPoint, SPAWN_COLLISION_CHECK_RADIUS))
            {
                float randomYRotation = RandomTileRotation(0f, 360f);
                float randomXRotation = RandomTileRotation(-20, 20);
                float randomZRotation = RandomTileRotation(-20, 20);
                Quaternion rotation = Quaternion.Euler(randomXRotation, randomYRotation, randomZRotation);
                var spawnedTile = Instantiate(tileToSpawn, spawnPoint, rotation);
                spawnedTile.transform.parent = transform;
                spawnedTile.TryGetComponent(out Tile tile);
                tile.TileDataSO = tilesData[0];
                spawnedTile.name = tilesData[0].tileName;
                return true;
            }
            spawnAttempts++;
        }
        return false;
    }
    List<TileDataSO> CacheTripleTileData(LevelDataSO levelDataSO)
    {
        List<TileDataSO> localTilesData = new List<TileDataSO>();
        foreach (var tileSpawnData in levelDataSO.tileSpawnDatas)
        {
            for (int i = 0; i < tileSpawnData.setAmount * 3; i++)
            {
                localTilesData.Add(tileSpawnData.tileData);
            }
        }
        return localTilesData;
    }
    float RandomTileRotation(float minRotation, float maxRotation) => Random.Range(minRotation, maxRotation);
    Vector3 GetRandomSpawnPoint(float radiusX, float radiusY, float radiusZ)
    {
        float randomX = Random.Range(-radiusX, radiusX);
        float randomY = Random.Range(-radiusY, radiusY);
        float randomZ = Random.Range(-radiusZ, radiusZ);

        Vector3 randomPoint = transform.position + new Vector3(randomX, randomY, randomZ);

        return randomPoint;
    }
    private void OnDrawGizmosSelected()
    {
        if (LevelDataSO == null) return;
        float spawnRadiusX = LevelDataSO.spawnRadiusX;
        float spawnRadiusY = LevelDataSO.spawnRadiusY;
        float spawnRadiusZ = LevelDataSO.spawnRadiusZ;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadiusX * 2, spawnRadiusY * 2, spawnRadiusZ * 2));
    }
}
