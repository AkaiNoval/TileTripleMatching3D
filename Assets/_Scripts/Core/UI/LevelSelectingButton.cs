using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectingButton : MonoBehaviour
{
    [SerializeField] LevelDataSO levelData;
    public void SelectingLevel()
    {
        TileSpawnManager.Instance.LevelDataSO = levelData;
    }
}
