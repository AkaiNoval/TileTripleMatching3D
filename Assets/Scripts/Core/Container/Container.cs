using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [Range(3,8)]
    [SerializeField] int startingSlot;
    [SerializeField] List<Slot> allSlots;
    [SerializeField] List<Slot> usableSlots = new List<Slot>();
    [SerializeField] bool canUnlockNewslot;

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
            usableSlots.Add(allSlots[i]);
        }
    }
    private void InitLockedSlots()
    {
        if (!canUnlockNewslot) return;
        foreach (Slot slot in allSlots)
        {
            if (!slot.gameObject.activeSelf)
            {
                slot.gameObject.SetActive(true);

                /*Make sure this slot is not in the usableSlots*/
                if (usableSlots.Contains(slot))
                {
                    usableSlots.Remove(slot);
                }
            }
        }
    }

    private void UnlockLockedSlot()
    {
        //When player click on locked slot it will unlock the first locked slot
    }
}
