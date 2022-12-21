using UnityEngine;

//public class GridTesting : MonoBehaviour
//{
//    [SerializeField] private Transform _tracked;
//    private Grid<WeightedNode> _grid;

//    private void Start()
//    {
//        _grid = new Grid<WeightedNode>(15, 25, 1f, transform.position, CreateNode);
//    }

//    private WeightedNode CreateNode()
//    {
//        return new WeightedNode();
//    }

//    private void Update()
//    {
//        _grid.DebugDrawGrid();
//        if (_grid.IsInGrid(_tracked.position))
//        {
//            var interpolated = _grid.Interpolate(_tracked.position);
//            var extrapolate = _grid.Extrapolate(interpolated.x, interpolated.y);
//            var cell = _grid.GetCell(interpolated);
//            Debug.DrawLine(extrapolate, extrapolate + Vector2.one * _grid.CellSize);
//            cell.Value = Random.Range(0, 1f);
//        }
//    }
//}
