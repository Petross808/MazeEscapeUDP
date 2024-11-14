using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovementScript : MonoBehaviour, IControllable
{
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private Transform _head;
    [SerializeField] private float _walkHeadOffset;

    private NavMeshAgent _agent;
    private Vector3 _moveDirection;
    private bool _isWalking;

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
            _head.transform.Translate(0, -_walkHeadOffset, 0);
        else
            _head.transform.Translate(0, _walkHeadOffset, 0);
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

            _agent.Move(moveVector * Time.deltaTime);
        }
        else
        {
            _agent.isStopped = true;
        }
    }
}
