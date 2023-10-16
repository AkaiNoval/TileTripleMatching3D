using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] TileDataSO tileDataSO;
    [SerializeField] string tileName;
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
        tileName = tileDataSO.TileName;
        tileImage.sprite = tileDataSO.TileSprite;
    }
    void OnMouseEnter()
    {
        // Code to execute when the mouse enters the object
    }
    void OnMouseExit()
    {
        // Code to execute when the mouse enters the object
    }
}
