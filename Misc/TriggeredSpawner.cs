using UnityEngine;

public class TriggeredSpawner : MonoBehaviour2D
{
    [SerializeField] private Vector2 _size;

    public T Spawn<T>(T obj) where T : Object
    {
        return Instantiate(obj, RandomPositionInBox, default, null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _size);
    }

    private Vector2 RandomPositionInBox => new Vector2(Random.Range(-_size.x / 2, _size.x / 2), Random.Range(-_size.y / 2, _size.y / 2)) + Position2D;
}
