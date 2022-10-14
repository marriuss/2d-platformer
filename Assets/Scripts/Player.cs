using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement))]
public class Player : MonoBehaviour
{
    [SerializeField] private UnityEvent _attacked;
    [SerializeField] private UnityEvent _gotHit;
    [SerializeField] private KeyCode _jumpKey;

    private const KeyCode DefaultJumpKey = KeyCode.Space;

    private Movement _movement;
    private bool _hasControl;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    private void Start()
    {
        _hasControl = true;
    }

    private void Update()
    {
        if (_hasControl)
        {
            if (Input.GetKeyDown(_jumpKey))
                _movement.Jump();

            _movement.Move(Input.GetAxisRaw("Horizontal"));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;

        if (collider.TryGetComponent<Enemy>(out _))
            _gotHit?.Invoke();
    }

    private void OnValidate()
    {
        if (_jumpKey == KeyCode.None)
            _jumpKey = DefaultJumpKey;
    }

    private void TurnOnControl() => _hasControl = true;

    private void TurnOffControl() => _hasControl = false;

}