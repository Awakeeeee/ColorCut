using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blur : PostRenderEffector
{
    [Range(1, 50)]
    public int texScaleDown = 1;
    public FilterMode mode;
    [Range(1, 10)]
    public int blurSpread;
    [Range(0, 50)]
    public int blurLoopTime;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mat == null)
        {
            Debug.LogError("No shader to use for the effect");
            Graphics.Blit(source, destination);
            return;
        }

        int width = source.width / texScaleDown;
        int height = source.width / texScaleDown;

        RenderTexture buffer0 = RenderTexture.GetTemporary(width, height);
        buffer0.filterMode = mode;
        Graphics.Blit(source, buffer0);
        //apply blur many times
        for (int i = 0; i < blurLoopTime; i++)
        {
            mat.SetFloat("_BlurSpread", 1f + (float)blurSpread * i);

            RenderTexture buffer1 = RenderTexture.GetTemporary(width, height);

            Graphics.Blit(buffer0, buffer1, mat, 0);
            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;

            buffer1 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(buffer0, buffer1, mat, 1);
            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;
        }
        Graphics.Blit(buffer0, destination);
        RenderTexture.ReleaseTemporary(buffer0);
    }
}
