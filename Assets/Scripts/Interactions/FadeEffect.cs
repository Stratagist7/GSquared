using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class FadeEffect : Interactable
{
    [SerializeField] private PostEffectsController effect;
    
    [SerializeField] private GameObject[] activateObjects;
    [SerializeField] private GameObject[] deactivateObjects;
    [Space] 
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private float startRadius = 1.5f;
    [SerializeField] private float endRadius = 0f;
    [SerializeField] private float startFeather = 2.5f;
    [SerializeField] private float endFeather = 0f;
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
        effect.tintColor = fadeColor;
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

        if (activateObjects.Length > 0)
        {
            foreach (GameObject obj in activateObjects)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in deactivateObjects)
            {
                obj.SetActive(false);
            }
            MenuUI.instance.FreezePlayer(true);
        }
        else
        {
            SceneManager.LoadScene("MainLevel");
        }
        
    }

}
