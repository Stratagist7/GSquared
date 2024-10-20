using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldParticle : MonoBehaviour
{

	private void Update()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * 50, Space.World);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			// give shield
			Destroy(gameObject);
		}
	}
}
