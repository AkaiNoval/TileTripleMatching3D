using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] GameObject unlockImage;
    [SerializeField] Tile tileOccupied = null;

    public Tile TileOccupied 
    { 
        get => tileOccupied; 
        set => tileOccupied = value; 
    }

    public void UnlockSlot()
    {
        unlockImage.SetActive(false);
        Container.Instance.UsableSlots.Add(this);
    }
}
