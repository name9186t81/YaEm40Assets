using UnityEngine;

public class PathNode : IGridElement
{
    private Vector2 _worldPosition;
    private Vector2Int _gridPosition;
    private Grid<PathNode> _grid;

    public PathNode(Grid<PathNode> grid, Vector2Int gridPosition, Vector2 worldPostion, float cellSize, LayerMask walls)
    {
        _grid = grid;
        _gridPosition = gridPosition;
        _worldPosition = worldPostion;

        IsWalkable = !Physics2D.OverlapCircle(worldPostion, cellSize, walls);
    }

    public bool IsWalkable { get; private set; }
    public int g;
    public int h;
    public int f => g + h;

    public PathNode _previous;
    public Vector2 WorldPosition { get => _worldPosition; set => _worldPosition = value; }
    public Vector2Int GridPosition { get => _gridPosition; set => _gridPosition = value; }
}
