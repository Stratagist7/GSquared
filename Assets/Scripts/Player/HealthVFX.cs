using System.Collections;
using UnityEngine;

public class HealthVFX : MonoBehaviour
{
    [SerializeField] private PostEffectsController effect;
    private float duration = 0.15f;
    private float startRadius = 1.5f;
    private float endRadius = 0.8f;
    private float shieldFeather = 2f;
    private float dmgFeather = 1.2f;
    private float endFeather = 2.5f;

    private Color fadeDmgColor = Color.red;
    private Color fadeShieldColor = Color.cyan;
    private bool isAnimating = false;

    public IEnumerator PlayHealthVFX(bool argIsDmg)
    {
        if (isAnimating)
        {
            yield break;
        }
        isAnimating = true;
        effect.tintColor = argIsDmg ? fadeDmgColor : fadeShieldColor;
        effect.feather = argIsDmg ? dmgFeather : shieldFeather;
        
        // Fade in
        effect.radius = startRadius;
        float t = 0;
        while (t < duration)
        {
            effect.radius = Mathf.Lerp(startRadius, endRadius, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        effect.radius = endRadius;
        
        // Fade out
        t = 0;
        while (t < duration)
        {
            effect.radius = Mathf.Lerp(endRadius, startRadius, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        effect.radius = startRadius;
        effect.feather = endFeather;
        isAnimating = false;
    }
}
