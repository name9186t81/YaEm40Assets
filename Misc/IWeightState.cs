using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeightState<T> : IState<T>
{
    float CalculateEffectivness();
}
