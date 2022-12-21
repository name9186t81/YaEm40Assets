using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindTesting : MonoBehaviour
{
    [SerializeField] private Astar _aster;
    [SerializeField] private Transform _t1;
    [SerializeField] private Transform _t2;

    private void Update()
    {
        var path = _aster.GetPath(_t1.position, _t2.position);

        Debug.LogError(path.Length);
        for (int i = 0, length = path.Length; i < length - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red);
        }
    }
}
