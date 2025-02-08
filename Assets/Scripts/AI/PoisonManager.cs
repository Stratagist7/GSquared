using UnityEngine;

public class PoisonManager : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float cooldown;
    private float lastTimeDamaged = 0;
    void Update()
    {
        if (PoisonDamage.playerInside > 0)
        {
            if (Time.time - lastTimeDamaged >= cooldown)
            {
                Damageable.Player.GetComponent<PlayerHealth>().Damage(damage);
                lastTimeDamaged = Time.time;
            }
        }
        else
        {
            lastTimeDamaged = 0;
        }
    }
}
