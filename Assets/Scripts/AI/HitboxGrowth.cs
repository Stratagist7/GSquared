using System.Collections;
using UnityEngine;

public class HitboxGrowth : MonoBehaviour
{
    [SerializeField] private Vector3 startScale;
    [SerializeField] private Vector3 endScale;
    [SerializeField] private float duration;
    
    void Start()
    {
        StartCoroutine(GrowHitbox());
    }

    private IEnumerator GrowHitbox()
    {
        float t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
    }
}
