using UnityEngine;

public interface IGridElement
{
    Vector2 WorldPosition { get; set; }
    Vector2Int GridPosition { get; set; }
}
