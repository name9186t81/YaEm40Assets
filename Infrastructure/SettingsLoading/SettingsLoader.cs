using System.Diagnostics;

public class SettingsLoader : FileLoader
{
    //todo: create tool in unity that allows to edit loaders like this
    private readonly string[] DEFAULT = new string[]
        {
        "TotalVolume: 1",
        "JoyStickType: Fixed",
        "MusicVolume: 1",
        "SoundVolume: 1"
        };
    
    public SettingsLoader(string filePath) : base(filePath)
    {
    }

    public SettingsContainer Load()
    {
        string[] settings = ReadOrCreateFile(DEFAULT);
        SettingsContainer container = new SettingsContainer();

        for (int i = 0, length = settings.Length; i < length; i++)
        {
            string[] input = settings[i].Replace(" ", "").Split(':');

            if (input.Length == 1)
            {
                UnityEngine.Debug.LogError(input[0] + " wtf this doing in loading");
                continue;
            }
            container.Append(input[0], ParseSetting(input[1]));
        }

        return container;
    }

    public object ParseSetting(string value)
    {
        //TODO: rework this lmao
        if (bool.TryParse(value, out _))
        {
            return bool.Parse(value);
        }
        if (float.TryParse(value, out _))
        {
            return float.Parse(value);
        }
        return value;
    }
}