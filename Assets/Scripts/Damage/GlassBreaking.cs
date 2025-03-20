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
	[SerializeField] private ParticleSystem particles;
	[SerializeField] private AudioClip sfx;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private MeshRenderer mRenderer;
	
	public override void TakeDamage(DamageType argType, int argDamage = -1)
	{
		// don't do anything if already broken
		if (_curHealth <= 0)
		{
			return;
		}
		
		base.TakeDamage(argType, argDamage);

		if (_curHealth <= 0)
		{
			audioSource.PlayOneShot(sfx);
			particles.Play();
			if (newMesh != null)
			{
				filter.mesh = newMesh;
			}

			if (mRenderer != null)
			{
				Material[] mats = new Material[2];
				mats[0] = mRenderer.materials[0];
				mats[1] = mRenderer.materials[1];
				mRenderer.materials = mats;
			}

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
		}
	}

}
