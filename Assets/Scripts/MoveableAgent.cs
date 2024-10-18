using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveableAgent : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    private const float STUN_TIME = 3.0f;
    private float normSpeed;

    protected virtual void Start()
    {
        normSpeed = agent.speed;
    }

    public void SetSpeedMultiplier(float argMultiplier)
    {
        agent.speed = normSpeed * argMultiplier;
    }

    public void Stun()
    {
        agent.isStopped = true;
        StartCoroutine(StunCoroutine());
    }

    private void UnStun()
    {
        agent.isStopped = false;
    }

    private IEnumerator StunCoroutine()
    {
        yield return new WaitForSeconds(STUN_TIME);
        UnStun();
    }
}
