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
        /* Control the the kinemetic of the tile for not letting it falls through the ground */
        rb.isKinematic = true;
        /* Make sure its not gonna collide with other tile or any bounder*/
        tileCollider.enabled = false;
    }

}
