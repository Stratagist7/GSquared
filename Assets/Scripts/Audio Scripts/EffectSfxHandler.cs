using UnityEngine;

public class EffectSfxHandler : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip clip;
    [SerializeField] protected bool manualPlay;
    [Space] 
    [SerializeField] private float pitchMin;
    [SerializeField] private float pitchMax;
    [SerializeField] private float volumeMin;
    [SerializeField] private float volumeMax;

    private void Start()
    {
        if (manualPlay == false)
        {
            SetAudioSettings();
            audioSource.PlayOneShot(clip);
            Destroy(gameObject, clip.length);
        }
    }

    protected virtual void SetAudioSettings()
    {
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, volumeMax);
    }

    public void PlayAudio()
    {
        SetAudioSettings();
        audioSource.PlayOneShot(clip);
        if (manualPlay == false)
        {
            Destroy(gameObject, clip.length);
        }
    }
}
