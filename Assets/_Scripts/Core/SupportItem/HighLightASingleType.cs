using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightASingleType : MonoBehaviour
{
    public void HightLightOne()
    {
        Container container = Container.Instance;
        TileManager tileManager = TileManager.Instance;
        // Check if there are at least three tiles in AllActiveTile
        if (tileManager.AllActiveTile.Count >= 3)
        {
            // Generate a random index to select a random tile
            int randomIndex = Random.Range(0, tileManager.AllActiveTile.Count);
            Tile randomTile = tileManager.AllActiveTile[randomIndex];

            // Get the TileDataSO from the random tile
            TileDataSO randomTileData = randomTile.TileDataSO;

            // Find all other tiles with the same TileDataSO
            List<Tile> matchingTiles = new List<Tile>();
            foreach (Tile tile in tileManager.AllActiveTile)
            {
                if (tile != randomTile && tile.TileDataSO == randomTileData)
                {
                    matchingTiles.Add(tile);
                }
            }

            // Loop through the list and highlight each one
            foreach (var tile in matchingTiles)
            {
                if (tile.TryGetComponent(out TileOutline tileOutline))
                {
                    tileOutline.HightLightThisManually();
                }
            }
        }
    }
}

