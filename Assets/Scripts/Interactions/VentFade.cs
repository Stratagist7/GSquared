using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class VentFade : Interactable
{
    [SerializeField] private PostEffectsController effect;
    [SerializeField] private float fadeDuration = 0.5f;
    void Start()
    {
        interactAction = StartFade;
    }

    private void StartFade(Collider other)
    {
        interactText.SetActive(false);
        StarterAssets.FirstPersonController fps = other.GetComponent<StarterAssets.FirstPersonController>();
        fps.MoveSpeed = 0f;
        fps.SprintSpeed = 0f;
        
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        MenuUI.instance.UnlockCursor(true); // REMOVE WHEN ADDING NEW SCENE TO MOVE TO
        effect.tintColor = Color.black;
        float t = 0;
        float startRadius = 1.5f;
        float endRadius = 0;

        float startFeather = 2.5f;
        float endFeather = 0;
        while (t < fadeDuration)
        {
            effect.radius = Mathf.Lerp(startRadius, endRadius, t / fadeDuration);
            effect.feather = Mathf.Lerp(startFeather, endFeather, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        effect.radius = endRadius;
        effect.feather = endFeather;
        // MOVE TO NEW SCENE
    }

}
