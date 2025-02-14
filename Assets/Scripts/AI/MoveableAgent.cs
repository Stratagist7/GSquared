using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveableAgent : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Rigidbody rb;
    private float normSpeed;
    protected bool isStunned = false;
    protected bool isSlowed = false;
    private int stunCounter = 0;

    protected virtual void Start()
    {
        normSpeed = agent.speed;
    }

    public void SetSpeedMultiplier(float argMultiplier)
    {
        isSlowed = argMultiplier < 1;
        agent.speed = normSpeed * argMultiplier;
    }

    public void Stun(float argSeconds = ReactionValues.WIND_STUN_TIME)
    {
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        isStunned = true;
        stunCounter++;
        if (agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        StartCoroutine(StunCoroutine(argSeconds));
    }

    protected virtual void UnStun()
    {
        stunCounter--;
        if (stunCounter != 0)
        {
            return;
        }
        
        if (rb != null)
        {
            rb.isKinematic = true;
        }
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
