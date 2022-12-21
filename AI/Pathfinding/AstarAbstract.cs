using UnityEngine;

public abstract class AstarAbstract : MonoBehaviour
{
    public abstract Vector2[] GetPath(Vector2 start, Vector2 end);
}
