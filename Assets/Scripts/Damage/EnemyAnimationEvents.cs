using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] private GameObject parent;

    public void Die()
    {
        Destroy(parent);
    }
}
