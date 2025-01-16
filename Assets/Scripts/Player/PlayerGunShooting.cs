#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using System;
using UnityEngine;

public class PlayerGunShooting : MonoBehaviour
{
	private static readonly int attackKey = Animator.StringToHash("t_attack");
	private static readonly int indexKey = Animator.StringToHash("i_attack_index");
	
	[SerializeField] private Transform bulletSpawn;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float bulletSpeed = 1f;
	
	[SerializeField, Tooltip("Bullets per second")] private int fireRate = 20;
	[SerializeField] private PlayerGunAmmo ammo;
	[SerializeField] private Animator animator;
	[SerializeField] private InputActionReference actionRef;
	
	private float timeSinceLastFire;
	
	private Vector3 point;
	private GameObject target;

	private void Update()
	{
		SetAim();
		if (actionRef.action.phase == InputActionPhase.Performed)
		{
			if (Time.time - timeSinceLastFire > 1.0 / fireRate)
			{
				Fire();
				timeSinceLastFire = Time.time;
			}
		}
	}
	
	private void OnEnable()
	{
		actionRef.action.Enable();
	}

	private void OnDisable()
	{
		actionRef.action.Disable();
	}

	private void Fire()
	{
		if (ammo.CanShoot())
		{
			animator.SetInteger(indexKey, 0);
			animator.SetTrigger(attackKey);
			PlayerSoundManager.instance.PlayGunShot();
			
			ammo.curAmmo -= 1;
			GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
			bullet.GetComponent<BulletDamage>().SetTarget(ammo.damageType, target, point);
			bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
		}
		else if(ammo.CanReload())
		{
			ammo.Reload();
		}
		else if(ammo.isReloading == false && MenuUI.Paused == false)
		{
			PlayerSoundManager.instance.PlayEmptyClip();
		}
	}

	private void SetAim()
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
		Physics.Raycast(ray, out RaycastHit hitInfo);
		Debug.DrawRay(transform.position, ray.direction * 10, Color.red);
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
