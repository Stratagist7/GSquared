using System.Collections;
using UnityEngine;

public class PillarMovement : MonoBehaviour
{
    [SerializeField] private float initialDelay;
    [SerializeField] private float duration;

    void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, -7.5f, transform.localPosition.z);
        StartCoroutine(StartUp());
    }

    private IEnumerator StartUp()
    {
        yield return new WaitForSeconds(initialDelay);
        StartCoroutine(MoveUp());
    }

    private IEnumerator MoveUp()
    {
        Vector3 startPos = new Vector3(transform.localPosition.x, -7.5f, transform.localPosition.z);
        Vector3 endPos = new Vector3(transform.localPosition.x, 3f, transform.localPosition.z);
        float t = 0f;

        while (t < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, t/duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPos;
        
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(MoveDown());
    }
    
    private IEnumerator MoveDown()
    {
        Vector3 startPos = new Vector3(transform.localPosition.x, 3f, transform.localPosition.z);
        Vector3 endPos = new Vector3(transform.localPosition.x, -7.5f, transform.localPosition.z);
        float t = 0f;

        while (t < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, t/duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPos;
        
        yield return new WaitForSeconds(7.5f);
        StartCoroutine(MoveUp());
    }

    
}
