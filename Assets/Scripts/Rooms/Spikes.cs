using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Damageable.Player.GetComponent<PlayerHealth>().Damage(1000);
        }
    }
}
