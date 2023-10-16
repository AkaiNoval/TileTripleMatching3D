using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] GameObject unlockImage;
    [SerializeField] bool usable;

    public void UnlockSlot()
    {
        unlockImage.SetActive(false);
        usable = true;
    }
}
