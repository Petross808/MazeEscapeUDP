using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BlobAIScript : MonoBehaviour, ISaveData
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

        if (!agent.pathPending && agent.hasPath && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        agent.speed = patrolSpeed;
        agent.isStopped = false;
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
            CreateGoo();
        }

        _gooTrail[_currentGooIndex].Spawn(transform.position, _gooTickDuration);
    }

    public GooScript CreateGoo()
    {
        GooScript goo = Instantiate<GooScript>(_gooPrefab);
        goo.gameObject.SetActive(false);
        _gooTrail.Add(goo);
        return goo;
    }


    [EventSignature]
    public void Stop(GameEvent.CallbackContext _)
    {
        agent.isStopped = true;
    }

    public void LoadData(SaveData data)
    {
        currentWaypointIndex = data.blob_currentWaypointIndex;
        chaseTimer = data.blob_chaseTimer;
        cooldownTimer = data.blob_cooldownTimer;
        wanderTimer = data.blob_wanderTimer;
        isChasingPlayer = data.blob_isChasingPlayer;
        isInCooldown = data.blob_isInCooldown;
        agent.Warp(data.blob_position);
        transform.rotation = data.blob_rotation;
        agent.isStopped = data.blob_isStopped;
        _currentGooIndex = data.blob_currentGooIndex;

        foreach(GooScript goo in _gooTrail)
        {
            goo.gameObject.SetActive(false);
        }

        for(int i = 0; i < data.blob_gooTrail.Count; i++)
        {
            if(i >= _gooTrail.Count)
            {
                CreateGoo();
            }

            if(data.blob_gooTrail[i].dur > 0)
            {
                _gooTrail[i].Spawn(data.blob_gooTrail[i].pos, data.blob_gooTrail[i].dur);
            }
            else
            {
                _gooTrail[i].gameObject.SetActive(false);
            }
        }
    }

    public void SaveData(ref SaveData data)
    {
        data.blob_currentWaypointIndex = currentWaypointIndex;
        data.blob_chaseTimer = chaseTimer;
        data.blob_cooldownTimer = cooldownTimer;
        data.blob_wanderTimer = wanderTimer;
        data.blob_isChasingPlayer = isChasingPlayer;
        data.blob_isInCooldown = isInCooldown;
        data.blob_position = agent.nextPosition;
        data.blob_rotation = transform.rotation;
        data.blob_isStopped = agent.isStopped;
        data.blob_currentGooIndex = _currentGooIndex;

        if (data.blob_gooTrail == null)
            data.blob_gooTrail = new();
        else
            data.blob_gooTrail.Clear();

        foreach (GooScript goo in _gooTrail)
        {
            data.blob_gooTrail.Add(new() { pos = goo.transform.position, dur = goo.Duration });
        }
    }
}
