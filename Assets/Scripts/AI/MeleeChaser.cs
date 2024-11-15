using System.Collections;
using UnityEngine;

public class MeleeChaser : MoveableAgent
{
	[Space] 
	[SerializeField] private int damage = 5;
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
		else if(attacking == false && isStunned == false)
		{
			StartCoroutine(Attack());
		}
	}
	
	private IEnumerator Attack()
	{
		attacking = true;
		agent.isStopped = true;
		agent.enabled = false;
		
		rb.AddForce(transform.forward * forwardThrust + Vector3.up * upThrust, ForceMode.Impulse);
		yield return new WaitForSeconds(0.1f);
		rb.AddForce(new Vector3(0, -upThrust / downThrustMultiplier, 0), ForceMode.Impulse);  // Force down quicker
		yield return new WaitForSeconds(1.5f);
		
		if (isStunned == false) // if frozen, depend on frozen timer rather than attack
		{
			agent.enabled = true;
			agent.isStopped = false;
		}

		attacking = false;
	}

	protected override void UnStun()
	{
		if (attacking == false)
		{
			base.UnStun();
		}
		isStunned = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (attacking && other.gameObject.CompareTag("Player"))
		{
			Damageable.Player.GetComponent<PlayerHealth>().Damage(damage);
		}
	}
}
