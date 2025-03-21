using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;
    [SerializeField] private HealthVFX healthVFX;
    
    private const int MAX_HEALTH = 100;
    private const float DURATION = 0.15f;
    private int health;
    private int shields;
    
    private readonly Queue<Action> hitPoints = new Queue<Action>();
    private bool isAnimating;
    public static bool playerIsDead;
    
    private void Awake()
    {
        health = MAX_HEALTH;
        healthBar.fillAmount = 1;
        
        shields = 0;
        shieldBar.fillAmount = 0;
        
        isAnimating = false;
        playerIsDead = false;
    }

    private void Update()
    {
        if (hitPoints.Count > 0 && isAnimating == false)
        {
            hitPoints.Dequeue()();
        }
#if DEBUG       
        if (Input.GetKeyDown(KeyCode.H))
        {
            Damage(5);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Damage(10);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Shield(5);
        }
#endif
    }

    public void Damage(int argDamage)
    {
        // no shields? just hurt
        if (shields <= 0)
        {
            hitPoints.Enqueue(() => StartCoroutine(ChangeHealth(-argDamage)));
        }
        else
        {
            // shields cover damage? bye bye shields
            if (shields >= argDamage)
            {
                Shield(-argDamage);
            }
            else
            {
                // shields & hurt necessary
                int extraDamage = argDamage - shields;
                Shield(-shields);
                hitPoints.Enqueue(() => StartCoroutine(ChangeHealth(-extraDamage, true)));
            }
        }
    }

    public void Shield(int argShields)
    {
        // Don't waste q time when full on shields
        if (argShields > 0 && shields >= MAX_HEALTH)
        {
            return;
        }
        
        hitPoints.Enqueue(() => StartCoroutine(ChangeShields(argShields)));
    }

    private IEnumerator ChangeHealth(int argDifference, bool argIsSplitDmg = false)
    {
        isAnimating = true;
        if (healthVFX != null && argIsSplitDmg == false && playerIsDead == false)
        {
            StartCoroutine(healthVFX.PlayHealthVFX(true));
        }

        float time = 0;
        float startValue = health;
        float endValue = health + argDifference;
        
        while (time < DURATION)
        {
            float value = Mathf.Lerp(startValue, endValue, time / DURATION);
            health = (int)value;
            healthBar.fillAmount = value / MAX_HEALTH;
            
            time += Time.deltaTime;
            yield return null;
        }
        health = (int)endValue;
        healthBar.fillAmount = endValue / MAX_HEALTH;
        isAnimating = false;
        
        if (health <= 0)
        {
            playerIsDead = true;
            MenuUI.instance.ShowDeathScreen();
        }
    }
    
    private IEnumerator ChangeShields(int argDifference)
    {
        isAnimating = true;
        if (argDifference < 0)
        {
            StartCoroutine(healthVFX.PlayHealthVFX(true));
        }

        float time = 0;
        float startValue = shields;

        float endValue = shields + argDifference > MAX_HEALTH ? MAX_HEALTH : shields + argDifference < 0 ? 0 : shields + argDifference;
        
        while (time < DURATION)
        {
            float value = Mathf.Lerp(startValue, endValue, time / DURATION);
            shields = (int)value;
            shieldBar.fillAmount = value / MAX_HEALTH;
            
            time += Time.deltaTime;
            yield return null;
        }
        shields = (int)endValue;
        shieldBar.fillAmount = endValue / MAX_HEALTH;
        isAnimating = false;
    }
}
