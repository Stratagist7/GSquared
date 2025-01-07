using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private MultiSfxHandler stepSounds;
    [SerializeField] private EffectSfxHandler deathSound;
    [SerializeField] private MultiSfxHandler attackSounds;

    public void Die()
    {
        Destroy(parent);
    }
    public void DeathSound()
    {
        deathSound.PlayAudio();
    }
    
    public void Step()
    {
        stepSounds.PlayAudio();
    }

    public void Attack()
    {
        attackSounds.PlayAudio();
    }
    
}
