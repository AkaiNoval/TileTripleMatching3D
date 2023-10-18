using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] TileDataSO tileDataSO;
    string tileName;
    [SerializeField] Image tileImage;

    public TileDataSO TileDataSO 
    { 
        get => tileDataSO;
        set 
        { 
            tileDataSO = value;
            InitTile(value);
        } 
    }
    private void InitTile(TileDataSO tileDataSO)
    {
        tileName = tileDataSO.tileName;
        tileImage.sprite = tileDataSO.tileSprite;
        TileManager.Instance.AllActiveTile.Add(this);
    }
    private void OnDisable()
    {
        if (Container.Instance != null && Container.Instance.AssignedTiles.Contains(this))
        {
            Container.Instance.AssignedTiles.Remove(this);
        }

        if (TileManager.Instance != null)
        {
            TileManager.Instance.AllActiveTile.Remove(this);
        }
    }

}
