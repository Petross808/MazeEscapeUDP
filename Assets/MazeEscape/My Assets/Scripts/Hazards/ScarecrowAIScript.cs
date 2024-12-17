using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ScarecrowAIScript : MonoBehaviour, ISaveData
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _unfreezeDelayTime;
    [SerializeField] private Collider _visibleBounds;
    [SerializeField] private float _visibilityDistance;
    [SerializeField] private LayerMask _blockingSight;
    [SerializeField] private Collider _hitbox;
    [SerializeField] private int _loseAggroAfterRepathAttempts;

    [SerializeField] private GameObject _activeModel;
    [SerializeField] private GameObject _inactiveModel;

    [SerializeField, EventSignature] GameEvent _onActivateEvent;
    [SerializeField, EventSignature] GameEvent _onUnfreezeStartedEvent;
    [SerializeField, EventSignature] GameEvent _onUnfreezeEvent;

    private NavMeshAgent _agent;
    private float _unfreezeTimer;
    private bool _activated;
    private Vector3 _homePosition;
    private Vector3 _homeRotation;
    private int _aggroTimer;

    public bool Activated { get => _activated; 
        set
        {
            if(!value)
            {
                _activeModel.SetActive(value);
                _inactiveModel.SetActive(!value);
            }
            _activated = value;
        }
    }

    private void Awake()
    {
        _aggroTimer = 0;
        _homePosition = transform.position;
        _homeRotation = transform.forward;
        _agent = GetComponent<NavMeshAgent>();
        _hitbox.enabled = false;
        Activated = false;
    }

    private void FixedUpdate()
    {
        if (CheckIfVisible())
        {
            if(!Activated)
            {
                Activated = true;
                _onActivateEvent.Raise(this);
            }
            _agent.isStopped = true;
            _unfreezeTimer = 0;
            _hitbox.enabled = false;
            return;
        }

        if(!Activated)
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
                _activeModel.SetActive(true);
                _inactiveModel.SetActive(false);
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

        if(Vector3.Distance(Camera.main.transform.position, _visibleBounds.bounds.center) > _visibilityDistance)
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
        if (!Activated) return;

        _agent.SetDestination(_player.transform.position);
        if(_agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            _agent.SetDestination(_homePosition);
            _aggroTimer++;
        }

        if(_loseAggroAfterRepathAttempts > 0 && _aggroTimer >= _loseAggroAfterRepathAttempts)
        {
            _aggroTimer = 0;
            Activated = false;
            _agent.Warp(_homePosition);
            transform.forward = _homeRotation;
            _agent.isStopped = true;
            _unfreezeTimer = 0;
            _hitbox.enabled = false;
        }
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
        this.Activated = data.enemyScarecrowActivated;
        this._agent.Warp(data.enemyScarecrowPosition);
        this._agent.gameObject.transform.rotation = data.enemyScarecrowRotation;
}

    public void SaveData(ref SaveData data)
    {
        data.enemyScarecrowActivated = this.Activated;
        data.enemyScarecrowPosition = this._agent.nextPosition;
        data.enemyScarecrowRotation = this._agent.transform.rotation;
    }
}
