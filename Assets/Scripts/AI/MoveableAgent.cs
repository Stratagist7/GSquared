using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveableAgent : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    private float normSpeed;

    protected virtual void Start()
    {
        normSpeed = agent.speed;
    }

    public void SetSpeedMultiplier(float argMultiplier)
    {
        agent.speed = normSpeed * argMultiplier;
    }

    public void Stun(float argSeconds = ReactionValues.WIND_STUN_TIME)
    {
        agent.isStopped = true;
        StartCoroutine(StunCoroutine(argSeconds));
    }

    private void UnStun()
    {
        agent.isStopped = false;
    }

    private IEnumerator StunCoroutine(float argSeconds)
    {
        yield return new WaitForSeconds(argSeconds);
        UnStun();
    }
}
