using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class MoveToSlot : MonoBehaviour
{
    Tile tile;
    TileCollisionController rbController;
    bool isMouseOver;
    private void Awake()
    {
        tile = GetComponent<Tile>();
        rbController = GetComponent<TileCollisionController>();
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && isMouseOver)
        {
            if (!rbController.enabled) return;
            GoToTarget(Container.Instance);
        }
    }
    private void OnMouseOver() => isMouseOver = true;
    private void OnMouseExit() => isMouseOver = false;

    void GoToTarget(Container container)
    {
        Transform targetTransform = null;
        targetTransform = SetTargetSlot(container.UsableSlots);
        if (targetTransform == null) return;
        /* Turn on Kinemetic for the tile */
        rbController.enabled = false;
        transform.DORotate(Vector3.zero, 0.3f);
        transform.DOScale(transform.localScale * 0.7f, 0.3f);
        transform.DOMove(targetTransform.position, 0.5f);
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
