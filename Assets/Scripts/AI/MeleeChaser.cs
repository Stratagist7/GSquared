using System.Collections;
using UnityEngine;

public class MeleeChaser : MoveableAgent
{
	protected static readonly int MOVE_KEY = Animator.StringToHash("Moving");
	protected static readonly int SLOW_KEY = Animator.StringToHash("Slowed");
	private static readonly int ATTACK_KEY = Animator.StringToHash("Attack");
	
	[Space] 
	[SerializeField] private int damage = 5;
	[SerializeField] private float upThrust = 10f;
	[SerializeField] private float forwardThrust = 10f;
	[SerializeField] private float downThrustMultiplier = 1.5f;
	[SerializeField] protected Animator animator;

	[Header("Audio")]
	[SerializeField] private MultiSfxHandler idleSfx;
	[SerializeField] private float idleSfxChance = 0.1f;
	[SerializeField] private float idleSfxCd = 2f;
	private float lastIdleTime;

	private float dist;
	private bool attacking = false;
	protected bool settingUp = true;
	private bool shouldUnStun = false;

	protected override void Start()
	{
		base.Start();
		dist = Mathf.Pow(agent.stoppingDistance, 2);
		StartCoroutine(StartUp());
	}
	
	protected virtual void Update()
	{
		if (settingUp)
		{
			return;
		}
		
		if (agent.enabled && (transform.position - Damageable.Player.transform.position).sqrMagnitude > dist)
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
		StartCoroutine(TryPlayIdle());
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
		if (shouldUnStun)
		{
			UnStun();
			shouldUnStun = false;
		}
	}

	protected override void UnStun()
	{
		if (attacking == false)
		{
			base.UnStun();
		}
		else
		{
			shouldUnStun = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (attacking && isStunned == false && other.gameObject.CompareTag("Player"))
		{
			Damageable.Player.GetComponent<PlayerHealth>().Damage(damage);
		}
	}

	private IEnumerator TryPlayIdle()
	{
		while (gameObject.activeSelf)
		{
			if (attacking == false && isStunned == false && Random.Range(0f, 1f) < idleSfxChance)
			{
				idleSfx.PlayAudio();
				yield return new WaitForSeconds(idleSfxCd);
			}
			else
			{
				yield return new WaitForSeconds(1);
			}
		}
	}
}
