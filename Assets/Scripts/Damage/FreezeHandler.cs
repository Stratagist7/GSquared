using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeHandler : MonoBehaviour
{
    [SerializeField] private GameObject freezeParticles;
    [SerializeField] private Material freezeMaterial;
    private MeshRenderer[] renderers;
    private List<Material> defaultMats = new List<Material>();
    
    private const float FREEZE_TIME = 0.5f;
    private const float MELT_TIME = 0.25f;
    
    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in renderers)
        {
            defaultMats.Add(ren.material);
        }
    }

    public IEnumerator Freeze()
    {
        Vector3 dirToPlayer = transform.position - Damageable.Player.transform.position;
        Vector3 spawnPos = transform.position;
        if (Mathf.Abs(dirToPlayer.x) > Mathf.Abs(dirToPlayer.z))
        {
            spawnPos += dirToPlayer.x > 0 ? Vector3.left : Vector3.right;
        }
        else
        {
            spawnPos += dirToPlayer.z > 0 ? Vector3.back : Vector3.forward;
        }
        
        Instantiate(freezeParticles, spawnPos, freezeParticles.transform.rotation);
        
        yield return new WaitForSeconds(FREEZE_TIME);
        foreach (MeshRenderer ren in renderers)
        {
            ren.material = freezeMaterial;
        }
        
        StartCoroutine(UnFreeze());
    }

    private IEnumerator UnFreeze()
    {
        yield return new WaitForSeconds(ReactionValues.FREEZE_TIME - FREEZE_TIME);
        float time = 0;

        while (time < MELT_TIME)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.Lerp(freezeMaterial, defaultMats[i], time / MELT_TIME);
            }
            time += Time.deltaTime;
            yield return null;
        }
        
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = defaultMats[i];
        }
        
    }
}
