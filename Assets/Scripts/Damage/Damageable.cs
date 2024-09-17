using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(HealthBar))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private HealthBar healthBar;
    private Dictionary<DamageType, float> types = new Dictionary<DamageType, float>();
    private const float MAX_TYPE_TIME = 5f;

    private int _curHealth;
    private int curHealth
    {
        get => _curHealth;
        set
        {
            _curHealth = value;
            healthBar.SetHealth(_curHealth);
        }
    }

    private void Start()
    {
        healthBar = GetComponent<HealthBar>();
        curHealth = maxHealth;
        tag = "Damageable";
    }

    private void Update()
    {
        UpdateTypes();
    }

    private void UpdateTypes()
    {
        if (types.Count <= 0)
        {
            return;
        }
        
        List<DamageType> keyList = new List<DamageType>(types.Keys);
        foreach (DamageType type in keyList)
        {
            if (Time.time - types[type] > MAX_TYPE_TIME)
            {
                types.Remove(type);
            }
        }
    }

    public void TakeDamage(DamageType argType, int argDamage)
    {
        curHealth -= argDamage;
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (argType == DamageType.None)
        {
            return;
        }
        
        types[argType] = Time.time;
        if (argType == DamageType.Fire)
        {
            StartCoroutine(OnFire());
        }
    }

    private IEnumerator OnFire()
    {
        float stopTime = types[DamageType.Fire] + MAX_TYPE_TIME;
        while (Time.time < stopTime)
        {
            yield return new WaitForSeconds(0.5f);
            TakeDamage(DamageType.None, 2);
        }
    }
}

public enum DamageType {None, Earth, Fire, Ice, Lightning, Nature, Water};
