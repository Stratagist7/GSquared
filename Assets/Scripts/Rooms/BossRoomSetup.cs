using UnityEngine;

public class BossRoomSetup : MonoBehaviour
{
    [SerializeField] private BossBehavior[] bosses;
    [SerializeField] private bool startInRoom = false;

    private void Start()
    {
        if (startInRoom)
        {
            foreach (BossBehavior boss in bosses)
            {
                boss.settingUp = false;
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (BossBehavior boss in bosses)
            {
                boss.settingUp = false;
            }

            Destroy(gameObject);
        }
    }
}
