using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private int damage;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Damageable.Player.GetComponent<PlayerHealth>().Damage(damage);
        }
    }
}
