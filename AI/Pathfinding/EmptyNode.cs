using UnityEngine;

public class EmptyNode : IGridElement
{
    private Vector2 worldPosition;
    private Vector2Int gridPosition;
    public Vector2 WorldPosition { get => worldPosition; set => worldPosition = value; }
    public Vector2Int GridPosition { get => gridPosition; set => gridPosition = value; }
}
