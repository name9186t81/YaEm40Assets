using System;
using UnityEngine;

public sealed class Grid<T> where T : IGridElement
{
    public readonly int Height;
    public readonly int Width;
    public readonly float CellSize;
    private readonly T[,] _grid;
    public readonly Vector2 WorldPositiob;

    public Grid(int height, int width, float cellSize, Vector2 worldPosition, Func<Grid<T>, Vector2Int, Vector2, T> createFunction)
    {
        Height = height;
        Width = width;
        CellSize = cellSize;
        WorldPositiob = worldPosition;

        _grid = new T[width, Height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _grid[x, y] = createFunction.Invoke(this, new Vector2Int(x, y), Extrapolate(x, y));
            }
        }
    }

    public bool IsInGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    public bool IsInGrid(Vector2 worldPosition)
    {
        var interpolated = Interpolate(worldPosition);
        return IsInGrid(interpolated.x, interpolated.y);
    }
    public T GetCell(int x, int y) => _grid[x, y];
    public T GetCell(Vector2Int position) => GetCell(position.x, position.y);
    public Vector2 Extrapolate(int x, int y) => new Vector2(x * CellSize, y * CellSize) + WorldPositiob;
    public Vector2 Extrapolate(Vector2Int vector) => Extrapolate(vector.x, vector.y);
    public Vector2Int Interpolate(Vector2 worldPosition) => new Vector2Int(
        Mathf.FloorToInt((worldPosition.x - WorldPositiob.x) / CellSize),
        Mathf.FloorToInt((worldPosition.y - WorldPositiob.y) / CellSize));
}
