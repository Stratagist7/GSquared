using System;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] private float maxLifeTime = 1.5f;
    private float lifeTime;
    
    private void Start()
    {
        lifeTime = 0f;
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damageable"))
        {
            // TODO: deal damage
        }
        Destroy(gameObject);
    }
}
