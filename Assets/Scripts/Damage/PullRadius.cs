using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullRadius : MonoBehaviour
{
    public List<PullRadius> pullables = new List<PullRadius>();
    [SerializeField] private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            pullables.Add(other.GetComponentInChildren<PullRadius>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            pullables.Remove(other.GetComponentInChildren<PullRadius>());
        }
    }

    public void PullObjects()
    {
        foreach (PullRadius p in pullables)
        {
            if (p != null)
            {
                p.PullThisObject(transform.parent.position);
            }
        }
    }

    private void PullThisObject(Vector3 position)
    {
        Vector3 forceDir = position - transform.position;
        
        // Use for dynamic pull based on distance
        //float distance = Vector3.Distance(position, transform.position); 
        //float scaledValue = Mathf.Lerp(6f, 8f, 1f - distance / 5.0f);
        if (forceDir.sqrMagnitude > 1.0f)
        {
            rb.AddForce(forceDir.normalized * 5, ForceMode.Impulse);
        }
    }
}
