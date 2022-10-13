using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _offset;

    private void Update()
    {
        Vector3 offset = new Vector3(_offset.x, _offset.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, _target.position + offset, _speed * Time.deltaTime);
    }
}
