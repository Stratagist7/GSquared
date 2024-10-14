using System;
using UnityEngine;

public class PlayerGunShooting : MonoBehaviour
{
	private static readonly int attackKey = Animator.StringToHash("t_attack");
	
	[SerializeField] private GameObject playerLook;
	[SerializeField] private Transform bulletSpawn;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float bulletSpeed = 1f;
	
	[SerializeField, Tooltip("Bullets per second")] private int fireRate = 20;
	[SerializeField] private PlayerGunAmmo ammo;
	[SerializeField] private Animator animator;
	private float timeSinceLastFire;
	
	
	private Vector3 point;
	private GameObject target;

	private void Update()
	{
		SetAim();
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
		if (ammo.CanShoot())
		{
			ammo.curAmmo -= 1;
			GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
			bullet.GetComponent<BulletDamage>().SetTarget(ammo.damageType, target, point);
			bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);

			animator.ResetTrigger(attackKey);
			animator.SetTrigger(attackKey);
			// TODO: play sfx
		}
	}

	private void SetAim()
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
		Physics.Raycast(ray, out RaycastHit hitInfo);
		if (hitInfo.point != Vector3.zero)
		{
			point = hitInfo.point;
			target = hitInfo.transform.gameObject;
			Vector3 targetDirection = hitInfo.point - transform.position;
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
		}
	}
}
