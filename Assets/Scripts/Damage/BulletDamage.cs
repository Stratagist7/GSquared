using UnityEngine;

public class BulletDamage : MonoBehaviour
{
	[SerializeField] private int damage = 5;
	[SerializeField] private float maxLifeTime = 1.5f;
	private float lifeTime;

	private Vector3 prevPos;
	private GameObject target;
	private bool canDamage = true;
	
	private void Start()
	{
		lifeTime = 0f;
		prevPos = transform.position;
	}

	private void Update()
	{
		CheckHit();
		
		lifeTime += Time.deltaTime;
		if (lifeTime >= maxLifeTime)
		{
			Destroy(gameObject);
		}
		prevPos = transform.position;
	}
	
	public void SetTarget(GameObject argTarget)
	{
		target = argTarget;
		canDamage = target.CompareTag("Damageable");
	}

	private void CheckHit()
	{
		RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);
		foreach (RaycastHit hit in hits)
		{
			print(hit.collider.name);
			if (hit.transform.gameObject == target && canDamage)
			{
				DealDamage();
				Destroy(gameObject);
				break;
			}
			
		}
	}

	private void DealDamage()
	{
		Damageable damageable = target.GetComponent<Damageable>();
		if (damageable)
		{
			damageable.TakeDamage(damage);
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
				damageable.TakeDamage(damage);
			}
			else
			{
				Debug.LogError("Object " + target.name + " is tagged as damageable but is missing the Damageable script");
			}
		}
	}
}
