using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(HealthBar), typeof(Rigidbody))]
public class Damageable : MonoBehaviour
{
	[SerializeField] private int maxHealth = 100;
	[SerializeField] private DamageTypeUI[] damageTypeUI;
	[SerializeField] private PullRadius pr;
	private HealthBar healthBar;
	private MoveableAgent agent;

	private Dictionary<DamageType, float> types = new Dictionary<DamageType, float>();
	private const float MAX_TYPE_TIME = 6f;
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
		agent = GetComponent<MoveableAgent>();
		
		healthBar = GetComponent<HealthBar>();
		curHealth = maxHealth;
		tag = "Damageable";

		// Setting up reactions
		reactions[(DamageType.Earth, DamageType.Fire)] = () => TestReaction(DamageType.Earth, DamageType.Fire);
		reactions[(DamageType.Earth, DamageType.Ice)] = () => TestReaction(DamageType.Earth, DamageType.Ice);
		reactions[(DamageType.Earth, DamageType.Lightning)] = () => TestReaction(DamageType.Earth, DamageType.Lightning);
		reactions[(DamageType.Earth, DamageType.Water)] = () => TestReaction(DamageType.Earth, DamageType.Water);
		
		reactions[(DamageType.Fire, DamageType.Ice)] = () => TestReaction(DamageType.Fire, DamageType.Ice);
		reactions[(DamageType.Fire, DamageType.Lightning)] = () => TestReaction(DamageType.Fire, DamageType.Lightning);
		reactions[(DamageType.Fire, DamageType.Water)] = () => TestReaction(DamageType.Fire, DamageType.Water);
		
		reactions[(DamageType.Ice, DamageType.Water)] = () => TestReaction(DamageType.Ice, DamageType.Water);
		
		reactions[(DamageType.Lightning, DamageType.Water)] = () => TestReaction(DamageType.Lightning, DamageType.Water);
		
		reactions[(DamageType.Fire, DamageType.Wind)] = () => TestReaction(DamageType.Wind, DamageType.Fire);
		reactions[(DamageType.Ice, DamageType.Wind)] = () => TestReaction(DamageType.Wind, DamageType.Ice);
		reactions[(DamageType.Lightning, DamageType.Wind)] = () => TestReaction(DamageType.Wind, DamageType.Lightning);
		reactions[(DamageType.Water, DamageType.Wind)] = () => TestReaction(DamageType.Wind, DamageType.Water);
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
			if (Time.time - types[type] > MAX_TYPE_TIME)
			{
				EnableDamageTypeUI(type, false);
				types.Remove(type);
			}
		}
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
			TakeDamage(DamageType.None, 1);
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

		types.Remove(argType);
		types.Remove(reactType);
	}

	private void TestReaction(DamageType argType, DamageType reactType)
	{
		print(argType + " with " + reactType);
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
