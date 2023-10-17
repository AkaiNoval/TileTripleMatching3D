using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MoveToSlot : MonoBehaviour
{
    Tile tile;
    TileCollisionController rbController;
    bool isMouseOver;
    Vector3 desiredScale = Vector3.one;
    private void Awake()
    {
        tile = GetComponent<Tile>();
        rbController = GetComponent<TileCollisionController>();
        desiredScale = transform.localScale*0.7f;
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && isMouseOver)
        {
            if (!rbController.enabled) return;
            AssignToContainer(Container.Instance);
        }
    }
    private void OnMouseOver() => isMouseOver = true;
    private void OnMouseExit() => isMouseOver = false;
    void AssignToContainer(Container container)
    {
        if (container.AssignedTiles.Count >= container.UsableSlots.Count) return;
        container.AssignedTiles.Add(tile);
        container.SortingAssignedTilesPosition();    
    }
    public void MoveToNewPostion(Transform targetPosition)
    {
        Sequence sequence = DOTween.Sequence();
        rbController.enabled = false;
        sequence.Join(transform.DORotate(Vector3.zero, 0.3f));
        sequence.Join(transform.DOScale(desiredScale, 0.3f));
        sequence.Join(transform.DOMove(targetPosition.position, 0.5f));
        sequence.Play();
    }

    //void GoToTarget(Container container)
    //{
    //    Transform targetTransform = null;
    //    targetTransform = SetTargetSlot(container.UsableSlots);
    //    if (targetTransform == null) return;
    //    /* Turn on Kinemetic for the tile */
    //    rbController.enabled = false;
    //    transform.DORotate(Vector3.zero, 0.3f);
    //    transform.DOScale(transform.localScale * 0.7f, 0.3f);
    //    transform.DOMove(targetTransform.position, 0.5f);
    //}
    //Transform SetTargetSlot(List<Slot> usableSlots)
    //{
    //    List<Tile> tempTilesData = new();
    //    foreach (var slot in usableSlots)
    //    {
    //        if (slot.TileOccupied != null)
    //        {
    //            tempTilesData.Add(slot.TileOccupied);
    //        }
    //    }
    //    if (tempTilesData.Count >= usableSlots.Count) return null;

    //    foreach (var slot in usableSlots)
    //    {
    //        if (slot.TileOccupied == null)
    //        {
    //            slot.TileOccupied = tile;
    //            return slot.transform;
    //        }
    //    }
    //    return null;
    //}



}
