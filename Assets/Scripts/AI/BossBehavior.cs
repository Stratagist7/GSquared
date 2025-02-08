using System.Collections;
using UnityEngine;

public class BossBehavior : MoveableAgent
{
	private static readonly int MOVE_KEY = Animator.StringToHash("Moving");
	// private static readonly int TURN_KEY = Animator.StringToHash("Turning");
	// private static readonly int TURN_RIGHT_KEY = Animator.StringToHash("Turn Right");
	private static readonly int SLOW_KEY = Animator.StringToHash("Slowed");
	private static readonly int MELEE_KEY = Animator.StringToHash("Melee Attack");
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
	[SerializeField] private GameObject meleeHitbox;
	[SerializeField] private GameObject jumpHitboxPrefab;
	
	[Header("Audio")]
	[SerializeField] private MultiSfxHandler idleSfx;
	[SerializeField] private float idleSfxChance = 0.1f;
	[SerializeField] private float idleSfxCd = 2f;
	private float lastIdleTime;
	
	private bool doingAction = false;
	// private bool isTurning = false;
	// private bool rightTurn = false;
	private bool settingUp = true;
	private readonly float baseTurnSpeed = 20f;
	private float turnSpeed;
	
	protected override void Start()
	{
		base.Start();
		turnSpeed = baseTurnSpeed;
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
		//rightTurn = Vector3.Dot(transform.right, target) > 0;
		Quaternion targetRot = Quaternion.LookRotation(target);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
		//isTurning = Quaternion.Angle(transform.rotation, targetRot) > 0.5f;

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
			animator.SetBool(MOVE_KEY, agent.enabled && agent.remainingDistance > agent.stoppingDistance);
			animator.SetBool(SLOW_KEY, isSlowed);
			// animator.SetBool(TURN_KEY, isTurning && (agent.enabled == false || agent.remainingDistance <= agent.stoppingDistance));
			// animator.SetBool(TURN_RIGHT_KEY, rightTurn);
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

#region Melee Attack
	private IEnumerator MeleeAttack()
	{
		//TODO deal damage
		doingAction = true;
		turnSpeed = 60f;
		
		// Ensure Facing player
		Vector3 target = (Damageable.Player.transform.position - transform.position).normalized;
		Quaternion targetRot = Quaternion.LookRotation(target);
		while (Quaternion.Angle(transform.rotation, targetRot) > 15f)
		{
			yield return null;
		}
		
		animator.SetTrigger(MELEE_KEY);
		yield return new WaitForSeconds(1f);
		
		turnSpeed = baseTurnSpeed;
		doingAction = false;
	}

	public void EnableMeleeHitbox(bool argShouldEnable)
	{
		meleeHitbox.SetActive(argShouldEnable);
	}
#endregion // Melee Attack

#region Jump Attack
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
	
	public void SpawnJumpHitbox()
	{
		GameObject hitbox = Instantiate(jumpHitboxPrefab, new Vector3(transform.position.x, jumpHitboxPrefab.transform.position.y, transform.position.z), Quaternion.identity);
		Destroy(hitbox, 0.2f);
	}
#endregion	// Jump Attack

#region Ranged Attack	
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
#endregion // Ranged Attack	
}
