using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float pullForce;
    [SerializeField] float refreshRate;

    private void OnEnable()
    {
        StartCoroutine(AutoTurnOff());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Tile tile))
        {
            StartCoroutine(PullTile(tile, true));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Tile tile))
        {
            StartCoroutine(PullTile(tile, false));
        }
    }
    IEnumerator PullTile(Tile tile,bool shouldPull)
    {
        if(shouldPull)
        {
            Vector3 forceDir = center.position - tile.transform.position; 
            tile.GetComponent<Rigidbody>().AddForce(forceDir.normalized* pullForce*Time.deltaTime);
            yield return refreshRate;
            StartCoroutine(PullTile(tile, shouldPull));
        }
    }
    IEnumerator AutoTurnOff()
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * 90f * Time.deltaTime);
    }
}
