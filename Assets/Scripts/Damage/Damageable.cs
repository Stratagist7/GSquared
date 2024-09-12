using UnityEngine;

[RequireComponent(typeof(Collider), typeof(HealthBar))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private HealthBar healthBar;

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

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
