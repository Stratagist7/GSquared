using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(HealthBar), typeof(Rigidbody))]
[RequireComponent(typeof(FreezeHandler))]
public class Damageable : MonoBehaviour
{
	public static GameObject Player;
	[SerializeField] private int maxHealth = 100;
	[SerializeField] private DamageTypeUI[] damageTypeUI;
	[SerializeField] private PullRadius pr;
	[SerializeField] private GameObject shieldParticlePrefab;
	private HealthBar healthBar;
	private MoveableAgent agent;
	private FreezeHandler freezeHandler;

	private Dictionary<DamageType, float> types = new Dictionary<DamageType, float>();
	private const float SLOW_MULTIPLIER = 0.5f;

	private bool isSlowed = false;
	private bool isBurning = false;

	private Dictionary<(DamageType, DamageType), Action> reactions = new();
	
	private int _curHealth;
	private int curHealth
	{
		get => _curHealth;
		set
		{
			_curHealth = value;
			healthBar.SetHealth(_curHealth);
		}
	}

	[Serializable]
	private class DamageTypeUI
	{
		public DamageType damageType;
		public GameObject uiObj;
	}

	private void Start()
	{
		if (Player == null)
		{
			Player = GameObject.FindGameObjectWithTag("Player");
		}
		
		agent = GetComponent<MoveableAgent>();
		freezeHandler = GetComponent<FreezeHandler>();
		
		healthBar = GetComponent<HealthBar>();
		curHealth = maxHealth;
		tag = "Damageable";

		// Setting up reactions
		reactions[(DamageType.Earth, DamageType.Fire)] = DropShieldParticle;
		reactions[(DamageType.Earth, DamageType.Ice)] = DropShieldParticle;
		reactions[(DamageType.Earth, DamageType.Lightning)] = DropShieldParticle;
		reactions[(DamageType.Earth, DamageType.Water)] = DropShieldParticle;

		reactions[(DamageType.Fire, DamageType.Ice)] = () => TakeDamage(DamageType.None, ReactionValues.MELT_DMG);
		reactions[(DamageType.Fire, DamageType.Lightning)] = () => pr.Explode();
		reactions[(DamageType.Fire, DamageType.Water)] = () => TakeDamage(DamageType.None, ReactionValues.VAPORIZE_DMG);

		reactions[(DamageType.Ice, DamageType.Water)] = () =>
		{
			freezeHandler.Freeze();
			if (agent != null)
			{
				agent.Stun(ReactionValues.FREEZE_TIME);
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

	public void TakeDamage(DamageType argType, int argDamage = -1)
	{
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
			Destroy(gameObject);
		}

		ApplyDamageTypeEffect(argType);
		
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

	private void DropShieldParticle()
	{
		Vector3 dirToPlayer = transform.position - Player.transform.position;
		Vector3 spawnPos = transform.position;
		if (Mathf.Abs(dirToPlayer.x) > Mathf.Abs(dirToPlayer.z))
		{
			spawnPos += dirToPlayer.x > 0 ? Vector3.left : Vector3.right;
		}
		else
		{
			spawnPos += dirToPlayer.z > 0 ? Vector3.back : Vector3.forward;
		}
		
		spawnPos.y += 1f;

		Instantiate(shieldParticlePrefab, spawnPos, shieldParticlePrefab.transform.rotation);
	}

	private void ApplyDamageType(DamageType argType)
	{
		types[argType] = Time.time;
		EnableDamageTypeUI(argType, true);
	}

	private void EnableDamageTypeUI(DamageType argType, bool argEnabled)
	{
		types[argType] = Time.time;
		GameObject uiObj = damageTypeUI[(int)argType].uiObj;
		uiObj.transform.SetAsFirstSibling();
		uiObj.SetActive(argEnabled);
	}
}

public enum DamageType {None = -1, Earth, Fire, Ice, Lightning, Water, Wind};
