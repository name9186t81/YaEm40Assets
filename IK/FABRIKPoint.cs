using UnityEngine;

public class FABRIKPoint
{
    private Transform _boneTransform;

    public FABRIKPoint(Transform boneTransform)
    {
        _boneTransform = boneTransform;
    }

    public float PreviousDistance;

    public Vector2 Position { get
        {
            return _boneTransform.position;
        }
        set
        {
            _boneTransform.position = value;
        }
    }

    public FABRIKPoint PreviousPoint;
}
