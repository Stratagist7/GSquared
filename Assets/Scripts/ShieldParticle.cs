using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldParticle : MonoBehaviour
{
	[SerializeField] private int shieldAmount = 10;
	
	private void Update()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * 50, Space.World);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.GetComponent<PlayerHealth>().Shield(shieldAmount);
			Destroy(gameObject);
		}
	}
}
