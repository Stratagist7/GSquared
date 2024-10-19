using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
		
	}
	

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
