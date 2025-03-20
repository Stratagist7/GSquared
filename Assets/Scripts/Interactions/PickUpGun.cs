using UnityEngine;

public class PickUpGun : Interactable
{
	
	[SerializeField] private GameObject gun;
	[SerializeField] private GameObject[] enableObjects;
	[SerializeField] private GlassBreaking glass;

	private void Start()
	{
		interactAction = GiveGun;
	}

	private void GiveGun(Collider other)
	{
		if (other.GetComponent<WeaponControl>().GetGun())
		{
			gun.SetActive(false);
					
			interactText.SetActive(false);
			foreach (GameObject obj in enableObjects)
			{
				obj.SetActive(true);
			}
			glass.TakeDamage(DamageType.None, 100);
			Destroy(gameObject);
		}
	}
}
