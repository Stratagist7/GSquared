using System.Collections;
using UnityEngine;

public class FreezeHandler : MonoBehaviour
{
	private const string UNFREEZE_NAME = "UnFreeze";
	
	[SerializeField] private GameObject frozenPrefab;
	[SerializeField] private GameObject shardPrefab;
	[SerializeField] private AudioClip[] freezeSounds;
	[SerializeField] private AudioSource audioSource;
	
	private Animation frozen;
	private float unfreezeTime = 0f;

	public void Freeze()
	{
		if (unfreezeTime < Time.time)
		{
			frozen = Instantiate(frozenPrefab, transform.position, transform.rotation, transform).gameObject.GetComponent<Animation>();
			audioSource.PlayOneShot(freezeSounds[0]);
			unfreezeTime = Time.time + ReactionValues.FREEZE_TIME;
			StartCoroutine(UnFreeze());
		}
		else
		{
			unfreezeTime = Time.time + ReactionValues.FREEZE_TIME;
		}
		
	}

	private IEnumerator UnFreeze()
	{
		while (unfreezeTime > Time.time)
		{
			yield return null;
		}
		
		audioSource.PlayOneShot(freezeSounds[1]);
		frozen.Play(UNFREEZE_NAME);
		Instantiate(shardPrefab, transform.position, frozenPrefab.transform.rotation);
		while (frozen.isPlaying)
		{
			yield return null;
		}
		Destroy(frozen.gameObject);
	}
}
