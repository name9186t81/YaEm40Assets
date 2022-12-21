using UnityEngine;

public class Interact : IWeightState<AIController>
{
    private Vector2[] _path;
    private int _currentIndex;

    private InteractablesScaner _scaner;
    private Interactable _lastInteractable;
    private MonoBehaviour2D _attachedUnitPosition;
    private LayerMask _walls;
    private AstarAbstract _astar;
    private AIController _controller;

    public Interact(LayerMask walls)
    {
        _walls = walls;
    }

    public float CalculateEffectivness()
    {
        if (_scaner == null)
        {
            if (_controller.AttachedUnit.ComponentSystem.TryToGetComponent<InteractablesScaner>(out _scaner))
            {
                _scaner.OnScan += Scan;
            }
        }
        return (_lastInteractable != null) ? _lastInteractable.AIPriority : -100f;
    }

    public void Execute()
    {
        if (_lastInteractable == null)
        {
            if (_controller.AttachedUnit.ComponentSystem.TryToGetComponent<InteractablesScaner>(out _scaner))
            {
                _scaner.OnScan += Scan;
            }
            return;
        }

        if (_path != null)
        {
            if (_currentIndex > _path.Length - 1)
            {
                _path = null;
                _currentIndex = 0;
                _lastInteractable.Interact(_controller.AttachedUnit);
                _lastInteractable = null;
            }

            _controller.SafeWalk(_path[_currentIndex]);
            _controller.LookAtPoint(_path[_currentIndex]);

            if (Vector2.Distance(_attachedUnitPosition.Position2D, _path[_currentIndex]) < _controller.AttachedUnit.Size)
            {
                _currentIndex++;
            }
        }
        else
        {
            if (Vector2.Distance(_attachedUnitPosition.Position2D, _lastInteractable.Position2D) < _controller.AttachedUnit.Size)
            {
                _lastInteractable.Interact(_controller.AttachedUnit);
                _lastInteractable = null;
                return;
            }

            _controller.SafeWalk(_lastInteractable.Position2D);
            _controller.LookAtPoint(_lastInteractable.Position2D);
        }
    }

    public void Init(AIController owner)
    {
        owner.AttachedUnit.ComponentSystem.TryToGetComponent<InteractablesScaner>(out _scaner);
        _controller = owner;
        _attachedUnitPosition = owner.AttachedUnit;

        if (ServiceLocator.TryGetService<AstarProvider>(out var provider))
        {
            _astar = provider.PathFinding;
        }

        if (_scaner == null)
        {
            throw new System.Exception("No interact scanner attached to unit");
        }

        _scaner.OnScan += Scan;
    }

    private void Scan(Interactable obj)
    {
        if (_lastInteractable != null)
        {
			if (_path == null && Physics2D.Raycast(_attachedUnitPosition.Position2D,
            _lastInteractable.Position2D - _attachedUnitPosition.Position2D,
            Vector2.Distance(_lastInteractable.Position2D, _attachedUnitPosition.Position2D),
            _walls))
			{
				_path = _astar.GetPath(_attachedUnitPosition.Position2D, _lastInteractable.Position2D);
			}
            return;
        }

        _lastInteractable = obj;
		
        if (_path == null && Physics2D.Raycast(_attachedUnitPosition.Position2D,
            _lastInteractable.Position2D - _attachedUnitPosition.Position2D,
            Vector2.Distance(_lastInteractable.Position2D, _attachedUnitPosition.Position2D),
            _walls))
        {
            _path = _astar.GetPath(_attachedUnitPosition.Position2D, _lastInteractable.Position2D);
        }
    }

    public void PreExecute()
    {
    }

    public void Undo()
    {
    }
}
