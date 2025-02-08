using System.Collections;
using UnityEngine;

public class BossBehavior : MoveableAgent
{
	private static readonly int MOVE_KEY = Animator.StringToHash("Moving");
	private static readonly int SLOW_KEY = Animator.StringToHash("Slowed");
	private static readonly int MELEE_KEY = Animator.StringToHash("Attack");
    private static readonly int JUMP_KEY = Animator.StringToHash("Jump Attack");
	private static readonly int RANGED_KEY = Animator.StringToHash("Range Attack");
	
	[SerializeField] private float meleeRange;
	[SerializeField] private float jumpRange;
	[SerializeField] private float poisonRange;
	[SerializeField] private float upThrust;
	[SerializeField] private float downThrust;
	[Space]
	[SerializeField] private Animator animator;
	[SerializeField] private Rigidbody rb;
	[SerializeField] private ParticleSystem poisonSpit;
	
	[Header("Audio")]
	[SerializeField] private MultiSfxHandler idleSfx;
	[SerializeField] private float idleSfxChance = 0.1f;
	[SerializeField] private float idleSfxCd = 2f;
	private float lastIdleTime;
	
	private bool doingAction = false;
	private bool isTurning = false;
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

		if (isStunned)
		{
			return;
		}
		
		Vector3 target = (Damageable.Player.transform.position - transform.position).normalized;
		Quaternion targetRot = Quaternion.LookRotation(target);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 20f * Time.deltaTime);
		isTurning = Quaternion.Angle(transform.rotation, targetRot) > 0.05f;

		if (agent.enabled == false || doingAction)
		{
			return;
		}

		if ((transform.position - Damageable.Player.transform.position).sqrMagnitude <= meleeRange)
		{
			StartCoroutine(MeleeAttack());
		}
		else if ((transform.position - Damageable.Player.transform.position).sqrMagnitude <= jumpRange)
		{
			StartCoroutine(JumpAttack());
		}
		else if ((transform.position - Damageable.Player.transform.position).sqrMagnitude <= poisonRange)
		{
			StartCoroutine(RangedAttack());
		}
		else
		{
			print("move closer");
		}
		
		if (animator != null)
		{
			animator.SetBool(MOVE_KEY, isTurning || (agent.enabled && agent.remainingDistance > agent.stoppingDistance));
			animator.SetBool(SLOW_KEY, isSlowed);
		}
	}
	
	private IEnumerator StartUp()
	{
		yield return new WaitForSeconds(1f);
		settingUp = false;
		StartCoroutine(TryPlayIdle());
	}
	
	private IEnumerator TryPlayIdle()
	{
		while (gameObject.activeSelf)
		{
			if (doingAction == false && isStunned == false && Random.Range(0f, 1f) < idleSfxChance)
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

	private IEnumerator MeleeAttack()
	{
		//TODO deal damage
		doingAction = true;
		animator.SetTrigger(MELEE_KEY);
		yield return new WaitForSeconds(1f);
		
		doingAction = false;
	}

	private IEnumerator JumpAttack()
	{
		//TODO deal damage
		doingAction = true;
		agent.isStopped = true;
		agent.enabled = false;
		
		animator.SetTrigger(JUMP_KEY);
		yield return new WaitForSeconds(3f);
		
		agent.enabled = true;
		agent.isStopped = false;
		doingAction = false;
	}

	public void JumpUp()
	{
		rb.AddForce(Vector3.up * upThrust, ForceMode.Impulse);
	}

	public void JumpDown()
	{
		rb.AddForce(Vector3.down * downThrust, ForceMode.Impulse);
	}

	private IEnumerator RangedAttack()
	{
		//TODO contact damage of poison
		doingAction = true;
		
		// Ensure Facing player
		Vector3 target = (Damageable.Player.transform.position - transform.position).normalized;
		Quaternion targetRot = Quaternion.LookRotation(target);
		while (Quaternion.Angle(transform.rotation, targetRot) > 2f)
		{
			yield return null;
		}
		
		animator.SetTrigger(RANGED_KEY);
		yield return new WaitForSeconds(3f);
		
		doingAction = false;
	}

	public void SpitPoison()
	{
		poisonSpit.Play();
	}
}
