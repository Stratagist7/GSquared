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
	[SerializeField] private new Collider collider;
	[SerializeField] private bool isManuallySet;

	private void Awake()
	{
		collider.enabled = isManuallySet;
	}

	private void Update()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * 50, Space.World);
	}

	public void SetAmmo(int argAmount, DamageType argType)
	{
		ammoAmount = argAmount;
		ammoType = argType;
		spriteRenderer.sprite = ElementManager.instance.GetElement(ammoType).icon;
		pointLight.color = ElementManager.instance.GetElement(ammoType).color;
		collider.enabled = true;
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
