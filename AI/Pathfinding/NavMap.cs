using UnityEngine;

public class NavMap : MonoBehaviour2D
{
    [SerializeField] private Vector2Int _size;
    [SerializeField] private LayerMask _wallsMask;
    [SerializeField] private float _cellSize;

    public Vector2Int Size => _size;
    public LayerMask WallksMask => _wallsMask;
    public float CellSize => _cellSize;
    public Vector2 StartPosition => Position2D;
}
