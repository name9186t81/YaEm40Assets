using System.Collections.Generic;
using UnityEngine;

public class Astar : AstarAbstract
{
    [SerializeField] private NavMap _map;
    [SerializeField] private bool _debug;
    private List<PathNode> _closedNodes;
    private Grid<PathNode> _grid;

    private void Awake()
    {
        _grid = new Grid<PathNode>(_map.Size.y, _map.Size.x, _map.CellSize, transform.position, 
            (Grid<PathNode> grid, Vector2Int gridPos, Vector2 worldPos) => new PathNode(grid, gridPos, worldPos, _map.CellSize, _map.WallksMask));
        //_main = Camera.main;
    }

    private void Update()
    {
        if(_debug)
            _grid.DebugDrawGrid();
    }

    public override Vector2[] GetPath(Vector2 start, Vector2 end)
    {
        if (!_grid.IsInGrid(start) || !_grid.IsInGrid(end))
        {
            start = _grid.Extrapolate(ClampToGrid(start));
            end = _grid.Extrapolate(ClampToGrid(end));
        }

        var path = GetPath(_grid.Interpolate(start), _grid.Interpolate(end));
        if (path == null)
        {
            return null;
        }
        Vector2[] pathVect = new Vector2[path.Count];

        for (int i = 0, length = pathVect.Length; i < length; i++)
        {
            pathVect[i] = path[i].WorldPosition;
        }

        return pathVect;
    }
    private List<PathNode> GetPath(Vector2Int start, Vector2Int end)
    {
        List<PathNode> openNodes = new List<PathNode>();
        _closedNodes = new List<PathNode>();

        var startNode = _grid.GetCell(start);
        var endNode = _grid.GetCell(end);
        openNodes.Add(startNode);

        for (int y = 0; y < _map.Size.y; y++)
        {
            for (int x = 0; x < _map.Size.x; x++)
            {
                var node = _grid.GetCell(x, y);
                node.g = int.MaxValue;
                node.h = 0;
                node._previous = null;
            }
        }

        startNode.g = 0;
        startNode.h = CalculateDistance(startNode, endNode);

        while (openNodes.Count > 0)
        {
            PathNode current = GetBestNode(openNodes);

            if (current == endNode)
            {
                return BackTrackPath(current, startNode);
            }

            openNodes.Remove(current);
            _closedNodes.Add(current);

            foreach (PathNode node in GetNeighbours(current))
            {
                if (!_closedNodes.Contains(node))
                {
                    int cost = current.g + CalculateDistance(current, node);

                    if (cost < node.g)
                    {
                        node._previous = current;
                        node.g = cost;
                        node.h = CalculateDistance(node, endNode);

                        if (!openNodes.Contains(node))
                        {
                            openNodes.Add(node);
                        }
                    }
                }
            }
        }

        return null;
    }

    private Vector2Int ClampToGrid(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, 0, _grid.Width - 1);
        pos.y = Mathf.Clamp(pos.y, 0, _grid.Height - 1);
        return new Vector2Int((int)pos.x, (int)pos.y);
    }
    private List<PathNode> GetNeighbours(PathNode node)
    {
        var nodePos = node.GridPosition;
        var result = new List<PathNode>();
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if ((x == 0 && y == 0) ||
                    !_grid.IsInGrid(nodePos.x + x, nodePos.y + y) ||
                    !_grid.GetCell(nodePos.x + x, nodePos.y + y).IsWalkable)
                {
                    continue;
                }

                result.Add(_grid.GetCell(nodePos.x + x, nodePos.y + y));
            }
        }

        return result;
    }
    private List<PathNode> BackTrackPath(PathNode node, PathNode started)
    {
        List<PathNode> nodes = new List<PathNode>();
        nodes.Add(node);
        PathNode current = node;

        while (current._previous != null)
        {
            nodes.Add(current._previous);
            current = current._previous;
        }

        nodes.Reverse();
        return nodes;
    }
    private int CalculateDistance(IGridElement element1, IGridElement element2)
    {
        int xDist = Mathf.Abs(element1.GridPosition.x - element2.GridPosition.x);
        int yDist = Mathf.Abs(element1.GridPosition.y - element2.GridPosition.y);
        int remaing = Mathf.Abs(xDist - yDist);
        return 14 * Mathf.Min(xDist, yDist) + 10 * remaing;
    }

    private PathNode GetBestNode(List<PathNode> nodes)
    {
        PathNode best = nodes[0];
        for (int i = 1, length = nodes.Count; i < length; i++)
        {
            if (best.f > nodes[i].f)
            {
                best = nodes[i];
            }
        }
        return best;
    }
}
