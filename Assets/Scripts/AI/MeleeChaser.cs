using System.Collections;
using UnityEngine;

public class MeleeChaser : MoveableAgent
{
    [Space]
    [SerializeField] private float upThrust = 10f;
    [SerializeField] private float forwardThrust = 10f;
    [SerializeField] private float downThrustMultiplier = 1.5f;
    [SerializeField] private Rigidbody rb;

    private bool attacking = false;
    
    private void Update()
    {
        if ((transform.position - Damageable.Player.transform.position).sqrMagnitude > agent.stoppingDistance && agent.enabled == true)
        {
            agent.SetDestination(Damageable.Player.transform.position);
        }
        else if(attacking == false)
        {
            StartCoroutine(Attack());
        }
    }
    
    private IEnumerator Attack()
    {
        attacking = true;
        agent.isStopped = true;
        agent.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Attacking");
        
        rb.AddForce(transform.forward * forwardThrust + Vector3.up * upThrust, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(new Vector3(0, -upThrust / downThrustMultiplier, 0), ForceMode.Impulse);  // Force down quicker
        yield return new WaitForSeconds(1.5f);
        
        gameObject.layer = LayerMask.NameToLayer("Default");
        agent.enabled = true;
        agent.isStopped = false;
        attacking = false;
    }
}
