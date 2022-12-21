using UnityEngine;

public class FABRIK
{
    private FABRIKPoint[] _points;
    private FABRIKPoint _start;
    private FABRIKPoint _end;
    private float _chainLength;

    public FABRIK(FABRIKPoint[] points)
    {
        _points = points;

        for (int i = 1, length = points.Length; i < length; i++)
        {
            points[i].PreviousPoint = points[i - 1];

            points[i].PreviousDistance = Vector2.Distance(points[i - 1].Position, points[i].Position);
            _chainLength += points[i].PreviousDistance;
        }

        _start = points[0];
        _end = points[points.Length - 1];
    }

    public void Solve(Vector2 point)
    {
        if(Vector2.Distance(_start.Position, point) > _chainLength)
        {
            SolveForOutOfReachPoint(point);
            return;
        }

        Vector2 startOriginalPosition = _start.Position;
        _end.Position = point;

        for(int i = _points.Length - 2; i >= 0; i--)
        {
            float distance = _points[i + 1].PreviousDistance;
            Vector2 direction = _points[i + 1].Position.GetDirection(_points[i].Position);

            _points[i].Position = direction * distance + startOriginalPosition;
        }

        _start.Position = startOriginalPosition;

        for(int i = 1, length = _points.Length; i < length; i++)
        {
            Vector2 direction = _points[i].Position.GetDirection(_points[i - 1].Position);
            float distance = _points[i].PreviousDistance;

            _points[i].Position = direction * distance + startOriginalPosition;
        }

        _end.Position = _end.PreviousDistance * point.GetDirection(_points[_points.Length - 2].Position) + _points[_points.Length - 2].Position;
    }

    private void SolveForOutOfReachPoint(Vector2 point)
    {
        Vector2 direction = point.GetDirection(_start.Position);
        float sumLength = 0f;

        for(int i = 0, length = _points.Length; i < length; i++)
        {
            sumLength += _points[i].PreviousDistance;
            _points[i].Position = direction * sumLength + _start.Position;
        }
    }
}
