using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _patrolingRadius;

    private const float MinPatrolingRadius = 1;
    private const float StanSeconds = 3f;

    private int _enemyLayer;
    private int _backgroundEnemyLayer;
    private float _direction;
    private bool _isPatrolling;
    private Movement _movement;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    private void Start()
    {
        _direction = 1;
        _isPatrolling = true;
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _backgroundEnemyLayer = LayerMask.NameToLayer("BackgroundEnemy");
    }

    private void Update()
    {
        if (_isPatrolling && _movement.IsMovingHorizontally == false)
            Patrol();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out _))
            StartCoroutine(GetStunned());
    }

    private void OnValidate()
    {
        if (_patrolingRadius < MinPatrolingRadius)
            _patrolingRadius = MinPatrolingRadius;
    }

    private IEnumerator GetStunned()
    {
        _isPatrolling = false;
        _movement.StopMovement();
        gameObject.layer = _backgroundEnemyLayer;

        yield return new WaitForSeconds(StanSeconds);

        gameObject.layer = _enemyLayer;
        _isPatrolling = true;
    }

    private void Patrol()
    {
        _movement.Move(_direction * _patrolingRadius);
        _direction *= -1;
    }
}
