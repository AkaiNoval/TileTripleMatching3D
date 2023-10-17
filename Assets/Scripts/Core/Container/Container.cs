using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Container : Singleton<Container>
{
    [Range(3,8)]
    [SerializeField] int startingSlot;
    [SerializeField] List<Slot> allSlots;
    [SerializeField] List<Slot> usableSlots = new List<Slot>();
    [SerializeField] List<Tile> assignedTiles = new List<Tile>();
    [SerializeField] bool canUnlockNewSlot;

    public List<Slot> UsableSlots 
    { 
        get => usableSlots; 
        private set => usableSlots = value; 
    }
    public List<Tile> AssignedTiles 
    { 
        get => assignedTiles;
        set 
        {
            assignedTiles = value;
        }
    }

    private void Start()
    {
        InitStartingSlots();
        InitLockedSlots();
    }
    private void InitStartingSlots()
    {
        if(allSlots.Count == 0)
        {
            Debug.LogWarning("Did you forget to assign slots into the container?");
            return;
        }
        foreach (Slot slot in allSlots)
        {
            slot.gameObject.SetActive(false);
        }
        for (int i = 0; i < startingSlot; i++)
        {
            allSlots[i].gameObject.SetActive(true);
            allSlots[i].UnlockSlot();
        }
    }
    private void InitLockedSlots()
    {
        if (!canUnlockNewSlot) return;
        foreach (Slot slot in allSlots)
        {
            if (!slot.gameObject.activeSelf)
            {
                slot.gameObject.SetActive(true);

                /*Make sure this slot is not in the usableSlots*/
                if (UsableSlots.Contains(slot))
                {
                    UsableSlots.Remove(slot);
                }
            }
        }
    }

    public void SortingAssignedTilesPosition()
    {
        AssignedTiles = AssignedTiles.OrderBy(tile => tile.TileDataSO.name).ToList();
        if (assignedTiles.Count > usableSlots.Count)
        {
            Debug.LogError("Tile list and slot list have different sizes.");
            return;
        }
        for (int i = 0; i < assignedTiles.Count; i++)
        {
            Tile tile = assignedTiles[i];
            Transform targetTransform = usableSlots[i].transform;

            tile.TryGetComponent(out MoveToSlot mover);
            mover.MoveToNewPostion(targetTransform);
        }
    }

    public void TileTripleMatching()
    {
        List<TileDataSO> assignedTileDataToRemove = new List<TileDataSO>();

        /* Count the occurrences of each unique TileDataSO in the list */
        Dictionary<TileDataSO, int> tileDataCount = new Dictionary<TileDataSO, int>();

        foreach (var tile in assignedTiles)
        {
            if(tile.TryGetComponent(out MoveToSlot mover) && mover.IsMoving)
            {
                continue;
            }
            TileDataSO tileData = tile.TileDataSO;
            if (tileDataCount.ContainsKey(tileData))
            {
                tileDataCount[tileData]++;
            }
            else
            {
                tileDataCount[tileData] = 1;
            }
        }

        /* Identify TileDataSO that appears three or more times */
        foreach (var kvp in tileDataCount)
        {
            if (kvp.Value >= 3)
            {
                assignedTileDataToRemove.Add(kvp.Key);
            }
        }

        /* Remove identified TileDataSO from the list */
        List<Tile> tilesToRemove = new List<Tile>();
        foreach (var tile in assignedTiles)
        {
            if (assignedTileDataToRemove.Contains(tile.TileDataSO))
            {
                MoveToSlot mover;
                if (tile.TryGetComponent(out mover))
                {
                    if (mover.IsMoving)
                    {
                        Debug.Log($"Do not remove the {tile.name} if it's moving");
                        continue;
                    }
                    tilesToRemove.Add(tile);
                }
                
            }
        }

        /* Disable the associated GameObjects of the removed tiles */
        foreach (var tileToRemove in tilesToRemove)
        {
            tileToRemove.gameObject.SetActive(false);
            assignedTiles.Remove(tileToRemove);
        }
        /* Move again if there is a match */
        if (tilesToRemove.Count == 3)
        {
            SortingAssignedTilesPosition();
        }
    }

    private void UnlockLockedSlot()
    {
        //When player click on locked slot it will unlock the first locked slot
    }
}
