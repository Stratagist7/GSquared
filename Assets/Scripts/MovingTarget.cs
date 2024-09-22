using System;
using UnityEngine;
using UnityEngine.AI;

public class MovingTarget : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject[] locations;
    private int index = 0;

    private void Start()
    {
        GoToNextLocation();
    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextLocation();
        }
    }

    private void GoToNextLocation()
    {
        agent.SetDestination(locations[index].transform.position);
        index = (index + 1) % locations.Length;
    }
}
