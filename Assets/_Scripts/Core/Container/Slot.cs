using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] GameObject unlockImage;

    public void UnlockSlot()
    {
        unlockImage.SetActive(false);
        Container.Instance.UsableSlots.Add(this);
    }
    public void LockSlot()
    {
        unlockImage.SetActive(true);
        if (!Container.Instance.UsableSlots.Contains(this)) return;
        Container.Instance.UsableSlots.Remove(this);
    }
    public void ButtonUnlockSlot()
    {
        //TODO: Do something about currency here
        //TODO: Animation or use VFX here
        UnlockSlot();
    }
}
