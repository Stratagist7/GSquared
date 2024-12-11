using UnityEngine;

public class EffectSfxHandler : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip clip;
    [Space] 
    [SerializeField] private float pitchMin;
    [SerializeField] private float pitchMax;
    [SerializeField] private float volumeMin;
    [SerializeField] private float volumeMax;

    protected virtual void Start()
    {
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, volumeMax);
        
        audioSource.PlayOneShot(clip);
        Destroy(gameObject, clip.length);
    }
}
