using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BlobAIScript : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints; 
    [SerializeField] private Transform player;
    [SerializeField] private int chaseTickDuration = 50; 
    [SerializeField] private int cooldownTickDuration = 30; 
    [SerializeField] private float patrolSpeed = 3.5f; 
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float wanderRadius = 10f; 
    [SerializeField] private int wanderTickDuration = 15;

    [SerializeField] private GooScript _gooPrefab;
    [SerializeField] private int _gooAmount;
    [SerializeField] private int _gooTickDuration;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private int chaseTimer = 0;
    private int cooldownTimer = 0;
    private int wanderTimer = 0;
    private bool isChasingPlayer = false;
    private bool isInCooldown = false;

    private List<GooScript> _gooTrail = new();
    private int _currentGooIndex = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints set for Blob patrol.");
        }
    }

    [EventSignature]
    public void Tick(GameEvent.CallbackContext _)
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
        if (!agent.isOnNavMesh) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        agent.speed = patrolSpeed;
    }

    private void ChasePlayer()
    {
        chaseTimer++;

        if (chaseTimer <= chaseTickDuration)
        {
            agent.destination = player.position;
            agent.speed = chaseSpeed;
        }
        else
        {
            isChasingPlayer = false;
            isInCooldown = true;
            chaseTimer = 0;
            cooldownTimer = 0;
            wanderTimer = 0; 
        }
    }

    private void Wander()
    {
        cooldownTimer++;
        wanderTimer++;

        if (wanderTimer >= wanderTickDuration)
        {
            Vector3 wanderPoint = GetRandomPoint(transform.position, wanderRadius);

            if (NavMesh.SamplePosition(wanderPoint, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                agent.speed = patrolSpeed; 
            }

            wanderTimer = 0; 
        }

        if (cooldownTimer >= cooldownTickDuration)
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

    [EventSignature(typeof(GameObject))]
    public void ChasePlayer(GameEvent.CallbackContext context)
    {
        if (context.Get<GameObject>().transform.root == player && !isInCooldown)
        {
            isChasingPlayer = true;
            chaseTimer = 0;
        }
    }

    [EventSignature]
    public void DropGoo(GameEvent.CallbackContext _)
    {
        if (_gooAmount == 0) return;

        _currentGooIndex = (_currentGooIndex + 1) % _gooAmount;
        
        while(_gooTrail.Count <= _currentGooIndex)
        {
            GooScript goo = Instantiate<GooScript>(_gooPrefab);
            goo.gameObject.SetActive(false);
            _gooTrail.Add(goo);
        }

        _gooTrail[_currentGooIndex].Spawn(transform.position, _gooTickDuration);
    }
}
