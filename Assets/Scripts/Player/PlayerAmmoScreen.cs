using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoScreen : MonoBehaviour
{
	[SerializeField] private Transform highlight;
	[SerializeField] private PlayerGunAmmo ammo;

	private readonly Dictionary<int, DamageType> ammoAngle = new Dictionary<int, DamageType>()
		{
			{0, DamageType.Wind},
			{60, DamageType.Earth},
			{120, DamageType.Fire},
			{180, DamageType.Lightning},
			{-180, DamageType.Lightning},
			{-120, DamageType.Water},
			{-60, DamageType.Ice}
		};

	private DamageType curType = DamageType.None;
	private Vector2 mouseLoc;
	
	private void OnDisable()
	{
		ammo.damageType = curType;
	}
	
	private void Update()
	{
		if (Mathf.Approximately(mouseLoc.x, Input.mousePosition.x) && Mathf.Approximately(mouseLoc.y, Input.mousePosition.y))
		{
			return;
		}
		
		mouseLoc.x = Input.mousePosition.x - Screen.width * 0.5f;
		mouseLoc.y = Input.mousePosition.y - Screen.height * 0.5f;
		mouseLoc.Normalize();
		
		if (mouseLoc != Vector2.zero)
		{
			float angle = Mathf.Atan2(mouseLoc.y, mouseLoc.x) * Mathf.Rad2Deg + 30;
			int closeAngle = GetClosestAngle(angle);
			highlight.rotation = Quaternion.Euler(0, 0, closeAngle);
			curType = ammoAngle[closeAngle];
		}
	}

	private int GetClosestAngle(float argAngle)
	{
		int closestAngle = 0;
		double minDif = Double.MaxValue;

		foreach (int angle in ammoAngle.Keys)
		{
			double dif = Math.Abs(argAngle - angle);
			if (dif < minDif)
			{
				closestAngle = angle;
				minDif = dif;
			}
		}
		
		return closestAngle;
	}
}
