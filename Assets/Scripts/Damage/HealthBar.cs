using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Transform healthBar;
    [SerializeField] private bool facePlayer = true;

    private void Update()
    {
        if (facePlayer)
        {
            healthBar.LookAt(Damageable.Player.transform.position);
        }
    }

    public void SetHealth(float health)
    {
        if (healthSlider)
        {
            healthSlider.value = health;
        }
    }

    public void SetMaxHealth(float health)
    {
        if (healthSlider)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
    }
}
