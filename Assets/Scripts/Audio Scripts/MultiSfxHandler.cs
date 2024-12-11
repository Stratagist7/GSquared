using UnityEngine;
using UnityEngine.Serialization;

public class MultiSfxHandler : EffectSfxHandler
{
    [SerializeField] private bool repeatSfx = false;
    [SerializeField] private AudioClip[] clips;
    private int currentClip;
    private static int lastPlayedClip = -1;

    protected override void Start()
    {
        currentClip = Random.Range(0, clips.Length);
        if (repeatSfx == false)
        {
            while (currentClip == lastPlayedClip)
            {
                currentClip = Random.Range(0, clips.Length);
            }

            lastPlayedClip = currentClip;
        }

        clip = clips[currentClip];
        base.Start();
    }
}
