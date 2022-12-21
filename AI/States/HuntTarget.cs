using UnityEngine;

public class HuntTarget : IWeightState<AIController>
{
    private Vector2[] _path;
    private AIController _controller;
    private AIVision _vision;
    private TargetMapProvider _targetMaps;
    private AstarProvider _astar;
    private int _currentIndex = 0;
    private const float UPDATEPATHFREQUANCY = 3f;
    private Timer _delay;
    private Color _debug;
    private LayerMask _walls;

    public HuntTarget(LayerMask walls)
    {
        _walls = walls;
    }

    public float CalculateEffectivness()
    {
        //return ((!_vision.ScanResults.TryGetValue(ScannedUnitType.Enemy, out var list) && (list == null || list.Count == 0)) && _path != null) ? 20f : -3f;
        return (_targetMaps.GetRandomTarget(_controller.AttachedUnit.teamNumber, out _) || _path != null) ? 0.9f : -100f;
    }

    public void Execute()
    {
        if (_path == null)
        {
            _currentIndex = 0;
            return;
        }


        for (int i = 0, length = _path.Length; i < length - 1; i++)
        {
            Debug.DrawLine(_path[i], _path[i + 1], _debug);
        }


        if (_currentIndex >= _path.Length)
        {
            _path = null;
            _currentIndex = 0;
            return;
        }

        Debug.DrawLine(_controller.AttachedUnit.Position2D, _path[_currentIndex], _debug);
        float distance = Vector2.Distance(_path[_currentIndex], _controller.AttachedUnit.Position2D);

        if (Physics2D.Raycast(_controller.AttachedUnit.Position2D, _path[_currentIndex] - _controller.AttachedUnit.Position2D, distance, _walls))
        {
            _path = null;
            return;
        }

        if (distance < _controller.AttachedUnit.Size * 2)
        {
            _currentIndex++;

            if (_currentIndex >= _path.Length)
            {
                _path = null;
                _currentIndex = 0;
                return;
            }
        }

        _controller.LookAtPoint(_path[_currentIndex]);
        _controller.SafeWalk(_path[_currentIndex]);
    }

    public void Init(AIController owner)
    {
        _debug = Random.ColorHSV(0f, 1f, 1, 1, 1, 1, 1, 1);
        _controller = owner;
        _vision = owner.Vision;
        owner.OnUpdate += Update;
        _targetMaps = ServiceLocator.GetService<TargetMapProvider>();
        _astar = ServiceLocator.GetService<AstarProvider>();
        GetTargetPath();
        _delay = new Timer(UPDATEPATHFREQUANCY);
        _delay.OnPeriodReached += GetTargetPath;
    }

    private void Update(float obj)
    {
        _delay.Update(obj);
    }

    public void PreExecute()
    {
    }

    private void GetTargetPath()
    {
        if (_targetMaps.GetRandomTarget(_controller.AttachedUnit.teamNumber, out var end))
        {
            _path = _astar.PathFinding.GetPath(_controller.AttachedUnit.Position2D, end);
        }
    }

    public void Undo()
    {
    }
}
