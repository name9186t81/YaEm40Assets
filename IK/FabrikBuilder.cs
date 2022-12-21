using UnityEngine;

public class FabrikBuilder : MonoBehaviour
{
    [SerializeField] private Transform[] _bones;
    public FABRIK Fabrik { get; private set; }

    public void Start()
    {
        if (_bones == null)
        {
            return;
        }
        FABRIKPoint[] points = new FABRIKPoint[_bones.Length];

        for(int i = 0, length = _bones.Length; i < length; i++)
        {
            points[i] = new FABRIKPoint(_bones[i]);
        }

        Fabrik = new FABRIK(points);
    }

    private void Update()
    {
        if (Fabrik == null)
        {
            return;
        }
        Fabrik.Solve(transform.position);
        Fabrik.Solve(transform.position);
    }
}
