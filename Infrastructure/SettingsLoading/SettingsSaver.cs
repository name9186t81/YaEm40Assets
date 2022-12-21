using System.IO;
using UnityEngine;

//todo: MAKE A FUCKING UNITY TOOL TO STORE SAVEPATH
public class SettingsSaver : MonoBehaviour
{
    [SerializeField] private SettingsParser[] _parsers;
    [SerializeField] private string _savePath;

    public void Save()
    {
        using (StreamWriter sw = new StreamWriter(new FileStream(Path, FileMode.OpenOrCreate)))
        {
            sw.Flush();
            for (int i = 0, length = _parsers.Length; i < length; i++)
            {
                string line = _parsers[i].Line + ":" + _parsers[i].SaveObject();
                sw.WriteLine(line);
            }
        }
        SettingsLoader loader = new SettingsLoader(Path);
        if (ServiceLocator.TryGetService<SettingsContainer>(out var container))
        {
            container.UpdateContainer(loader.Load());
        }
    }

    private string Path => Application.persistentDataPath + _savePath;
}
