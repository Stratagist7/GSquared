using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private PostEffectsController effect;
    private float fadeDuration = 1f;
    private Color fadeColor = Color.black;
    private float startRadius = 0f;
    private float endRadius = 1.5f;
    private float startFeather = 0f;
    private float endFeather = 2.5f;

    private float moveSpeed;
    private float sprintSpeed;
    private StarterAssets.FirstPersonController fps;
    
    void Start()
    {
        fps = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.FirstPersonController>();
        if (fps != null)
        {
            moveSpeed = fps.MoveSpeed;
            sprintSpeed = fps.SprintSpeed;
        }

        fps.MoveSpeed = 0f;
        fps.SprintSpeed = 0f;
        
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        effect.tintColor = fadeColor;
        effect.radius = startRadius;
        effect.feather = startFeather;
        float t = 0;
        while (t < fadeDuration)
        {
            effect.radius = Mathf.Lerp(startRadius, endRadius, t / fadeDuration);
            effect.feather = Mathf.Lerp(startFeather, endFeather, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        effect.radius = endRadius;
        effect.feather = endFeather;
        fps.MoveSpeed = moveSpeed;
        fps.SprintSpeed = sprintSpeed;
        
    }
}
