//using Unity.Burst;
//using Unity.Collections;
//using Unity.Jobs;
//using Unity.Mathematics;
//using UnityEngine;

//public class DOTSAstar : MonoBehaviour
//{
//    [SerializeField] private NavMap _map;
//    [SerializeField] private Transform _test1;
//    [SerializeField] private Transform _test2;
//    private NativeArray<PathNode> _nodes;

//    private void Awake()
//    {
//        _nodes = new NativeArray<PathNode>(new PathNode[_map.Size.x * _map.Size.y], Allocator.Persistent);

//        for (int x = 0, length = _map.Size.x; x < length; x++) 
//        {
//            for (int y = 0, length2 = _map.Size.y; y < length2; y++)
//            {
//                _nodes[x + y * length] = new PathNode()
//                {
//                    IsWalkable = !Physics2D.OverlapCircle(new Vector2(x + _map.StartPosition.x, y + _map.StartPosition.y) * _map.CellSize, _map.CellSize),
//                    x = x,
//                    y = y
//                };
//            }
//        }
//    }

//    private void Update()
//    {
//        var path =  GetPath(_test1.position, _test2.position);
//        //Debug.LogError(path.Length);
//        //for (int i = 0, length = path.Length - 1; i < length; i++)
//        //{
//        //    Debug.DrawLine(path[i], path[i + 1]);
//        //}
//    }

//    public Vector2[] GetPath(Vector2 start, Vector2 end)
//    {
//        var startInGrid = Interpolate(start);
//        var endInGrid = Interpolate(end);

//        //NativeList<int2> path = new NativeList<int2>(Allocator.TempJob);
//        var pathJob = new FindPathJob()
//        {
//            start = new int2(startInGrid.x, startInGrid.y),
//            end = new int2(endInGrid.x, endInGrid.y),
//            size = new int2(_map.Size.x, _map.Size.y)
//        }.Schedule();

//        //pathJob.Complete();
//        //Debug.Log(path.Length);
//        //Vector2[] points = new Vector2[path.Length];
//        //for (int i = 0, length = points.Length; i < length; i++)
//        //{
//        //    var current = path[i];
//        //    points[i] = new Vector2(current.x * _map.CellSize, current.y * _map.CellSize) + _map.StartPosition;
//        //}

//        //path.Dispose();
        
//        return null;
//    }

//    private Vector2Int Interpolate(Vector2 worldPosition) => new Vector2Int(
//        Mathf.FloorToInt((worldPosition.x - _map.StartPosition.x) / _map.CellSize),
//        Mathf.FloorToInt((worldPosition.y - _map.StartPosition.y) / _map.CellSize));

//    [BurstCompile(DisableSafetyChecks = true)]
//    private struct FindPathJob : IJob
//    {
//        public int2 start;
//        public int2 end;
//        public int2 size;

//        public void Execute()
//        {
//            NativeArray<PathNode> nodes = new NativeArray<PathNode>(new PathNode[size.x * size.y], Allocator.Temp);

//            for (int y = 0, length = size.y; y < length; y++)
//            {
//                for (int x = 0, length2 = size.x; x < length2; x++)
//                {
//                    PathNode currentNode = new PathNode();
//                    currentNode.x = x;
//                    currentNode.y = y;
//                    currentNode.index = CalculateIndex(x, y, length2);
//                    currentNode.g = int.MaxValue;
//                    currentNode.h = CalculateDistance(new int2(x, y), end);

//                    currentNode.prevIndex = -1;
//                    currentNode.IsWalkable = true;
//                    nodes[currentNode.index] = currentNode;
//                }
//            }

//            NativeArray<int2> offset = new NativeArray<int2>(8, Allocator.Temp);
//            offset[0] = new int2(-1, 0);
//            offset[1] = new int2(1, 0);
//            offset[2] = new int2(-1, 1);
//            offset[3] = new int2(0, 1);
//            offset[4] = new int2(1, 1);
//            offset[5] = new int2(-1, -1);
//            offset[6] = new int2(0, -1);
//            offset[7] = new int2(1, -1);

//            int startIndex = CalculateIndex(start.x, start.y, size.x);
//            int endIndex = CalculateIndex(end.x, end.y, size.x);
//            PathNode startNode = nodes[startIndex];
//            startNode.g = 0;
//            nodes[startIndex] = startNode;

//            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
//            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

//            openList.Add(startNode.index);

//            while (openList.Length > 0)
//            {
//                int currentIndex = GetBestNode(openList, nodes);
//                PathNode currentNode = nodes[currentIndex];

//                if (startIndex == endIndex)
//                {
//                    break;
//                }

//                for (int i = 0, length = openList.Length; i < length; i++)
//                {
//                    if (openList[i] == currentIndex)
//                    {
//                        openList.RemoveAtSwapBack(i);
//                        break;
//                    }
//                }

//                closedList.Add(currentIndex);

//                for (int i = 0, length = 8; i < length; i++)
//                {
//                    int2 neighbour = offset[i];
//                    int2 position = new int2(neighbour.x + currentNode.x, neighbour.y + currentNode.y);

//                    if (!IsInGrid(position, size)) continue;

//                    int neighbourIndex = CalculateIndex(position.x, position.y, size.x);

//                    if (closedList.Contains(neighbourIndex)) continue;

//                    PathNode node = nodes[neighbourIndex];

//                    if (!node.IsWalkable) continue;

//                    int2 currentPosition = new int2(currentNode.x, currentNode.y);
//                    int cost = node.g + CalculateDistance(currentPosition, position);
//                    if (cost < node.g)
//                    {
//                        node.prevIndex = currentIndex;
//                        node.g = cost;
//                        nodes[neighbourIndex] = node;

//                        if (!openList.Contains(neighbourIndex))
//                        {
//                            openList.Add(neighbourIndex);
//                        }
//                    }
//                }
//            }

//            PathNode endNode = nodes[endIndex];
//            if (endNode.prevIndex != -1)
//            {
//                NativeList<int2> path = BacktrackPath(nodes, endNode);

//                //foreach (var p in path)
//                //{
//                //    Debug.LogError(p.x + " " + p.y);
//                //}
//                path.Dispose();
//            }
//            else
//            {
//                Debug.Log("FUck you");
//            }

//            nodes.Dispose();
//            openList.Dispose();
//            closedList.Dispose();
//            offset.Dispose();
//        }

//        private NativeList<int2> BacktrackPath(NativeArray<PathNode> nodes, PathNode end)
//        {
//            if (end.prevIndex == -1)
//            {
//                return new NativeList<int2>(Allocator.Temp);
//            }

//            PathNode current = end;
//            NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
//            path.Add(new int2(current.x, current.y));

//            while (current.prevIndex != -1)
//            {
//                PathNode prev = nodes[current.prevIndex];
//                path.Add(new int2(prev.x, prev.y));
//                current = prev;
//            }

//            return path;
//        }
//        public bool IsInGrid(int2 position, int2 size)
//        {
//            return position.x >= 0 && position.y >= 0 && position.x < size.x && position.y < size.y;
//        }

//        private int GetBestNode(NativeList<int> openList, NativeArray<PathNode> nodes)
//        {
//            PathNode best = nodes[openList[0]];
//            for (int i = 1, length = openList.Length; i < length; i++)
//            {
//                var node = nodes[openList[i]];
//                if (best.f > node.f)
//                {
//                    best = node;
//                }
//            }
//            return best.index;
//        }

//        private int CalculateIndex(int x, int y, int width) => x + y * width;
//        private int CalculateDistance(int2 element1, int2 element2)
//        {
//            int xDist = Mathf.Abs(element1.x - element2.x);
//            int yDist = Mathf.Abs(element1.y - element2.y);
//            int remaing = Mathf.Abs(xDist - yDist);
//            return 14 * Mathf.Min(xDist, yDist) + 10 * remaing;
//        }
//    }

//    private struct PathNode
//    {
//        public int x;
//        public int y;

//        public int index;
//        public int prevIndex;
//        public bool IsWalkable;
//        public int g;
//        public int h;
//        public int f => g + h;
//    }
//}
