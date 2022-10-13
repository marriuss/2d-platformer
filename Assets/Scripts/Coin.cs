using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] private UnityEvent _collected;

    private const float DestructionDelay = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out _))
        {
            _collected?.Invoke();
            Destroy(gameObject, DestructionDelay);
        }
    }
}
