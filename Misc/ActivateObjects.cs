using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActivateObjects : MonoBehaviour
{
    [SerializeField] private ObjectChance[] _objects;
    private float _totalChance;

    private void Awake()
    {
        for (int i = 0, length = _objects.Length; i < length; i++)
        {
            _totalChance += _objects[i].Chance;
        }

        for (int i = 0, length = _objects.Length; i < length; i++)
        {
            _objects[i].Chance /= _totalChance;
        }

        float progress = 0f;
        for (int i = 0, length = _objects.Length; i < length; i++)
        {
            progress += _objects[i].Chance;
            _objects[i].AssignedNumber = progress;
        }
    }

    public void Activate()
    {
        float random = Random.Range(0, 1f);

        for (int i = 0, length = _objects.Length; i < length; i++)
        {
            if (_objects[i].AssignedNumber > random)
            {
                _objects[i].GameObject.SetActive(true);
                return;
            }
        }
    }

    [Serializable]
    private struct ObjectChance
    {
        public GameObject GameObject;
        public float Chance;
        [NonSerialized] public float AssignedNumber;
    }
}
