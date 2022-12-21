using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLocker : MonoBehaviour
{
    [SerializeField, Range(1, 1000)] private int FPS;

    private void Update()
    {
        Application.targetFrameRate = FPS;
    }
}
