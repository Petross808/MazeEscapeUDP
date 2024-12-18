using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovementScript : MonoBehaviour, IControllable, ISaveData
{
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private Transform _head;
    [SerializeField] private float _walkHeadOffset;
    [SerializeField] private float _runningRadius;
    [SerializeField] private float _walkingRadius;
    [SerializeField] private float _stepDistance;
    [SerializeField, EventSignature] private GameEvent _onRunStepEvent;
    [SerializeField, EventSignature] private GameEvent _onWalkStepEvent;


    private NavMeshAgent _agent;
    private Vector3 _moveDirection;
    private bool _isWalking;
    float _distanceTravelled = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _moveDirection = Vector3.zero;
        _isWalking = false;
    }

    public void SetMoveDirection(Vector2 dir)
    {
        _moveDirection = new Vector3(dir.x, 0, dir.y).normalized;
    }

    public void SetWalking(bool isWalking)
    {
        if(isWalking == _isWalking) return;

        _isWalking = isWalking;
        if (_isWalking)
        {
            _head.transform.Translate(0, -_walkHeadOffset, 0);
            _agent.radius = _walkingRadius;
        }
        else
        {
            _head.transform.Translate(0, _walkHeadOffset, 0);
            _agent.radius = _runningRadius;
        }
    }

    private void Update()
    {
        if (_moveDirection != Vector3.zero)
        {
            Vector3 moveVector = 0.1f * (_isWalking ? _walkSpeed : _runSpeed) * _moveDirection;

            // Rotate move vector to the direction the player is facing
            Vector3 forward = Vector3.ProjectOnPlane(_head.transform.forward, Vector3.up);
            float angle = Vector3.SignedAngle(Vector3.forward, forward, Vector3.up);
            moveVector = Quaternion.AngleAxis(angle, Vector3.up) * moveVector;
            moveVector *= Time.deltaTime;

            _agent.Move(moveVector);

            _distanceTravelled += moveVector.magnitude;
            if(_distanceTravelled > _stepDistance)
            {
                _distanceTravelled = 0;
                if (_isWalking)
                    _onWalkStepEvent.Raise(this);
                else
                    _onRunStepEvent.Raise(this);
            }
        }
        else
        {
            _agent.isStopped = true;
        }
    }

    public void LoadData(SaveData data)
    {
        this._head.localRotation = data.playerHeadRotation;
        this._agent.Warp(data.playerPosition);
        this._agent.gameObject.transform.rotation = data.playerRotation;
    }

    public void SaveData(ref SaveData data)
    {
        data.playerHeadRotation = this._head.localRotation;
        data.playerPosition = this._agent.nextPosition;
        data.playerRotation = this._agent.gameObject.transform.rotation;
    }
}
