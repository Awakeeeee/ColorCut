using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRenderEffector : MonoBehaviour
{
    public Shader shader;
    protected Material mat;

    protected void Start()
    {
        if (shader == null)
            return;

        mat = new Material(shader);
        mat.hideFlags = HideFlags.DontSave;
    }
}
