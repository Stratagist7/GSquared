using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            other.GetComponentInChildren<Hitable>().TakeDamage(DamageType.None, MeleeCombat.MELEE_DAMAGE);
            StartCoroutine(HitLag());
        }
    }

    private IEnumerator HitLag()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.01f);
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
