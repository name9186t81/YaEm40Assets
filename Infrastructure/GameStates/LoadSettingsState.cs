using System;

namespace Infrastructure.Boot
{
    public class LoadSettingsState : IGameState
    {
        public event Action OnExecutionEnded;
        private string _pathToSave;

        public LoadSettingsState(string savePath)
        {
            _pathToSave = savePath;
        }

        public void Execute()
        {
            SettingsLoader loader = new SettingsLoader(_pathToSave);
            ServiceLocator.AddService(loader.Load());
            OnExecutionEnded?.Invoke();
        }
    }
}