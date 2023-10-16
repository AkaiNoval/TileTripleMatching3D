using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollisionController : MonoBehaviour
{
    Rigidbody rb;
    Collider tileCollider;
    private void Awake() 
    { 
        rb = GetComponent<Rigidbody>();
        tileCollider = GetComponent<Collider>();
    }
    private void OnEnable() 
    { 
        rb.isKinematic = false;
        tileCollider.enabled = true;
    }
    private void OnDisable() 
    { 
        rb.isKinematic = true;
        tileCollider.enabled = false;
    }

}
