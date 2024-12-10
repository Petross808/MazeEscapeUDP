using UnityEngine;
using UnityEngine.AI;

public class BlobAIScript : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints; 
    [SerializeField] private GameObject player;
    [SerializeField] private float chaseDuration = 5f; 
    [SerializeField] private float cooldownDuration = 3f; 
    [SerializeField] private float patrolSpeed = 3.5f; 
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float wanderRadius = 10f; 
    [SerializeField] private float wanderInterval = 1.5f; 

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float waitTimer = 0f;
    private float chaseTimer = 0f;
    private float cooldownTimer = 0f;
    private float wanderTimer = 0f;
    private bool isChasingPlayer = false;
    private bool isInCooldown = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints set for Blob patrol.");
        }
    }

    private void Update()
    {
        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else if (isInCooldown)
        {
            Wander();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.destination = waypoints[currentWaypointIndex].position;
            waitTimer = 0f;
            
        }

        agent.speed = patrolSpeed;
    }

    private void ChasePlayer()
    {
        chaseTimer += Time.deltaTime;

        if (chaseTimer <= chaseDuration)
        {
            agent.destination = player.transform.position;
            agent.speed = chaseSpeed;
        }
        else
        {
            isChasingPlayer = false;
            isInCooldown = true;
            chaseTimer = 0f;
            cooldownTimer = 0f;
            wanderTimer = 0f; 
        }
    }

    private void Wander()
    {
        cooldownTimer += Time.deltaTime;
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderInterval)
        {
            Vector3 wanderPoint = GetRandomPoint(transform.position, wanderRadius);

            if (NavMesh.SamplePosition(wanderPoint, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                agent.speed = patrolSpeed; 
            }

            wanderTimer = 0f; 
        }

        if (cooldownTimer >= cooldownDuration)
        {
            isInCooldown = false;
            GoToNextWaypoint();
        }
    }

    private Vector3 GetRandomPoint(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        return randomDirection;
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length > 0)
        {
            agent.destination = waypoints[currentWaypointIndex].position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player && !isInCooldown)
        {
            isChasingPlayer = true;
            chaseTimer = 0f;
        }
    }
}
