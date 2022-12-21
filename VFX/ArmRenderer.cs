using UnityEngine;

[ExecuteAlways]
public class ArmRenderer : MonoBehaviour2D
{
    [SerializeField] private Transform _handTransform;
    [SerializeField] private LineRenderer _armRenderer;

    private void Start()
    {
        _armRenderer.positionCount = 2;
    }

    private void Update()
    {
        _armRenderer.SetPosition(0, _handTransform.position);
        _armRenderer.SetPosition(1, Position2D);
    }

    private void OnDrawGizmos()
    {
        return;
        if(_armRenderer == null || _handTransform == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(_handTransform.position, _armRenderer.endWidth);
        Gizmos.DrawLine(_handTransform.position, Position2D);
    }
}
