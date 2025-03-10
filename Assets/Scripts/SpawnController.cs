using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public bool shouldSpawn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldSpawn = !shouldSpawn;
        }
    }

    public void SetShouldSpawn(bool argShouldSpawn)
    {
        shouldSpawn = argShouldSpawn;
    }
}
