using System.Collections;
using UnityEngine;

public class MeleeChaser : MoveableAgent
{
	private static readonly int MOVE_KEY = Animator.StringToHash("Moving");
	private static readonly int SLOW_KEY = Animator.StringToHash("Slowed");
	private static readonly int ATTACK_KEY = Animator.StringToHash("Attack");
	
	[Space] 
	[SerializeField] private int damage = 5;
	[SerializeField] private float upThrust = 10f;
	[SerializeField] private float forwardThrust = 10f;
	[SerializeField] private float downThrustMultiplier = 1.5f;
	[SerializeField] private Rigidbody rb;
	[SerializeField] private Animator animator;

	private bool attacking = false;
	private bool settingUp = true;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(StartUp());
	}
	
	private void Update()
	{
		if (settingUp)
		{
			return;
		}
		
		if ((transform.position - Damageable.Player.transform.position).sqrMagnitude > agent.stoppingDistance && agent.enabled == true)
		{
			agent.SetDestination(Damageable.Player.transform.position);
		}
		else if(attacking == false && isStunned == false)
		{
			StartCoroutine(Attack());
		}

		if (animator != null)
		{
			animator.SetBool(MOVE_KEY, agent.enabled && agent.remainingDistance > agent.stoppingDistance);
			animator.SetBool(SLOW_KEY, isSlowed);
		}
	}

	// Wait for spawn animation before starting typical behaviors 
	private IEnumerator StartUp()
	{
		yield return new WaitForSeconds(1f);
		settingUp = false;
	}
	
	private IEnumerator Attack()
	{
		attacking = true;
		agent.isStopped = true;
		agent.enabled = false;

		if (animator != null)
		{
			animator.SetTrigger(ATTACK_KEY);
		}
		
		yield return new WaitForSeconds(0.3f);
		rb.AddForce(transform.forward * forwardThrust + Vector3.up * upThrust, ForceMode.Impulse);
		yield return new WaitForSeconds(0.1f);
		rb.AddForce(new Vector3(0, -upThrust / downThrustMultiplier, 0), ForceMode.Impulse);  // Force down quicker
		yield return new WaitForSeconds(0.75f);
		
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
