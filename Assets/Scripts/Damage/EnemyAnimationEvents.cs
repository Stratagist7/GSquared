using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private MultiSfxHandler stepSounds;

    public void Die()
    {
        Destroy(parent);
    }

    public void Step()
    {
        stepSounds.PlayAudio();
    }
}
