using UnityEngine;

public class PlayerGunShooting : MonoBehaviour
{
	[SerializeField] private Transform bulletSpawn;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float bulletSpeed = 1f; 
	
	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Fire();
		}
	}

	private void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		bullet.GetComponentInChildren<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
	}
}
