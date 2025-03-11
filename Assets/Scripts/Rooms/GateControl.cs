using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// Opens the gate once the specified enemies have been destroyed
public class GateControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private float openGateTime;
    [SerializeField] private MeshRenderer gatePad;
    [SerializeField] private int matIndex;
    [SerializeField] private Material gatePadOpenMat;
    [SerializeField] private Material gatePadClosedMat;
    [Space]
    [SerializeField] private AudioSource source;
    [SerializeField] private bool manualControl = false;

    private bool open = false;

    private void Update()
    {
        if (manualControl || open || enemies.All(obj => obj == null) == false)
        {
            return;
        }
        
        StartCoroutine(OpenGate());
    }

    public void MoveGate(bool argShouldOpen)
    {
        if (argShouldOpen)
        {
            StartCoroutine(OpenGate());
        }
        else
        {
            StartCoroutine(CloseGate());
        }
    }
    
    public void MoveGate(bool argShouldOpen, bool argShouldPlayAudio)
    {
        if (argShouldOpen)
        {
            StartCoroutine(OpenGate(argShouldPlayAudio));
        }
        else
        {
            StartCoroutine(CloseGate(argShouldPlayAudio));
        }
    }


    private IEnumerator OpenGate(bool argShouldPlayAudio = true)
    {
        open = true;
        if (argShouldPlayAudio)
        {
            source.Play();
        }

        Material[] mats = gatePad.materials;
        mats[matIndex] = gatePadOpenMat;
        gatePad.materials = mats;
        
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, 2.5f, 0);

        while (t < openGateTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t / openGateTime);
            t += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPos;
    }
    
    private IEnumerator CloseGate(bool argShouldPlayAudio = true)
    {
        open = false;
        if (argShouldPlayAudio)
        {
            source.Play();
        }
        
        Material[] mats = gatePad.materials;
        mats[matIndex] = gatePadClosedMat;
        gatePad.materials = mats;
        
        float t = 0;
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y + 180f, 0));
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos - new Vector3(0, 2.5f, 0);

        while (t < openGateTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t / openGateTime);
            t += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPos;
    }
}
