using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DigitalRuby.ThunderAndLightning;
using UnityEngine;

public class PullRadius : MonoBehaviour
{
    private List<PullRadius> inRange = new List<PullRadius>();
    [SerializeField] private float drag = 1.7f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MoveableAgent agent;
    [SerializeField] private Damageable damageable;
    [SerializeField] private GameObject explodePrefab;
    [SerializeField] private LightningBoltPrefabScript lightningPrefab;

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
        Instantiate(explodePrefab, transform.position + Vector3.up, transform.rotation);
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
        
        foreach (PullRadius p in inRange.OrderBy(radius => (radius.transform.position - transform.position).sqrMagnitude))
        {
            if (p != null && p.damageable.IsWet())
            {
                yield return new WaitForSeconds(0.25f);
                if (p.damageable.IsWet())  // confirming still wet after waiting
                {
                    LightningBoltPrefabScript bolt = Instantiate(lightningPrefab, transform.position, Quaternion.identity);
                    bolt.LightningEndedCallback += (_, _, _) => Destroy(bolt);
                    bolt.Source.transform.position = transform.position + Vector3.up;
                    bolt.Destination.transform.position = p.transform.position + Vector3.up;
                    
                    p.damageable.TakeDamage(DamageType.Lightning, 0);
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
