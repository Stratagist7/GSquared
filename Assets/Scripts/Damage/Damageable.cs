using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider), typeof(HealthBar))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private HealthBar healthBar;
    private NavMeshAgent agent;
    private float normSpeed;
    
    private Dictionary<DamageType, float> types = new Dictionary<DamageType, float>();
    private const float MAX_TYPE_TIME = 5f;
    private const float SLOW_MULTIPLIER = 0.5f;
    
    private bool isSlowed = false;
    private bool isBurning = false;
    
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
        agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            normSpeed = agent.speed;
        }

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
        switch (argType)
        {
            case DamageType.Fire:
                if (!isBurning)
                {
                    StartCoroutine(Burning());
                }
                break;
            case DamageType.Ice:
                if (!isSlowed)
                {
                    StartCoroutine(Slowed());
                }
                break;
        }
        
    }

    private IEnumerator Burning()
    {
        isBurning = true;
        while (types.ContainsKey(DamageType.Fire))
        {
            yield return new WaitForSeconds(0.25f);
            TakeDamage(DamageType.None, 1);
        }
        isBurning = false;
    }

    private IEnumerator Slowed()
    {
        if (!agent)
        {
            yield break;
        }
        isSlowed = true;
        agent.speed = normSpeed * SLOW_MULTIPLIER;
        while (types.ContainsKey(DamageType.Ice))
        {
            yield return null;
        }
        agent.speed = normSpeed;
        isSlowed = false;
    }
}

public enum DamageType {None, Earth, Fire, Ice, Lightning, Nature, Water};
