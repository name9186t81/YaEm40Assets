using UnityEngine;

public class WeightedStateMachine<TOwner> : IStateMachine<IWeightState<TOwner>, TOwner>
{
    private TOwner _owner;
    private IWeightState<TOwner>[] _states;
    private IWeightState<TOwner> _current;
    public IState<TOwner> CurrentState => _current;

    public void Execute()
    {
        if(_states == null)
        {
            return;
        }
        var calculted = CalculateBestState();
        if(_current != calculted)
        {
            _current?.Undo();
            _current = calculted;
            _current?.PreExecute();
        }
        _current.Execute();
    }

    public void Init(IWeightState<TOwner>[] states, TOwner owner)
    {
        _states = states;
        statesLength = _states.Length;
        _owner = owner;

        for(int i = 0; i < statesLength; i++)
        {
            _states[i].Init(owner);
        }
    }

    private IWeightState<TOwner> CalculateBestState()
    {
        IWeightState<TOwner> result = _current;
        float maxEffect = _current == null ? float.MinValue : _current.CalculateEffectivness();
        for(int i = 0; i < statesLength; i++)
        {
            IWeightState<TOwner> current = _states[i];
            float currentEf = current.CalculateEffectivness();
            if (currentEf > maxEffect)
            {
                maxEffect = currentEf;
                result = current;
            }
        }
        return result;
    }

    private int statesLength;
}
