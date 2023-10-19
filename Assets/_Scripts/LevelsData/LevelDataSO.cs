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
    [Header("Containers Info")]
    [Tooltip("How many starting slots do players have?")]
    [Range(3, 8)]
    public int startingSlot;
    [Tooltip("Can player unlock new slots?")]
    public bool canUnlockNewSlot;
    [Header("Timer")]
    public float stateMaxTime;
    public float plusTime;
}

[Serializable]
public class TileSpawnData
{
    public TileDataSO tileData;
    [Range(1,10)]
    public int setAmount;
}
