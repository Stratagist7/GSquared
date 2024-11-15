using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveableAgent : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    private float normSpeed;
    protected bool isStunned = false;

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
        isStunned = true;
        if (agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        StartCoroutine(StunCoroutine(argSeconds));
    }

    protected virtual void UnStun()
    {
        agent.enabled = true;
        agent.isStopped = false;
        isStunned = false;
    }

    private IEnumerator StunCoroutine(float argSeconds)
    {
        yield return new WaitForSeconds(argSeconds);
        UnStun();
    }
}
