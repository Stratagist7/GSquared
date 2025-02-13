using System.Collections;
using UnityEngine;

public class BossBehavior : MoveableAgent
{
#region Static Variables
	// Animator variables
	private static readonly int MOVE_KEY = Animator.StringToHash("Moving");
	private static readonly int TURN_KEY = Animator.StringToHash("Turning");
	private static readonly int TURN_RIGHT_KEY = Animator.StringToHash("Turn Right");
	private static readonly int SLOW_KEY = Animator.StringToHash("Slowed");
	private static readonly int MELEE_KEY = Animator.StringToHash("Melee Attack");
    private static readonly int JUMP_KEY = Animator.StringToHash("Jump Attack");
	private static readonly int RANGED_KEY = Animator.StringToHash("Range Attack");
	
	// Const numbers
	private const float MELEE_ANGLE = 15f;
	private const float MELEE_TIMEOUT = 5f;
	private const float MELEE_DURATION = 0.85f;
	private const float JUMP_DURATION = 2.5f/1.5f;
	private const float JUMP_RANGE_DURATION = 1.1f/1.5f;
	private const float JUMP_DAMAGE_DURATION = 0.2f;
	private const float RANGED_ANGLE = 2f;
	private const float RANGED_TIMEOUT = 3f;
	private const float RANGED_DURATION = 1f;
#endregion // Static Variables	
	
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
	[SerializeField] private GameObject rangeHitboxPrefab;
	[SerializeField] private GameObject jumpDustPrefab;
	
	[Header("Audio")]
	[SerializeField] private MultiSfxHandler idleSfx;
	[SerializeField] private float idleSfxChance = 0.1f;
	[SerializeField] private float idleSfxCd = 2f;
	[SerializeField] private EffectSfxHandler poisonSfx;
	private float lastIdleTime;
	
	private bool doingAction = false;
	private bool canTurn = true;
	private bool isTurning = false;
	private bool rightTurn = false;
	private bool canAnimateTurn = true;
	public bool settingUp = true;
	private readonly float baseTurnSpeed = 90f;
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

		if (canTurn)
		{
			Vector3 target = (Damageable.Player.transform.position - transform.position).normalized;
			rightTurn = Vector3.Dot(transform.right, target) < 0;
			Quaternion targetRot = Quaternion.LookRotation(target);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
			isTurning = Quaternion.Angle(transform.rotation, targetRot) > 0;
		}


		if (agent.enabled == false || doingAction)
		{
			return;
		}

		if ((transform.position - Damageable.Player.transform.position).sqrMagnitude <= meleeRange)
		{
			agent.ResetPath();
			StartCoroutine(MeleeAttack());
		}
		else if ((transform.position - Damageable.Player.transform.position).sqrMagnitude <= jumpRange)
		{
			agent.ResetPath();
			StartCoroutine(JumpAttack());
		}
		else if ((transform.position - Damageable.Player.transform.position).sqrMagnitude <= poisonRange)
		{
			agent.ResetPath();
			StartCoroutine(RangedAttack());
		}
		else
		{
			agent.SetDestination(Damageable.Player.transform.position);
		}
		
		if (animator != null)
		{
			animator.SetBool(MOVE_KEY, agent.enabled && agent.remainingDistance > agent.stoppingDistance);
			animator.SetBool(SLOW_KEY, isSlowed);
			animator.SetBool(TURN_KEY, canAnimateTurn && (canTurn && isTurning) && (agent.enabled == false || agent.remainingDistance <= agent.stoppingDistance));
			animator.SetBool(TURN_RIGHT_KEY, rightTurn);
		}
	}
	
	private IEnumerator StartUp()
	{
		yield return new WaitForSeconds(1f);
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
		doingAction = true;
		turnSpeed = 100f;
		
		// Ensure Facing player
		Vector3 target = (Damageable.Player.transform.position - transform.position).normalized;
		Quaternion targetRot = Quaternion.LookRotation(target);
		float t = 0;
		while (Quaternion.Angle(transform.rotation, targetRot) > MELEE_ANGLE)
		{
			t += Time.deltaTime;

			if ((transform.position - Damageable.Player.transform.position).sqrMagnitude > meleeRange)
			{
				turnSpeed = baseTurnSpeed;
				doingAction = false;
				yield break;
			}
			
			if (t > MELEE_TIMEOUT)
			{
				break;
			}
			yield return null;
		}
		
		canTurn = false;
		animator.SetTrigger(MELEE_KEY);
		yield return new WaitForSeconds(MELEE_DURATION);
		
		turnSpeed = baseTurnSpeed;
		canTurn = true;
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
		doingAction = true;
		canAnimateTurn = false;
		agent.isStopped = true;
		agent.enabled = false;
		
		animator.SetTrigger(JUMP_KEY);
		yield return new WaitForSeconds(JUMP_DURATION);
		
		agent.enabled = true;
		agent.isStopped = false;
		canAnimateTurn = true;
		doingAction = false;
	}

	public void JumpUp()
	{
		rb.AddForce(Vector3.up * upThrust, ForceMode.Impulse);
		GameObject hitbox = Instantiate(rangeHitboxPrefab, new Vector3(transform.position.x, rangeHitboxPrefab.transform.position.y, transform.position.z), Quaternion.identity);
		Destroy(hitbox, JUMP_RANGE_DURATION);
	}

	public void JumpDown()
	{
		rb.AddForce(Vector3.down * downThrust, ForceMode.Impulse);
	}
	
	public void SpawnJumpHitbox()
	{
		GameObject hitbox = Instantiate(jumpHitboxPrefab, new Vector3(transform.position.x, jumpHitboxPrefab.transform.position.y, transform.position.z), Quaternion.identity);
		Instantiate(jumpDustPrefab, new Vector3(transform.position.x, jumpDustPrefab.transform.position.y, transform.position.z), Quaternion.identity);
		Destroy(hitbox, JUMP_DAMAGE_DURATION);
	}
#endregion	// Jump Attack

#region Ranged Attack	
	private IEnumerator RangedAttack()
	{
		doingAction = true;

		// Ensure Facing player
		Vector3 target = (Damageable.Player.transform.position - transform.position).normalized;
		Quaternion targetRot = Quaternion.LookRotation(target);
		float t = 0;
		while (Quaternion.Angle(transform.rotation, targetRot) > RANGED_ANGLE)
		{
			t += Time.deltaTime;

			float dist = (transform.position - Damageable.Player.transform.position).sqrMagnitude;
			if (dist > poisonRange || dist <= jumpRange)
			{
				doingAction = false;
				yield break;
			}
			
			if (t > RANGED_TIMEOUT)
			{
				break;
			}
			yield return null;
		}
		
		canTurn = false;
		animator.SetTrigger(RANGED_KEY);
		yield return new WaitForSeconds(RANGED_DURATION);
		
		canTurn = true;
		doingAction = false;
	}

	public void SpitPoison()
	{
		poisonSpit.Play();
	}

	public void PoisonSfx()
	{
		poisonSfx.PlayAudio();
	}
#endregion // Ranged Attack	
}
