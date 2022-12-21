using UnityEngine;

public class DisplacmentAdjuster : MonoBehaviour
{
    [SerializeField] private DisplacmentRendering _render;
    [SerializeField] private Material _distortionMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        _distortionMaterial.SetTexture("_DisplacmentMap", _render.DisplacmentTexture);
        Graphics.Blit(source, destination, _distortionMaterial);
    }

}
