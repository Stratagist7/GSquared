using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider), typeof(HealthBar), typeof(Rigidbody))]
[RequireComponent(typeof(FreezeHandler))]
public class Damageable : Hitable
{
	public static GameObject Player;
	private static PlayerHealth PlayerSheilds;
	
	private static readonly int DEATH_KEY = Animator.StringToHash("Die");
	private static readonly int DEAD_KEY = Animator.StringToHash("Dead");
	
	[SerializeField] private GameObject[] damageTypeUI;
	[SerializeField] private PullRadius pr;
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject healthUI;
	[SerializeField] private GameObject steamPrefab;
	[Header("Dropped Ammo")] 
	[SerializeField] private GameObject droppedAmmoPrefab;
	[SerializeField] private int ammoAmount;
	[SerializeField] private DamageType damageType;
	
	private HealthBar healthBar;
	private MoveableAgent agent;
	private FreezeHandler freezeHandler;

	private Dictionary<DamageType, float> types = new Dictionary<DamageType, float>();
	private const float SLOW_MULTIPLIER = 0.5f;

	private bool isSlowed = false;
	private bool isBurning = false;

	private Dictionary<(DamageType, DamageType), Action> reactions = new();
	
	
	private int curHealth
	{
		get => _curHealth;
		set
		{
			_curHealth = value;
			healthBar.SetHealth(_curHealth);
		}
	}

	private void Awake()
	{
		if (Player == null)
		{
			Player = GameObject.FindGameObjectWithTag("Player");
			PlayerSheilds = Player.GetComponent<PlayerHealth>();
		}
		
	}

	protected override void Start()
	{
		agent = GetComponent<MoveableAgent>();
		freezeHandler = GetComponent<FreezeHandler>();
		
		healthBar = GetComponent<HealthBar>();
		healthBar.SetMaxHealth(maxHealth);
		base.Start();

		// Setting up reactions
		reactions[(DamageType.Earth, DamageType.Fire)] = GiveShieldParticle;
		reactions[(DamageType.Earth, DamageType.Ice)] = GiveShieldParticle;
		reactions[(DamageType.Earth, DamageType.Lightning)] = GiveShieldParticle;
		reactions[(DamageType.Earth, DamageType.Water)] = GiveShieldParticle;

		reactions[(DamageType.Fire, DamageType.Ice)] = () =>
		{
			TakeDamage(DamageType.None, ReactionValues.MELT_DMG);
			Instantiate(steamPrefab, transform);
		};
		reactions[(DamageType.Fire, DamageType.Lightning)] = () => pr.Explode();
		reactions[(DamageType.Fire, DamageType.Water)] = () =>
		{
			TakeDamage(DamageType.None, ReactionValues.VAPORIZE_DMG);
			Instantiate(steamPrefab, transform);
		};

		reactions[(DamageType.Ice, DamageType.Water)] = () =>
		{
			freezeHandler.Freeze();
			if (agent != null)
			{
				agent.Stun(ReactionValues.FREEZE_TIME, false);
			}
		};

		reactions[(DamageType.Lightning, DamageType.Water)] = () => StartCoroutine(pr.ChainReaction());

		reactions[(DamageType.Fire, DamageType.Wind)] = () => pr.Swirl(DamageType.Fire);
		reactions[(DamageType.Ice, DamageType.Wind)] = () => pr.Swirl(DamageType.Ice);
		reactions[(DamageType.Lightning, DamageType.Wind)] = () => pr.Swirl(DamageType.Lightning);
		reactions[(DamageType.Water, DamageType.Wind)] = () => pr.Swirl(DamageType.Water);
	}

	private void Update()
	{
		UpdateTypes();
	}

	private void UpdateTypes()
	{
		if (types.Count <= 0)
		{
			return;
		}
		
		List<DamageType> keyList = new List<DamageType>(types.Keys);
		foreach (DamageType type in keyList)
		{
			if (Time.time - types[type] > ReactionValues.MAX_TYPE_TIME)
			{
				RemoveType(type);
			}
		}
	}

	private void RemoveType(DamageType argType)
	{
		EnableDamageTypeUI(argType, false);
		types.Remove(argType);
	}

	public override void TakeDamage(DamageType argType, int argDamage = -1)
	{
		if (curHealth <= 0)
		{
			return;
		}
		
		if (argDamage < 0)
		{
			curHealth -= ElementManager.instance.GetElement(argType).damage;
		}
		else
		{
			curHealth -= argDamage;
		}
		
		// if dead
		if (curHealth <= 0)
		{
			if (animator != null)
			{
				agent.Stun(10, false, true);  // Prevent damage after death
				animator.SetTrigger(DEATH_KEY);
				animator.SetBool(DEAD_KEY, true);
				healthUI.SetActive(false);
			}
			else
			{
				Destroy(gameObject);
			}

			DropAmmo();
		}

		if (agent is MeleeWanderer wanderer)
		{
			wanderer.isAlert = true;
		}

		ApplyDamageTypeEffect(argType);
	}

	private void DropAmmo()
	{
		DroppedAmmo ammo = Instantiate(droppedAmmoPrefab, transform.position + Vector3.up * 0.75f, Quaternion.identity).GetComponent<DroppedAmmo>();
		if (damageType == DamageType.None)
		{
			damageType = (DamageType)Random.Range(0, 6);
		}
		ammo.SetAmmo(ammoAmount, damageType);
	}
	
#region Solo Effects
	private IEnumerator Burning()
	{
		isBurning = true;
		while (types.ContainsKey(DamageType.Fire))
		{
			yield return new WaitForSeconds(0.25f);
			TakeDamage(DamageType.None, ReactionValues.BURN_DMG);
		}
		isBurning = false;
	}

	private IEnumerator Slowed()
	{
		if (!agent)
		{
			yield break;
		}
		isSlowed = true;
		agent.SetSpeedMultiplier(SLOW_MULTIPLIER);
		while (types.ContainsKey(DamageType.Ice))
		{
			yield return null;
		}
		agent.SetSpeedMultiplier(1);
		isSlowed = false;
	}

	private void Pull()
	{
		pr.PullObjects();
	}

	public bool IsWet()
	{
		return types.ContainsKey(DamageType.Water);
	}
#endregion // Solo Effects
	
#region Reactions	
	private void ApplyDamageTypeEffect(DamageType argType)
	{
		if (argType == DamageType.None)
		{
			return;
		}
		
		switch (argType)
		{
			case DamageType.Earth:
				break;
			case DamageType.Fire:
				ApplyDamageType(argType);
				if (!isBurning)
				{
					StartCoroutine(Burning());
				}
				break;
			case DamageType.Ice:
				ApplyDamageType(argType);
				if (!isSlowed)
				{
					StartCoroutine(Slowed());
				}
				break;
			case DamageType.Wind:
				Pull();
				break;
			default:
				ApplyDamageType(argType);
				break;
		}
		
		CheckReaction(argType);
	}

	private void CheckReaction(DamageType argType)
	{
		DamageType[] reactList = ElementManager.instance.GetElement(argType).reactsWith;
		List<DamageType> orderedTypes = types.OrderBy(pair => pair.Value).Select(pair => pair.Key).ToList();
		foreach (DamageType type in orderedTypes)
		{
			if (reactList.Contains(type))
			{
				TriggerReaction(argType, type);
				break;
			}	
		}
	}

	private void TriggerReaction(DamageType argType, DamageType reactType)
	{
		(DamageType, DamageType) key = argType < reactType ? (argType, reactType) : (reactType, argType);
		reactions[key]();
		
		if (argType == DamageType.Wind)  // Keep the elements if swirled
		{
			return;
		}
		
		RemoveType(argType);
		RemoveType(reactType);
	}

	private void GiveShieldParticle()
	{
		PlayerSheilds.Shield(ReactionValues.SHIELD_AMT);
	}
#endregion // Reactions
	
	private void ApplyDamageType(DamageType argType)
	{
		types[argType] = Time.time;
		EnableDamageTypeUI(argType, true);
	}

	private void EnableDamageTypeUI(DamageType argType, bool argEnabled)
	{
		types[argType] = Time.time;
		damageTypeUI[(int)argType].transform.SetAsFirstSibling();
		damageTypeUI[(int)argType].SetActive(argEnabled);
	}
}

public enum DamageType {None = -1, Earth, Fire, Ice, Lightning, Water, Wind};
