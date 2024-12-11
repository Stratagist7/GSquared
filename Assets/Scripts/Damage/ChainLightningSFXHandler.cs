using UnityEngine;

public class ChainLightningSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    
    private static int lastPlayedClip = -1;

    private void Start()
    {
        audioSource.pitch = Random.Range(0.7f, 1.5f);
        audioSource.volume = Random.Range(0.9f, 1.5f);
        
        int clipIndex = Random.Range(0, clips.Length);
        while (clipIndex == lastPlayedClip)
        {
            clipIndex = Random.Range(0, clips.Length);
        }
        
        audioSource.PlayOneShot(clips[clipIndex]);
        lastPlayedClip = clipIndex;
        Destroy(gameObject, clips[clipIndex].length);
    }
}
