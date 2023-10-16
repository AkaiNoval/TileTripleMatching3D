using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// THIS SCRIPT FOR HANDLING A REALLY BAD CASE THAT IF TILE FELL INTO THE GROUND AND THROW TILE BACK TO ON THE GROUND
/// </summary>
public class Catcher : MonoBehaviour
{
    [SerializeField] Transform throwPostion;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Tile tile)) return;
        tile.transform.position = throwPostion.transform.position;
    }
}
