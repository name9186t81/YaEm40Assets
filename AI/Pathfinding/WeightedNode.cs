using UnityEngine;

public class WeightedNode : IGridElement
{
    public float Value;
    private Vector2 _worldPos;
    private Vector2Int _gridPosition;

    public Vector2 WorldPosition { get => _worldPos; set => _worldPos = value; }
    public Vector2Int GridPosition { get => _gridPosition; set => _gridPosition = value; }
}
