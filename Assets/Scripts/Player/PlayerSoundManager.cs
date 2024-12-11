using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private AudioSource gunSource;
    
    [Header("Sound Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip emptyClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private AudioClip[] jumpClips;
    
    public static PlayerSoundManager instance;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }  else
            Destroy(gameObject);
        
        playerSource.clip = reloadClip;
    }

    private void ResetSettings()
    {
        playerSource.pitch = 1f;
        playerSource.volume = 1f;
    }

    public void PlayGunShot()
    {
        ResetSettings();
        gunSource.PlayOneShot(shootClip);
    }

    public void PlayEmptyClip()
    {
        ResetSettings();
        gunSource.PlayOneShot(emptyClip);
    }

    public void PlayReload()
    {
        gunSource.Stop();
        playerSource.volume = 0.8f;
        gunSource.Play();
    }

    public void PlayStep()
    {
        playerSource.pitch = Random.Range(0.9f, 1.1f);
        playerSource.volume = Random.Range(0.4f, 0.6f);
        playerSource.PlayOneShot(stepClips[Random.Range(0, stepClips.Length)]);
    }

    public void PlayJump()
    {
        playerSource.pitch = Random.Range(1f, 1.1f);
        playerSource.volume = Random.Range(0.4f, 0.6f);
        playerSource.PlayOneShot(jumpClips[Random.Range(0, jumpClips.Length)]);
    }
}
