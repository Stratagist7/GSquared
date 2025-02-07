using System.Collections;
using UnityEngine;

public class BossBehavior : MoveableAgent
{
	private static readonly int MOVE_KEY = Animator.StringToHash("Moving");
	private static readonly int SLOW_KEY = Animator.StringToHash("Slowed");
	private static readonly int MELEE_KEY = Animator.StringToHash("Attack");
	private static readonly int RANGED_KEY = Animator.StringToHash("Range Attack");
	
	[SerializeField] private float meleeRange;
	[SerializeField] private float jumpRange;
	[SerializeField] private float poisonRange;
	[Space]
	[SerializeField] protected Animator animator;
	
	[Header("Audio")]
	[SerializeField] private MultiSfxHandler idleSfx;
	[SerializeField] private float idleSfxChance = 0.1f;
	[SerializeField] private float idleSfxCd = 2f;
	private float lastIdleTime;
	
	private bool doingAction = false;
	private bool settingUp = true;
	
	protected override void Start()
	{
		base.Start();
		StartCoroutine(StartUp());
	}
	
	private void Update()
	{
		if (settingUp || agent.enabled == false || doingAction)
		{
			return;
		}

		if ((transform.position - Damageable.Player.transform.position).sqrMagnitude <= meleeRange)
		{
			StartCoroutine(MeleeAttack());
		}
		else if ((transform.position - Damageable.Player.transform.position).sqrMagnitude <= jumpRange)
		{
			print("Jump Slam Attack");
		}
		else if ((transform.position - Damageable.Player.transform.position).sqrMagnitude <= poisonRange)
		{
			print("Spit poison");
		}
		else
		{
			print("move closer");
		}
		
		if (animator != null)
		{
			animator.SetBool(MOVE_KEY, agent.enabled && agent.remainingDistance > agent.stoppingDistance);
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
		//TODO turn to face player
		doingAction = true;
		animator.SetTrigger(MELEE_KEY);
		yield return new WaitForSeconds(1f);
		
		doingAction = false;
	}

	// private IEnumerator JumpAttack()
	// {
	// 	
	// }
}
