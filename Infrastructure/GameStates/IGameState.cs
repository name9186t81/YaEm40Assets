using System;

namespace Infrastructure.Boot
{
    public interface IGameState
    {
        void Execute();
        event Action OnExecutionEnded;
    }
}
