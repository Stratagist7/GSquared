using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Transform healthBar;
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        healthBar.LookAt(player.position);
    }

    public void SetHealth(float health)
    {
        if (healthSlider)
        {
            healthSlider.value = health;
        }
    }
}
