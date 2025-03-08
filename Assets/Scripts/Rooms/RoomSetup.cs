using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class RoomSetup : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnterRoom;
    [SerializeField] private bool startInRoom = false;

    private void Start()
    {
        if (startInRoom)
        {
            onEnterRoom.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onEnterRoom.Invoke();
        }
    }
}
