using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeHandler : MonoBehaviour
{
	[SerializeField] private GameObject frozenPrefab;
	[SerializeField] private GameObject shardPrefab;
	[SerializeField] private AudioClip[] freezeSounds;
	[SerializeField] private AudioSource audioSource;
	
	private List<Animation> frozen = new List<Animation>();

	public void Freeze()
	{
		frozen.Add(Instantiate(frozenPrefab, transform.position, transform.rotation, transform).gameObject.GetComponent<Animation>());
		audioSource.PlayOneShot(freezeSounds[0]);
		StartCoroutine(UnFreeze());
	}

	private IEnumerator UnFreeze()
	{
		yield return new WaitForSeconds(ReactionValues.FREEZE_TIME);
		if (frozen.Count == 1)
		{
			audioSource.PlayOneShot(freezeSounds[1]);
		}
		frozen[0].Play("UnFreeze");
		Instantiate(shardPrefab, transform.position, frozenPrefab.transform.rotation);
		while (frozen[0].isPlaying)
		{
			yield return null;
		}
		Animation anim = frozen[0];
		frozen.RemoveAt(0);
		Destroy(anim.gameObject);
	}
}
