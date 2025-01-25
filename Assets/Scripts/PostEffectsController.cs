/*
 * Guide written by Benjamin Swee on YouTube
 */
using UnityEngine;

[ExecuteInEditMode]
public class PostEffectsController : MonoBehaviour
{
    [SerializeField] private Shader postShader;
    [SerializeField, Range(0, 1.5f)] public float radius;
    [SerializeField, Range(0, 5f)] public float feather;
    [SerializeField] public Color tintColor;
    private Material postEffectMaterial;

    private void Awake()
    {
        postEffectMaterial = new Material(postShader);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        int width = src.width;
        int height = src.height;
        
        RenderTexture startRenderTexture = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
        
        postEffectMaterial.SetFloat("_Radius", radius);
        postEffectMaterial.SetFloat("_Feather", feather);
        postEffectMaterial.SetColor("_TintColor", tintColor);
        Graphics.Blit(src, startRenderTexture, postEffectMaterial);
        Graphics.Blit(startRenderTexture, dest);
        RenderTexture.ReleaseTemporary(startRenderTexture);
    }
}
