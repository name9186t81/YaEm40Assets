using UnityEngine;

public abstract class SettingsParser : MonoBehaviour
{
    [field: SerializeField] public string Line { get; private set; }
    public object Object; //YES YOU CAN CHANGE IT ANYWHERE

    private void OnEnable()
    {
        if (ServiceLocator.TryGetService<SettingsContainer>(out var container) && container.TryGetSetting(Line, out var obj))
        {
            ValueLoded(obj);
        }
    }


    public abstract object ParseLine(string value);
    public abstract string SaveObject();
    protected virtual void ValueLoded(object value)
    {

    }
}
