using System.Collections.Generic;
using UnityEngine;

public class PoisonGoopSFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private EffectSfxHandler effectSfx;
    private bool shouldPlaySfx = true;
    private bool particlePlaying = false;
    private List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void Update()
    {
        if (shouldPlaySfx == false && particlePlaying != ps.isPlaying)
        {
            shouldPlaySfx = ps.isPlaying;
            particlePlaying = ps.isPlaying;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
        if (shouldPlaySfx && numCollisionEvents > 0)
        {
            shouldPlaySfx = false;
            Instantiate(effectSfx, collisionEvents[0].intersection, Quaternion.identity);
        }
    }
}
