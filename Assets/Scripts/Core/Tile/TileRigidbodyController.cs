using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRigidbodyController : MonoBehaviour
{
    Rigidbody rb;
    private void Awake() => rb = GetComponent<Rigidbody>();
    private void OnEnable() => rb.isKinematic = false;
    private void OnDisable() => rb.isKinematic = true;

}
