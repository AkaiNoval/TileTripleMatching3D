using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 /* NOTE: If the amountToSpawn is too high,and you get a warning. 
  * You need to consider increase the spawnRadius or turning off the spawnCollisionCheckRadius*/
public class TileManager : Singleton<TileManager>
{
    #region Fields
    [SerializeField] LevelDataSO levelDataSO;
    [SerializeField] List<LevelDataSO> allLevelDataSO;

    List<Tile> allActiveTile = new List<Tile>();
    TilePool tilePool;

    const int MAX_SPAWN_ATTEMPTS = 100;
    const float SPAWN_COLLISION_CHECK_RADIUS = 0.5f; 
    #endregion

    #region Properties
    public LevelDataSO LevelDataSO
    {
        get => levelDataSO;
        set
        {
            InitTile(value);
            levelDataSO = value;
        }
    }

    public List<Tile> AllActiveTile
    {
        get => allActiveTile;
        set => allActiveTile = value;
    }
    #endregion

    #region WinningEvent
    private void OnEnable()
    {
        Container.OnTileMatching += CheckForWinCondition;
    }
    private void OnDisable()
    {
        Container.OnTileMatching -= CheckForWinCondition;
    }
    private void CheckForWinCondition()
    {
        if (allActiveTile.Count != 0) return;
        GameManager.Instance.UpdateGameState(GameState.Victory);
    }
    #endregion

    #region InitilizeTiles
    private new void Awake()
    {
        tilePool = GetComponent<TilePool>();
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
                //var spawnedTile = Instantiate(tileToSpawn, spawnPoint, rotation);
                var spawnedTile = tilePool.tilePool.Get();
                spawnedTile.transform.position = spawnPoint;
                spawnedTile.transform.rotation = rotation;
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
    #endregion

    #region UI
    public void RestartLevel()
    {
        ClearTile();
        LevelDataSO = levelDataSO;
    } 

    public void ClearTile()
    {
        // Create a new list to store the tiles to deactivate
        List<Tile> tilesToDeactivate = new List<Tile>();

        // Iterate over allActiveTile and add tiles to the new list
        foreach (var tile in allActiveTile)
        {
            tilesToDeactivate.Add(tile);
        }

        // Deactivate the tiles from the new list
        foreach (var tile in tilesToDeactivate)
        {
            tile.gameObject.SetActive(false);
        }

        // Optionally, you can clear the allActiveTile list
        allActiveTile.Clear();
    }

    public void GoToNextLeveButton()
    {
          // Check if the current level data is in the list
        if (allLevelDataSO.Contains(levelDataSO))
        {
            GameManager.Instance.UpdateGameState(GameState.Playing);
            int currentIndex = allLevelDataSO.IndexOf(levelDataSO);
            /* Wrap around if at the end of the list*/
            int nextIndex = (currentIndex + 1) % allLevelDataSO.Count; 
            LevelDataSO = allLevelDataSO[nextIndex];
            Debug.Log(allLevelDataSO[nextIndex].name);
        }
        else
        {
            Debug.LogWarning("Current LevelDataSO is not in the AlllevelDataSO list.");
        }
    }
    #endregion

    #region DebugSpawningZone
    private void OnDrawGizmosSelected()
    {
        if (LevelDataSO == null) return;
        float spawnRadiusX = LevelDataSO.spawnRadiusX;
        float spawnRadiusY = LevelDataSO.spawnRadiusY;
        float spawnRadiusZ = LevelDataSO.spawnRadiusZ;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadiusX * 2, spawnRadiusY * 2, spawnRadiusZ * 2));
    } 
    #endregion

}
