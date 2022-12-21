using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Boot
{
    public sealed class BootstrapLoader : FileLoader
    {
        private readonly string[] _standartStates = new string[]
            {
                "LoadSettings",
                "LoadMenu",
                "LoadGame",
                "SaveProgress",
                "Exit"
            };

        private GameBootstrap _gameBootstrap;

        public BootstrapLoader(GameBootstrap bootstrap) : base(bootstrap.BootstrapSavePath)
        {
            _gameBootstrap = bootstrap;
        }

        public Queue<IGameState> Load()
        {
            var strings = ReadOrCreateFile(_standartStates);
            Queue<IGameState> states = new Queue<IGameState>();

            for (int i = 0, length = strings.Length; i < length; i++)
            {
                if (TryParse(strings[i], out var state))
                {
                    states.Enqueue(state);
                }
            }

            return states;
        }

        private bool TryParse(string name, out IGameState value)
        {
            switch (name)
            {
                case "LoadSettings":
                    {
                        value = new LoadSettingsState(_gameBootstrap.SettingsSavePath);
                        return true;
                    }
                case "LoadMenu":
                    {
                        value = new LoadMenuState(_gameBootstrap.MainMenuSceneName);
                        return true;
                    }
                default:
                    {
                        Debug.LogWarning("Cannot load game state with a name: " + name);
                        value = null;
                        return false;
                    }
            }
        }
    }
}