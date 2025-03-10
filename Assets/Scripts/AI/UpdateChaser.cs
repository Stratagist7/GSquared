using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UpdateChaser : MonoBehaviour
{
	[SerializeField] private bool alwaysAttack;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Damageable"))
		{
			other.GetComponent<MeleeChaser>().SetAlwaysAttacking(alwaysAttack);
		}
	}
}
