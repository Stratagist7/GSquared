using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PickUpGun : MonoBehaviour
{
	[SerializeField] private GameObject interactText;
	[SerializeField] private GameObject gun;
	[SerializeField] private InputActionReference interactInput;
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player")){
			interactText.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player")){
			interactText.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		interactText.SetActive(false);
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player")){
			if (interactInput.action.phase == InputActionPhase.Performed)
			{
				if (other.GetComponent<WeaponControl>().GetGun())
				{
					gun.SetActive(false);
					Destroy(gameObject);
				}
			}
		}
	}
}
