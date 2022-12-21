using System.Collections.Generic;
using UnityEngine;

public class TargetMapProvider : MonoBehaviour, IService
{
    [SerializeField] private NavMap _navMap;
    [SerializeField] private float _updateTime;
    private Grid<EmptyNode> _grid;
    private Dictionary<int, TargetMap> _targetMaps = new Dictionary<int, TargetMap>();
    private Timer _timer;

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }
    private void Awake()
    {
        _grid = new Grid<EmptyNode>(_navMap.Size.y, _navMap.Size.x, _navMap.CellSize, _navMap.transform.position, (Grid<EmptyNode> gird, Vector2Int gridPos, Vector2 worldPost) => new EmptyNode());
        _timer = new Timer(_updateTime);
        _timer.OnPeriodReached += Clear;
        if (ServiceLocator.TryGetService<TargetMapProvider>(out var provider))
        {
            Debug.Log("Replacing");
            provider.UpdateProvider(provider);
        }
        else
        {
            Debug.Log("Adding");
            ServiceLocator.AddService<TargetMapProvider>(this);
        }
    }

    private void Clear()
    {
        _targetMaps = new Dictionary<int, TargetMap>();
    }

    public void AddTarget(int selfTeamNumber, Vector2 worldPosition)
    {
        if (!_grid.IsInGrid(worldPosition))
        {
            return;
        }
        var map = GetMap(selfTeamNumber);

        map.AddTarget(worldPosition);
    }

    public bool GetRandomTarget(int selfTeamNumber, out Vector2 target)
    {
        var map = GetMap(selfTeamNumber);
        return map.TryGetRandomTarget(out target);
    }

    private TargetMap GetMap(int teamNum)
    {
        TargetMap map = default;
        if (!_targetMaps.TryGetValue(teamNum, out map))
        {
            _targetMaps[teamNum] = new TargetMap();
            map = _targetMaps[teamNum];
        }
        return map;
    }

    private void UpdateProvider(TargetMapProvider provider)
    {
        provider._navMap = _navMap;
        provider._updateTime = _updateTime;
        provider._targetMaps = new Dictionary<int, TargetMap>();
        provider._timer.OnPeriodReached -= Clear;
        provider._timer = _timer;
        provider._timer.OnPeriodReached += Clear;
    }
}
