using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingTarget : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject[] locations;
    private int index = 0;

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(locations[index].transform.position);
            index = (index + 1) % 2;
        }
    }
}
