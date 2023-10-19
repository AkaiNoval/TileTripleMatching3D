using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCompleteSingleTile : MonoBehaviour
{
    public void FindingHelper()
    {
        Container container = Container.Instance;
        TileManager tileManager = TileManager.Instance;

        #region ForEmptyList
        if (container.AssignedTiles.Count == 0 && tileManager.AllActiveTile.Count > 2)
        {
            Tile firstTile = tileManager.AllActiveTile[0];
            Debug.Log("I came");
            // Check if the first tile has a TileDataSO
            if (firstTile != null && firstTile.TileDataSO != null)
            {
                TileDataSO firstTileData = firstTile.TileDataSO;

                // Find two other tiles with the same TileDataSO
                List<Tile> matchingTiles = new List<Tile>
                {
                    firstTile
                };
                foreach (Tile tile in tileManager.AllActiveTile)
                {
                    if (tile != firstTile && tile.TileDataSO == firstTileData)
                    {
                        matchingTiles.Add(tile);

                        // If you've found two matching tiles, break the loop
                        if (matchingTiles.Count == 3)
                        {
                            break;
                        }
                    }
                }

                foreach (var tile in matchingTiles)
                {
                    tile.TryGetComponent(out MoveToSlot mover);
                    mover.AssignToContainer(container);
                }
            }
        }
        #endregion

    }
}
