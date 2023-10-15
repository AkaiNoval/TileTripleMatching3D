using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelDataSO : ScriptableObject
{
    public List<TileSpawnData> tileSpawnDatas;
    [Header("Spawning Method")]
    [Range(5f, 7f)]
    public float spawnRadiusX;
    [Range(5f, 7f)]
    public float spawnRadiusY;
    [Range(6f, 10f)]
    public float spawnRadiusZ;
    [Tooltip("For checking free space when spawning, if there is a warning, please turning this off")]
    public bool shouldDetectCollisionWhenSpawning;
}

[Serializable]
public class TileSpawnData
{
    public TileDataSO tileData;
    public int setAmount;
}
