using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TilePool : Singleton<TilePool>
{
    public ObjectPool<Tile> tilePool;

    [SerializeField] Tile tileToSpawn;
    [SerializeField] GameObject tilePoolHolder;
    [SerializeField] int defaultCapacity =300;
    [SerializeField] int maxCapacity = 600;
    private void Start()
    {
        tilePool = new ObjectPool<Tile>(CreateTile, OnTakeObjectFromPool, OnReturnObjectFromPool, OnDestroyTile, true, defaultCapacity, maxCapacity);
        CreatTileAtStart();
    }
    private void CreatTileAtStart()
    {
        for (int i = 0; i < defaultCapacity; i++)
        {
            Tile tile = Instantiate(tileToSpawn, tilePoolHolder.transform);
            tile.SetPool(tilePool);
            tile.gameObject.SetActive(false);
            tile.gameObject.name = "Inactivated Tile";
        }
    }
    private Tile CreateTile()
    {
        Tile tile = Instantiate(tileToSpawn, tilePoolHolder.transform);
        Debug.Log("Create new tile if it is needed...");
        tile.SetPool(tilePool);
        return tile;
    }

    /* What to do the with the tile that just got out of the pool */
    private void OnTakeObjectFromPool(Tile tile)
    {
        //Set transform and rotation
        tile.gameObject.SetActive(true);
    }
    /* What to do the with the tile that just got back into the pool */
    private void OnReturnObjectFromPool(Tile tile)
    {
        tile.transform.localScale = tile.OriginalScale;
        tile.gameObject.SetActive(false);
    }
    /* What to do the with the tile that needs to destroy instead of returning to the pool */
    private void OnDestroyTile(Tile tile)
    {
        
    }
}
