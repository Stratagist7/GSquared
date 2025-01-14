using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour
{
	[SerializeField] protected int maxHealth = 100;
	protected int _curHealth;

	protected virtual void Start()
	{
		_curHealth = maxHealth;
		tag = "Damageable";
	}
	
	public virtual void TakeDamage(DamageType argType, int argDamage = -1)
	{
		if (_curHealth <= 0)
		{
			return;
		}
		_curHealth -= argDamage;
		
	}
}
