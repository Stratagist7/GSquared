using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedAmmo : MonoBehaviour
{
	[SerializeField] private int ammoAmount = 10;
	[SerializeField] private DamageType ammoType;
	
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private Light pointLight;
	
	private void Update()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * 50, Space.World);
	}

	public void SetAmmo(int argAmount, DamageType argType)
	{
		ammoAmount = argAmount;
		ammoType = argType;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerGunAmmo>().PickUpAmmo(ammoAmount, ammoType);
			Destroy(gameObject);
		}
	}
}
