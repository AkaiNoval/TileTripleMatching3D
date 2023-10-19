using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;

public class MoveToSlot : MonoBehaviour
{
    Tile tile;
    TileCollisionController rbController;
    bool isMouseOver;
    bool isMoving;

    Vector3 desiredScale = Vector3.one;

    public bool IsMoving 
    { 
        get => isMoving; 
        private set => isMoving = value; 
    }

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
    private void OnMouseOver() 
    {
        if (MouseOverUIUtil.IsMouseOverUIWithIgnores()) return;
        isMouseOver = true; 
    }
    private void OnMouseExit()
    {
        isMouseOver = false;
    }
    public void AssignToContainer(Container container)
    {
        if (container.AssignedTiles.Count >= container.UsableSlots.Count) return;
        container.AssignedTiles.Add(tile);
        container.SortingAssignedTilesPosition();    
    }
    public void MoveToNewPostion(Transform targetPosition)
    {
        IsMoving = true;
        Sequence sequence = DOTween.Sequence();
        rbController.enabled = false;
        sequence.Join(transform.DORotate(Vector3.zero, 0.3f));
        sequence.Join(transform.DOScale(desiredScale, 0.3f));
        sequence.Join(transform.DOMove(targetPosition.position, 0.5f));
        sequence.OnComplete(() => 
        {
            IsMoving = false;
            AudioSFXManager.PlaySFX(AudioKey.SlotSort);
            Container.Instance.TileTripleMatching();
        });
        sequence.Play();
    }
}
