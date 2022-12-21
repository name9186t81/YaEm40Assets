using UnityEngine;

public class DisplacmentRendering : MonoBehaviour
{
    [SerializeField] private Camera _displacmentCamera;
    [SerializeField] private Material _explosionMaterial;
    [SerializeField] private RenderTexture _displacmentTexture;

    private void Awake()
    {
        _displacmentTexture = new RenderTexture(_displacmentCamera.pixelWidth, _displacmentCamera.pixelHeight, (int)_displacmentCamera.depth);
        _displacmentCamera.targetTexture = _displacmentTexture;
    }

    public RenderTexture DisplacmentTexture => _displacmentTexture;
}
