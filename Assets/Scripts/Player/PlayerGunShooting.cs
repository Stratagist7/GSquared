using System;
using UnityEngine;

public class PlayerGunShooting : MonoBehaviour
{
	[SerializeField] private GameObject playerLook;
	[SerializeField] private Transform bulletSpawn;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float bulletSpeed = 1f;
	
	[SerializeField, Tooltip("Bullets per second")] private int fireRate = 20;
	private float timeSinceLastFire;

	private void Start()
	{
		SetGunAim();
	}

	private void Update()
	{
		if (Input.GetButton("Fire1"))
		{
			if (Time.time - timeSinceLastFire > 1.0 / fireRate)
			{
				Fire();
				timeSinceLastFire = Time.time;
			}
		}
	}

	private void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
		// TODO: play sfx
	}

	private void SetGunAim()
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
		Physics.Raycast(ray, out RaycastHit hitInfo);
		transform.LookAt(hitInfo.point);
	}
}
