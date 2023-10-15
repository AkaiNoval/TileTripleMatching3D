using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelDataSO : ScriptableObject
{
    public List<TileSpawnData> tileSpawnDatas;
    [Range(5f, 7f)]
    public float spawnRadiusX;
    [Range(5f, 7f)]
    public float spawnRadiusY;
    [Range(6f, 10f)]
    public float spawnRadiusZ;

    public bool shouldDetectCollisionWhenSpawning;
}

[Serializable]
public class TileSpawnData
{
    public TileDataSO tileData;
    public int setAmount;
}
