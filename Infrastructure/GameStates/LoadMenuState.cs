using System;
using UnityEngine.SceneManagement;

namespace Infrastructure.Boot
{
    public class LoadMenuState : IGameState
    {
        public event Action OnExecutionEnded;
        private string _sceneName;

        public LoadMenuState(string sceneName)
        {
            _sceneName = sceneName;
        }

        public void Execute()
        {
            SceneManager.LoadScene(_sceneName);
            OnExecutionEnded?.Invoke();
        }
    }
}
