using System.Collections.Generic;
using UnityEngine;

public class PoisonDamage : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;

    public static int playerInside = 0;

    private void Start()
    {
        playerInside = 0;
    }

    private void OnParticleTrigger()
    {
        List <ParticleSystem.Particle> enter = new List <ParticleSystem.Particle> ();
        int numEnter = ps.GetTriggerParticles (ParticleSystemTriggerEventType.Enter, enter);
        playerInside = numEnter;
    }
}
