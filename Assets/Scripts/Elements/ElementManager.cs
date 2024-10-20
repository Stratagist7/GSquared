using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
	public static ElementManager instance;
	
	[SerializeField] private Element[] elements;

	private void Awake()
	{
		if (instance == null) {
			instance = this;
		}  else
			Destroy(gameObject);
	}
	
	public Element GetElement(DamageType argType)
	{
		return elements[(int)argType];
	}
}

public static class ReactionValues
{
	// Specific type damage found in Element SO
	
	// Duration
	public const float MAX_TYPE_TIME = 10f;  // Might move to element SO
	
	public const float WIND_STUN_TIME = 1.5f;
	public const float FREEZE_TIME = 5.0f;
	
	// Damage
	public const int BURN_DMG = 1;
	
	public const int MELT_DMG = 10;
	public const int VAPORIZE_DMG = 10;
	public const int EXP_DMG = 8;
	public const int CHAIN_DMG = 15;
}
