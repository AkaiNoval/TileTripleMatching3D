using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveToSlot : MonoBehaviour
{
    Tile tile;
    TileRigidbodyController rbController;
    private void Awake()
    {
        tile = GetComponent<Tile>();
        rbController = GetComponent<TileRigidbodyController>();
    }
    private void OnMouseUp()
    {
        if (!rbController.enabled) return;
        GoToTarget(Container.Instance);
    }

    void GoToTarget(Container container)
    {
        Transform targetTransform = null;
        targetTransform = SetTargetSlot(container.UsableSlots);
        if (targetTransform == null) return;
        rbController.enabled = false;
        transform.rotation = Quaternion.identity;
        transform.position = targetTransform.position;
    }
    Transform SetTargetSlot(List<Slot> usableSlots)
    {
        List<Tile> tempTilesData = new();
        foreach (var slot in usableSlots)
        {
            if(slot.TileOccupied != null)
            {
                tempTilesData.Add(slot.TileOccupied);
            }
        }
        if (tempTilesData.Count >= usableSlots.Count) return null;

        foreach (var slot in usableSlots)
        {
            if(slot.TileOccupied == null)
            {
                slot.TileOccupied = tile;
                return slot.transform;
            }
        }       
        return null;
    }
}
