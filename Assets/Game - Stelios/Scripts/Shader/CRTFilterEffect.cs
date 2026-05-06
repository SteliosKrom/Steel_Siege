using UnityEngine;

public class CRTFilterEffect : MonoBehaviour
{
    public Material crtMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (crtMaterial != null)
        {
            Graphics.Blit(src, dst, crtMaterial);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }
}