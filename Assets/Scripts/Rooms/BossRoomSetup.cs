using UnityEngine;

public class BossRoomSetup : MonoBehaviour
{
    [SerializeField] private BossBehavior[] bosses;
    [SerializeField] private PillarMovement[] pillars;
    [SerializeField] private bool startInRoom = false;

    private void Start()
    {
        if (startInRoom)
        {
            Setup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Setup();
        }
    }

    private void Setup()
    {
        foreach (BossBehavior boss in bosses)
        {
            boss.settingUp = false;
        }

        foreach (PillarMovement pillar in pillars)
        {
            pillar.Setup();
        }

        Destroy(gameObject);
    }
}
