using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : PostRenderEffector
{
    public Color maskColor;
    [Range(0, 1)]
    public float lerpValue;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mat == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        mat.SetColor("_MaskColor", maskColor);
        mat.SetFloat("_LerpValue", lerpValue);
        Graphics.Blit(source, destination, mat);
    }
}
