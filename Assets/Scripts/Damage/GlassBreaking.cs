using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlassBreaking : Hitable
{
	[SerializeField] private MeshFilter filter;
	[SerializeField] private Collider disableColl;
	[SerializeField] private GameObject disableObject;
	[SerializeField] private Collider[] enableColliders;
	[SerializeField] private Mesh newMesh;
	[Space]
	[SerializeField] private GameObject tempText;
	
	public override void TakeDamage(DamageType argType, int argDamage = -1)
	{
		base.TakeDamage(argType, argDamage);

		if (_curHealth <= 0)
		{
			filter.mesh = newMesh;
			if (disableColl != null)
			{
				disableColl.enabled = false;
			}
			
			if (disableObject != null)
			{
				disableObject.SetActive(false);
			}

			foreach (Collider coll in enableColliders)
			{
				coll.enabled = false;
			}
			
			tag = "Untagged";
			StartCoroutine(DisplayTempText());
		}
	}

	private IEnumerator DisplayTempText()
	{
		tempText.SetActive(true);
		yield return new WaitForSeconds(1f);
		tempText.SetActive(false);
		Destroy(this);
	}
}
