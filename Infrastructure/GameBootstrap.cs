using System;
using System.Collections.Generic;
using UnityEngine;
using Infrastructure.Boot;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private string _pathToSave;
    [field: SerializeField] public string MainMenuSceneName { get; private set; }
    [SerializeField] private string _pathToSettingsSave;
    private Queue<IGameState> _states = new Queue<IGameState>();
    private IGameState _lastState;

    private void OnEnable()
    {
        BootstrapLoader loader = new BootstrapLoader(this);
        _states = loader.Load();

        Execute();
    }

    private void Execute()
    {
        if (_lastState != null)
        {
            _lastState.OnExecutionEnded -= Execute;
        }

        if (_states.Count == 0)
        {
            return;
        }
        var state = _states.Dequeue();

        _lastState = state;
        _lastState.OnExecutionEnded += Execute;
        _lastState.Execute();
    }

    public string BootstrapSavePath => Application.persistentDataPath + _pathToSave;
    public string SettingsSavePath => Application.persistentDataPath + _pathToSettingsSave;
}
