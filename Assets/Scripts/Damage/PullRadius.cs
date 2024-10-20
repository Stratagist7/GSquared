using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PullRadius : MonoBehaviour
{
    private List<PullRadius> inRange = new List<PullRadius>();
    [SerializeField] private float drag = 1.7f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MoveableAgent agent;
    [SerializeField] private Damageable damageable;

    private void Start()
    {
        if (agent != null)
        {
            rb.drag = drag;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            inRange.Add(other.GetComponentInChildren<PullRadius>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            inRange.Remove(other.GetComponentInChildren<PullRadius>());
        }
    }

    public void Explode()
    {
        damageable.TakeDamage(DamageType.None, ReactionValues.EXP_DMG);
        foreach (PullRadius p in inRange)
        {
            if (p != null)
            {
                p.damageable.TakeDamage(DamageType.None, ReactionValues.EXP_DMG);
            }
        }
    }

    public IEnumerator ChainReaction()
    {
        damageable.TakeDamage(DamageType.None, ReactionValues.CHAIN_DMG);
        yield return null;
        foreach (PullRadius p in inRange)
        {
            if (p != null && p.damageable.IsWet())
            {
                yield return new WaitForSeconds(0.25f);
                if (p.damageable.IsWet())  // confirming still wet after waiting
                {
                    p.damageable.TakeDamage(DamageType.Lightning, ReactionValues.CHAIN_DMG);
                }
            }
        }
    }

    public void Swirl(DamageType argType)
    {
        foreach (PullRadius p in inRange)
        {
            if (p != null)
            {
                p.damageable.TakeDamage(argType, 0);  // only apply the element
            }
        }
    }

    public void PullObjects()
    {
        foreach (PullRadius p in inRange)
        {
            if (p != null)
            {
                p.PullThisObject(transform.parent.position);
            }
        }
    }

    private void PullThisObject(Vector3 position)
    {
        if (agent != null)
        {
            agent.Stun();
        }
        
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
