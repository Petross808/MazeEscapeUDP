
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ScarecrowAIScript : MonoBehaviour, ISaveData
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _unfreezeDelayTime;
    [SerializeField] private Collider _visibleBounds;
    [SerializeField] private LayerMask _blockingSight;
    [SerializeField] private Collider _hitbox;

    [SerializeField, EventSignature] GameEvent _onActivateEvent;
    [SerializeField, EventSignature] GameEvent _onUnfreezeStartedEvent;
    [SerializeField, EventSignature] GameEvent _onUnfreezeEvent;

    private NavMeshAgent _agent;
    private float _unfreezeTimer;
    private bool _activated;

    public bool Activated { get => _activated; set => _activated = value; }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _hitbox.enabled = false;
        _activated = false;
    }

    private void FixedUpdate()
    {
        if (CheckIfVisible())
        {
            if(!_activated)
            {
                _activated = true;
                _onActivateEvent.Raise(this);
            }
            _agent.isStopped = true;
            _unfreezeTimer = 0;
            _hitbox.enabled = false;
            return;
        }

        if(!_activated)
        {
            return;
        }

        if (_unfreezeTimer < _unfreezeDelayTime)
        {
            _unfreezeTimer += Time.deltaTime;

            if(_unfreezeTimer >= _unfreezeDelayTime/2 && !_hitbox.enabled)
            {
                _onUnfreezeStartedEvent.Raise(this);
                _hitbox.enabled = true;
            }

            if (_unfreezeTimer >= _unfreezeDelayTime)
            {
                _onUnfreezeEvent.Raise(this);
            }
            return;
        }

        _agent.isStopped = false;
        _hitbox.enabled = true;
    }

    private bool CheckIfVisible()
    {
        Plane[] frustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool isInFrustum = GeometryUtility.TestPlanesAABB(frustum, _visibleBounds.bounds);

        if (!isInFrustum)
            return false;

        Vector3[] points = new Vector3[8];
        points[0] = _visibleBounds.bounds.min;
        points[1] = _visibleBounds.bounds.max;
        points[2] = new Vector3(points[0].x, points[0].y, points[1].z);
        points[3] = new Vector3(points[0].x, points[1].y, points[0].z);
        points[4] = new Vector3(points[1].x, points[0].y, points[0].z);
        points[5] = new Vector3(points[0].x, points[1].y, points[1].z);
        points[6] = new Vector3(points[1].x, points[0].y, points[1].z);
        points[7] = new Vector3(points[1].x, points[1].y, points[0].z);

        Vector3 camPosition = Camera.main.transform.position;

        foreach(var point in points)
        {
            if(!Physics.Linecast(point, camPosition, _blockingSight))
            {
                return true;
            }
        }

        return false;
    }

    [EventSignature]
    public void UpdatePath(GameEvent.CallbackContext _)
    {
        if (!_activated) return;

        NavMeshPath path = new();
        if(_agent.CalculatePath(_player.transform.position, path))
            _agent.SetPath(path);
    }

    [EventSignature]
    public void Stop(GameEvent.CallbackContext _)
    {
        _agent.isStopped = true;
        _unfreezeTimer = 0;
        _hitbox.enabled = false;
    }

    public void LoadData(SaveData data)
    {
        this._agent = data.enemyScarecrowAgent;
        this._activated = data.enemyScarecrowActivated;
    }

    public void SaveData(ref SaveData data)
    {
        data.enemyScarecrowAgent = this._agent;
        data.enemyScarecrowActivated = this._activated;
    }
}
