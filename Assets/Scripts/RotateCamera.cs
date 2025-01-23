using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private Vector3 minAngle;
    [SerializeField] private Vector3 maxAngle;
    [SerializeField] private float turnDuration;
    [SerializeField] private float pauseDuration;

    private void Start()
    {
        StartCoroutine(MinToMax());
    }

    private IEnumerator MinToMax()
    {
        Quaternion start = Quaternion.Euler(minAngle);
        Quaternion end = Quaternion.Euler(maxAngle);
        float t = 0f;

        while (t < turnDuration)
        {
            transform.rotation = Quaternion.Slerp(start, end, t / turnDuration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = end;
        yield return new WaitForSeconds(pauseDuration);
        StartCoroutine(MaxToMin());
    }

    private IEnumerator MaxToMin()
    {
        Quaternion start = Quaternion.Euler(maxAngle);
        Quaternion end = Quaternion.Euler(minAngle);
        float t = 0f;

        while (t < turnDuration)
        {
            transform.rotation = Quaternion.Slerp(start, end, t / turnDuration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = end;
        yield return new WaitForSeconds(pauseDuration);
        StartCoroutine(MinToMax());
    }
}
