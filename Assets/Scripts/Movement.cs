using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private UnityEvent _movementLeftStarted;
    [SerializeField] private UnityEvent _movementRightStarted;
    [SerializeField] private UnityEvent _movementStopped;
    [SerializeField] private UnityEvent _jumped;

    private const float MinMovementDistance = 0.001f;
    private const float MinSpeed = 1;
    private const float MinJumpHeight = 1;
    private const float GroundNormalY = 0.9f;
    private const float ShellRadius = 0.001f;

    private Rigidbody2D _rigidbody;
    private ContactFilter2D _contactFilter;
    private Coroutine _movementCoroutine;
    private Vector2 _velocity;
    private bool _isGrounded;

    public bool IsMovingHorizontally => _velocity.x != 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.useLayerMask = true;
        _contactFilter.SetLayerMask(LayerMask.GetMask("Default"));
        _velocity = Vector2.zero;
        _isGrounded = true;
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.deltaTime;
        _velocity.y += Physics2D.gravity.y * deltaTime;
        MoveRigidbody(_velocity.y * deltaTime * Vector2.up);
    }

    private void OnValidate()
    {
        if (_speed < MinSpeed)
            _speed = MinSpeed;

        if (_jumpHeight < MinJumpHeight)
            _jumpHeight = MinJumpHeight;
    }

    public void Move(float distance)
    {
        StopMovement();
        _movementCoroutine = StartCoroutine(MoveHorizontally(distance));
    }

    public void StopMovement()
    {
        Stop();

        if (_movementCoroutine != null)
            StopCoroutine(_movementCoroutine);
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _velocity.y = _jumpHeight;
            _jumped?.Invoke();
        }
    }

    private void Stop()
    {
        _movementStopped?.Invoke();
        _velocity.x = 0;
    }

    private IEnumerator MoveHorizontally(float distance)
    {
        if (distance == 0)
            yield return null;

        bool isLeftDirection = distance < 0;

        if (isLeftDirection)
        {
            _movementLeftStarted?.Invoke();
        }
        else
        {
            _movementRightStarted?.Invoke();
        }

        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        distance = Mathf.Abs(distance);
        float directionKoefficient = isLeftDirection ? -1.0f : 1.0f;
        float distanceMovedTotally = 0.0f;
        float distanceMoved;
        bool isMoving;

        do
        {
            _velocity.x = Mathf.MoveTowards(distanceMovedTotally, distance, 1.0f) * directionKoefficient * _speed;
            Vector2 deltaPosition = _velocity * Time.deltaTime;
            distanceMoved = MoveRigidbody(Vector2.right * deltaPosition.x);
            distanceMovedTotally += distanceMoved;
            isMoving = distanceMovedTotally < distance && distanceMoved > MinMovementDistance;
            yield return waitForFixedUpdate;
        }
        while (isMoving);

        Stop();
    }

    private float MoveRigidbody(Vector2 movement)
    {
        float distance = movement.magnitude;
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        _rigidbody.Cast(movement, _contactFilter, hits, distance + ShellRadius);

        foreach(RaycastHit2D hit in hits)
        {
            _isGrounded = hit.normal.y >= GroundNormalY;

            float modifiedDistance = hit.distance - ShellRadius;
            distance = modifiedDistance < distance ? modifiedDistance : distance;
        }

        _rigidbody.position += movement.normalized * distance;
        return distance;
    }
}
