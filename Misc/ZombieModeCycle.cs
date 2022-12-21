using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieModeCycle : MonoBehaviour, IService
{
    [SerializeField] private WaveInfo[] _waves;
    [SerializeField] private TriggeredSpawner[] _zombieSpawners;
    [SerializeField] private TriggeredSpawner _playerSpawner;
    [SerializeField] private int _maxActiveSpawns;
    [SerializeField] private int _maxPlayers;
    [SerializeField] private Unit[] _playerSpawns;

    [Header("Revive")]
    [SerializeField] private Unit _reviveHelicopter;
    [SerializeField] private float _helicopterSpawnRadius;
    [SerializeField] private int _revivesCount;
    private float _reviveTime;

    private WaveInfo _currentWave;
    private int _currentWaveIndex;
    private Unit[] _players;
    private List<Unit> _activeZombies = new List<Unit>();
    private int _currentRevives;
    public event Action<(int zombiesCount, int waveIndex)> OnWaveChange;
    public event Action OnLoose;
    public event Action OnWin;
    public event Action OnAllPlayerDeath;
    public event Action OnRevive;
    private int _spawnedZombiesCount;

    private void Start()
    {
        NormzlizeSpawnChances();
        SpawnPlayers();
        _currentWave = _waves[0];
    }

    private void SpawnPlayers()
    {
        _players = new Unit[_maxPlayers];
        for (int i = 0, length = _maxPlayers; i < length; i++)
        {
            _players[i] = _playerSpawner.Spawn<Unit>(_playerSpawns[Random.Range(0, _playerSpawns.Length)]);
            _players[i].Health.OnDeath += PlayerDied;
        }
    }

    private void PlayerDied(DamageArgs obj)
    {
        int aliveCount = 0;
        for (int i = 0, length = _players.Length; i < length; i++)
        {
            if (_players[i] != null && _players[i].gameObject.activeSelf)
            {
                aliveCount++;
                if (aliveCount == 2)
                {
                    return;
                }
            }
        }

        if (_currentRevives > _revivesCount)
        {
            OnLoose?.Invoke();
            return;
        }


        for (int i = 0, length = _players.Length; i < length; i++)
        {
            if (_players[i] == null)
            {
                continue;
            }

            _players[i].Health.OnDeath -= PlayerDied;
        }

        StartCoroutine(RevivePlayers());
        _currentRevives++;
    }

    private void Update()
    {
		if (_currentWaveIndex == _waves.Length){
			return;
		}
        UpdateList();
        if (_spawnedZombiesCount < _currentWave.ZombiesCount && _activeZombies.Count < _maxActiveSpawns)
        {
            _activeZombies.Add(RandomZobmieSpawner.Spawn<Unit>(GetRandomUnit(_currentWave.Infos)));
            _spawnedZombiesCount++;
        }

        if (_activeZombies.Count == 0 && _spawnedZombiesCount >= _currentWave.ZombiesCount)
        {
            _spawnedZombiesCount = 0;
            ChangeWave();
        }
    }

    private void UpdateList()
    {
        List<Unit> newList = new List<Unit>();
        for (int i = 0, length = _activeZombies.Count; i < length; i++)
        {
            if (_activeZombies[i] != null && _activeZombies[i].gameObject.activeSelf)
            {
                newList.Add(_activeZombies[i]); 
            }
        }
        _activeZombies = newList;
    }

    private void ChangeWave()
    {
        _currentWaveIndex++;
        if (_currentWaveIndex == _waves.Length)
        {
            Debug.Log("My ass won");
            OnWin?.Invoke();
        }

        _currentWave = _waves[_currentWaveIndex];
        OnWaveChange?.Invoke((_currentWave.ZombiesCount, _currentWaveIndex));
    }

    private IEnumerator RevivePlayers() 
    {
        var randomPoint = Vector2Utils.GetRandomPointOnCircle() * _helicopterSpawnRadius + _playerSpawner.Position2D;
        var helicopter = Instantiate(_reviveHelicopter, randomPoint, randomPoint.LookAt2D(_playerSpawner.Position2D), null);
        yield return new WaitForSeconds(1f);
        if (helicopter.ComponentSystem.TryToGetComponent<Motor>(out var motor))
        {
            _reviveTime = _helicopterSpawnRadius / motor.GetMaxSpeed();
        }
        Debug.LogError("ASS");
        AIController helicopterAI = helicopter.CurrentController as AIController;
        helicopterAI.MoveToPoint(_playerSpawner.Position2D);
        helicopter.transform.position += Vector3.one;
        helicopterAI.MoveToPoint(_playerSpawner.Position2D);
        helicopter.transform.rotation = helicopter.transform.LookAt2D(_playerSpawner.Position2D);
        helicopterAI._enabled = false;
        yield return new WaitForSeconds(_reviveTime);
        helicopterAI._enabled = true;
        SpawnPlayers();
        yield return new WaitForSeconds(_reviveTime);
        helicopterAI._enabled = false;
        helicopterAI.MoveToPoint(randomPoint);
        helicopter.transform.rotation = helicopter.transform.LookAt2D(randomPoint);
        yield return new WaitForSeconds(_reviveTime);
        Destroy(helicopter);
    }

    private void NormzlizeSpawnChances()
    {
        for (int i = 0, length = _waves.Length; i < length; i++)
        {
            float sum = 0f;
            for (int j = 0; j < _waves[i].Infos.Length; j++)
            {
                sum += _waves[i].Infos[j].Chance;
            }

            for (int j = 0; j < _waves[i].Infos.Length; j++)
            {
                _waves[i].Infos[j].Chance /= sum;
            }

            float progress = 0f;
            for (int j = 0, length2 = _waves[i].Infos.Length; j < length2; j++)
            {
                progress += _waves[i].Infos[j].Chance;
                _waves[i].Infos[j].Chance = progress;
            }
        }
    }

    private Unit GetRandomUnit(ZombieInfo[] infos)
    {
        float random = Random.Range(0, 1f);

        for (int i = 0, length = infos.Length; i < length; i++)
        {
            if (infos[i].Chance > random)
            {
                return infos[i].Spawnable;
            }
        }

        return null;
    }

    [Serializable]
    private struct WaveInfo
    {
        public ZombieInfo[] Infos;
        public int ZombiesCount;
    }

    [Serializable]
    private struct ZombieInfo
    {
        public Unit Spawnable;
        public float Chance;
    }

    private TriggeredSpawner RandomZobmieSpawner => _zombieSpawners[Random.Range(0, _zombieSpawners.Length)];
	public int MaxWaves => _waves.Length;
	public int SpawnedInThisWave => _currentWave.ZombiesCount;
	public int SpawnedCount => _activeZombies.Count;
}
