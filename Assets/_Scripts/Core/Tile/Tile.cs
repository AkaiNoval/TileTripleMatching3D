using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;


public class Tile : MonoBehaviour
{
    [SerializeField] TileDataSO tileDataSO;
    string tileName;
    [SerializeField] Image tileImage;
    [SerializeField] TileCollisionController tileCollisionController;
    private ObjectPool<Tile> pool;
    public Vector3 OriginalScale { get; set; }
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

    private void Awake()
    {
        OriginalScale = transform.localScale;
    }
    private void OnEnable()
    {
        tileCollisionController.enabled = true;
    }
    private void OnDisable()
    {
        TilePool.Instance.tilePool.Release(this);
        if (Container.Instance != null && Container.Instance.AssignedTiles.Contains(this))
        {
            Container.Instance.AssignedTiles.Remove(this);
        }

        if (TileManager.Instance != null)
        {
            TileManager.Instance.AllActiveTile.Remove(this);
        }
    }
    public void SetPool(ObjectPool<Tile> _pool)
    {
        pool = _pool;
    }
}
