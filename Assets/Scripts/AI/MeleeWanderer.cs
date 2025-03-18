using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeWanderer : MeleeChaser
{
    [SerializeField] private Transform[] locations;
    [SerializeField] private float playerDetectionRadius;
    [SerializeField, Tooltip("Range of 0 to 1")] private float changeLocationChance;
    [SerializeField] private float wanderStoppingDistance = 0.3f;
    [SerializeField] private float defaultStoppingDistance = 2f;
    [SerializeField] private PullRadius pr;

    private int currentLocation = 0;
    private bool wandering = false;
    private bool isAlert = false;
    
    protected override void Update()
    {
        if (settingUp || agent.isOnNavMesh == false)
        {
            return;
        }
// CHECK IF OTHER SPIDERS SEE PLAYER
        if (isAlert || Vector3.Distance(transform.position, Damageable.Player.transform.position) <= playerDetectionRadius)
        {
            agent.stoppingDistance = defaultStoppingDistance;
            dist = Mathf.Pow(defaultStoppingDistance, 2);;
            AlertNearby();
            base.Update();
        }
        else if (agent.enabled && agent.remainingDistance <= agent.stoppingDistance && wandering == false)
        {
            if (Random.value < changeLocationChance)
            {
                ChangeLocation();
            }
            else
            {
                StartCoroutine(Wander());
            }
        }
        
        
        if (animator != null)
        {
            animator.SetBool(MOVE_KEY, agent.enabled && agent.remainingDistance > agent.stoppingDistance);
            animator.SetBool(SLOW_KEY, isSlowed);
        }
    }

    private void ChangeLocation()
    {
        currentLocation = (currentLocation + 1) % locations.Length;
        agent.SetDestination(locations[currentLocation].position);
    }

    private IEnumerator Wander()
    {
        wandering = true;
        agent.stoppingDistance = wanderStoppingDistance;
        Vector3 finalPosition = Vector3.zero;

        while (finalPosition == Vector3.zero)
        {
            float radius = Random.Range(0.5f, 1.0f);
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1)) {
                finalPosition = hit.position;
            }
        }
        
        agent.SetDestination(finalPosition);
        yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
        wandering = false;
    }

    private void AlertNearby()
    {
        isAlert = true;
        foreach (PullRadius p in pr.inRange)
        {
            MeleeWanderer wanderer = p.agent as MeleeWanderer;
            if (wanderer == null)
            {
                return;
            }
            wanderer.isAlert = true;
        }
    }
}
