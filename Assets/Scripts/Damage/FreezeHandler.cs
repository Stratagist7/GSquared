using System.Collections;
using UnityEngine;

public class FreezeHandler : MonoBehaviour
{
	[SerializeField] private GameObject frozenPrefab;
	[SerializeField] private GameObject shardPrefab;
	
	private Animation frozen;

	public void Freeze()
	{
		frozen = Instantiate(frozenPrefab, transform.position, transform.rotation, transform).gameObject.GetComponent<Animation>();
		StartCoroutine(UnFreeze());
	}

	private IEnumerator UnFreeze()
	{
		yield return new WaitForSeconds(ReactionValues.FREEZE_TIME);
		frozen.Play("UnFreeze");
		Instantiate(shardPrefab, transform.position, frozenPrefab.transform.rotation);
		while (frozen.isPlaying)
		{
			yield return null;
		}
		Destroy(frozen.gameObject);
	}
}
