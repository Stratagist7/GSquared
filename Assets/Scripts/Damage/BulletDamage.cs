using UnityEngine;

public class BulletDamage : MonoBehaviour
{
	[SerializeField] private float maxLifeTime = 1.5f;
	[SerializeField, Tooltip("When attacking point blank, the bullet struggles to detect the object due to the gun barrel. " +
	                         "This is the max distance the point can be from the spawn point to automatically deal damage.")] 
	private float pointDistance = 0.55f;

	private Vector3 prevPos;
	private GameObject target;
	private bool canDamage = false;
	private DamageType damageType;
	
	
	private void Start()
	{
		prevPos = transform.position;
		Destroy(gameObject, maxLifeTime);
	}

	private void Update()
	{
		CheckHit();
	}
	
	public void SetTarget(DamageType argType, GameObject argTarget, Vector3 argPoint)
	{
		damageType = argType;
		target = argTarget;
		canDamage = target.CompareTag("Damageable");

		if (canDamage && Vector3.Distance(transform.position, argPoint) < pointDistance)
		{
			DealDamage();
			Destroy(gameObject);
		}
	}

	private void CheckHit()
	{
		RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);
		foreach (RaycastHit hit in hits)
		{
			if (hit.transform.gameObject == target && !target.CompareTag("Passthrough"))
			{
				if (canDamage)
				{
					DealDamage();
				}
				Destroy(gameObject);
				break;
			}
			
		}
	}

	private void DealDamage()
	{
		canDamage = false;
		Damageable damageable = target.GetComponent<Damageable>();
		if (damageable)
		{
			damageable.TakeDamage(damageType);
		}
		else
		{
			// Get highest parent
			GameObject par = target;
			while (par.transform.parent != null)
			{
				par = par.transform.parent.gameObject;
			}

			damageable = par.GetComponent<Damageable>();
			if (damageable)
			{
				damageable.TakeDamage(damageType);
			}
			else
			{
				Debug.LogError("Object " + target.name + " is tagged as damageable but is missing the Damageable script");
			}
		}
	}
}
