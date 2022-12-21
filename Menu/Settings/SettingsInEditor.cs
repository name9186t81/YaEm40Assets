using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsInEditor : MonoBehaviour
{
    [SerializeField] private List<SettingsData> _data;
    [SerializeField] private string _savePath;

    private void Start()
    {
        if (!File.Exists(SavePath))
        {
            File.Create(SavePath);
        }
    }

    public void Rewrite()
    {

    }

    [Serializable]
    private struct SettingsData
    {
        public string Name;
        public string DefaultValue;
    }

    private string SavePath => Application.dataPath + _savePath;
}
