using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectingButton : MonoBehaviour
{
    [SerializeField] LevelDataSO levelData;
    public void SelectingLevel()
    {
        TileManager.Instance.LevelDataSO = levelData;
        Container.Instance.LevelDataSO = levelData;
        TimeManager.Instance.LevelDataSO = levelData;
    }
}
