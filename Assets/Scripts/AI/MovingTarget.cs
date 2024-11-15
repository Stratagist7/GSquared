using System;
using UnityEngine;
using UnityEngine.AI;

public class MovingTarget : MoveableAgent
{
    
    [SerializeField] private GameObject[] locations;
    private int index = 0;

    protected override void Start()
    {
        base.Start();
        GoToNextLocation();
    }

    private void Update()
    {
        if (agent.enabled && agent.remainingDistance <= agent.stoppingDistance)
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
