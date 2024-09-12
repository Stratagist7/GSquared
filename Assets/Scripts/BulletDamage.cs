using UnityEngine;

public class BulletDamage : MonoBehaviour
{
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
				// TODO: deal damage
			}
			Destroy(gameObject);
			break;
		}
	}
}
