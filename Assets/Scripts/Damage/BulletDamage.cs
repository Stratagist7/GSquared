using UnityEngine;

public class BulletDamage : MonoBehaviour
{
	[SerializeField] private int damage = 5;
	[SerializeField] private float maxLifeTime = 1.5f;
	private float lifeTime;

	private Vector3 prevPos;
	
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

	private void CheckHit()
	{
		RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);
		foreach (RaycastHit hit in hits)
		{
			if (hit.transform.gameObject.CompareTag("Damageable"))
			{
				Damageable damageable = hit.transform.GetComponent<Damageable>();
				if (damageable)
				{
					damageable.TakeDamage(damage);
				}
				else
				{
					// Get highest parent
					GameObject par = hit.transform.gameObject;
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
						Debug.LogError("Object " + hit.transform.name + " is tagged as damageable but is missing the Damageable script");
					}
				}
			}
			Destroy(gameObject);
			break;
		}
	}
}
