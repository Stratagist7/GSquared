using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// Opens the gate once the specified enemies have been destroyed
public class GateControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private float openGateTime;
    [SerializeField] private MeshRenderer gatePad;
    [SerializeField] private Material gatePadMat;
    [SerializeField] private int matIndex;
    [Space]
    [SerializeField] private AudioSource source;

    bool open = false;

    private void Update()
    {
        if (open || enemies.All(obj => obj == null) == false)
        {
            return;
        }
        
        StartCoroutine(OpenGate());
    }


    private IEnumerator OpenGate()
    {
        open = true;
        source.Play();
        Material[] mats = gatePad.materials;
        mats[matIndex] = gatePadMat;
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
}
