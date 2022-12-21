using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAstar : AstarAbstract
{
    [SerializeField] private GOPathNode[] _nodes;
    [SerializeField] private LayerMask _walls;
    [SerializeField] private Transform _test1;
    [SerializeField] private Transform _test2;
    private void Awake()
    {
        for (int i = 0, length = _nodes.Length; i < length; i++)
        {
            GOPathNode currentNode = _nodes[i];
            for (int j = 0; j < length; j++)
            {
                if (j == i)
                {
                    continue;
                }
                GOPathNode checkNode = _nodes[j];
                float distance = Vector2.Distance(currentNode.Position2D, checkNode.Position2D);
                if (Physics2D.Raycast(currentNode.Position2D, (checkNode.Position2D - currentNode.Position2D) / distance, distance, _walls))
                {
                    continue;
                }

                currentNode.AddConnection(checkNode);
            }
        }
    }

    private void Update()
    {
        //for (int i = 0, length = _nodes.Length; i < length; i++)
        //{
        //    for (int j = 0, length2 = _nodes[i].Connections.Count; j < length2; j++)
        //    {
        //        Debug.DrawLine(_nodes[i].Position2D, _nodes[i].Connections[j].node.Position2D);
        //    }
        //}
        //var path = GetPath(_test1.position, _test2.position);
        //Debug.LogError(path.Length);
        //for (int i = 0, length = path.Length - 1; i < length; i++)
        //{
        //    Debug.DrawLine(path[i], path[i + 1], Color.blue);
        //}
        //Debug.DrawLine(_test1.position, path[0], Color.blue);
    }

    private GOPathNode FindClosestNode(Vector2 worldPosition)
    {
        GOPathNode best = default;
        float dist = float.MaxValue;

        for (int i = 0, length = _nodes.Length; i < length; i++)
        {
            float currentDistance = Vector2.Distance(_nodes[i].Position2D, worldPosition);

            if (currentDistance > dist || Physics2D.Raycast(worldPosition, (_nodes[i].Position2D - worldPosition) / currentDistance, currentDistance, _walls))
            {
                continue;
            }

            best = _nodes[i];
            dist = currentDistance;
        }

        return best;
    }

    public override Vector2[] GetPath(Vector2 start, Vector2 end)
    {
        var startNode = FindClosestNode(start);
        var endNode = FindClosestNode(end);
		
        if (startNode == null || endNode == null)
        {
            return null;
        }

        List<GOPathNode> openList = new List<GOPathNode>();
        HashSet<GOPathNode> closedList = new HashSet<GOPathNode>();

        for (int i = 0, length = _nodes.Length; i < length; i++)
        {
            _nodes[i].G = int.MaxValue;
            _nodes[i].H = 0;
            _nodes[i].Previous = null;
        }

        openList.Add(startNode);
        startNode.G = 0;
        startNode.H = GetDistance(startNode.Position2D, endNode.Position2D);

        while (openList.Count > 0)
        {
            GOPathNode current = GetBestNode(openList);

            if (current == endNode)
            {
                var path = BackTrackPath(endNode);
                path.Reverse();
                path.Add(end);
                return path.ToArray();
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (var connection in current.Connections)
            {
                if (closedList.Contains(connection.node))
                {
                    continue;
                }

                float cost = current.G + connection.distance;
                if (cost < connection.node.G)
                {
                    connection.node.G = cost;
                    connection.node.Previous = current;
                    connection.node.H = GetDistance(connection.node.Position2D, endNode.Position2D);

                    if (!openList.Contains(connection.node))
                    {
                        openList.Add(connection.node);
                    }
                }
            }
        }

        return null;
    }

    private List<Vector2> BackTrackPath(GOPathNode endNode)
    {
        List<Vector2> path = new List<Vector2>();
        GOPathNode node = endNode;
        path.Add(node.Position2D);

        while (node.Previous != null)
        {
            path.Add(node.Previous.Position2D);
            node = node.Previous;
        }

        return path;
    }

    private float GetDistance(Vector2 left, Vector2 right)
    {
        float x = Mathf.Abs(left.x - right.x);
        float y = Mathf.Abs(left.y - right.y);
        return x + y;
    }
    private GOPathNode GetBestNode(List<GOPathNode> nodes)
    {
        GOPathNode node = nodes[0];
        for (int i = 1, length = nodes.Count; i < length; i++)
        {
            if (node.F > nodes[i].F)
            {
                node = nodes[i];
            }
        }
        return node;
    }
}
