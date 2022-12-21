using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour2D
{
    [SerializeField] private float _spawnDelay;
    [SerializeField] private GameObject[] _spawnable;
    [SerializeField] private int _maxActiveSpawns;
    [SerializeField] private Vector2 _size;
    [SerializeField] private int _teamNumber;
    [SerializeField] private bool _ignoreSettings = false;
    [SerializeField] private bool _immediateSpawn = false;
    [SerializeField] private bool _oneSpawn = false;

    private bool _locked = false;
    private List<GameObject> _spawned = new List<GameObject>();
    private Timer _timer;

    private void Awake()
    {
        _timer = new Timer(_spawnDelay);
        _timer.OnPeriodReached += Spawn;

        if (_immediateSpawn)
        {
            Spawn();
        }
        if (_ignoreSettings) return;
        if (ServiceLocator.TryGetService<GameSettingsContainer>(out var container))
        {
            List<GameObject> newSpawnable = new List<GameObject>();
            for (int i = 0, length = _spawnable.Length; i < length; i++)
            {
                if (_spawnable[i].TryGetComponent<Unit>(out var unit))
                {
                    if (container.TryGetBool(unit.UnitName, out var value) && value)
                    {
                        newSpawnable.Add(_spawnable[i]);
                    }
                }
                else
                {
                    newSpawnable.Add(_spawnable[i]);
                }
            }
            _spawnable = newSpawnable.ToArray();
        }
    }

    private void Spawn()
    {
        if (_locked) return;

        UpdateList();
        if (_spawned.Count < _maxActiveSpawns && _spawnable.Length != 0)
        {
            var obj = Instantiate(RandomSpawnable, RandomPositionInBox, default, null);
            _spawned.Add(obj);

            //TODO: make a teamNumber container
            if (obj.TryGetComponent<Unit>(out var unit))
            {
                unit.teamNumber = _teamNumber;
            }
        }
        if (_oneSpawn)
        {
            _locked = true;
        }
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void UpdateList()
    {
        var newList = _spawned;
        for (int i = 0, length = _spawned.Count; i < length; i++)
        {
            if (_spawned[i] == null)
            {
                newList.RemoveAt(i);
                break;
            }
        }
        _spawned = newList;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _size);
    }

    private Vector2 RandomPositionInBox => new Vector2(Random.Range(-_size.x / 2, _size.x / 2), Random.Range(-_size.y / 2, _size.y / 2)) + Position2D;
    private GameObject RandomSpawnable => _spawnable[Random.Range(0, _spawnable.Length)];
}
