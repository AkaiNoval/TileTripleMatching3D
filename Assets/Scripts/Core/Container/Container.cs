using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
                Debug.Log("This slot is not active: " + slot.name);
                slot.gameObject.SetActive(true);

                /*Make sure this slot is not in the usableSlots*/
                if (UsableSlots.Contains(slot))
                {
                    UsableSlots.Remove(slot);
                }
            }
        }
    }

    public void SortingAssignedTilesPosition(Tile tile)
    {
        AssignedTiles = AssignedTiles.OrderBy(tile => tile.TileDataSO.name).ToList();
        RearrangeTiles();
    }
    
    void RearrangeTiles()
    {
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

    private void UnlockLockedSlot()
    {
        //When player click on locked slot it will unlock the first locked slot
    }
}
