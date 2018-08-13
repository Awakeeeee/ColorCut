using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VolumetricLines;

public class JourneyLine : MonoBehaviour
{
    public float completeWidth;
    public float lerpTime;
    public VolumetricLineBehavior lineRenderComponent;

    private void Awake()
    {
        lineRenderComponent = GetComponent<VolumetricLineBehavior>();
    }

    private void OnEnable()
    {
        StartCoroutine(LerpWidth());
    }

    IEnumerator LerpWidth()
    {
        lineRenderComponent.LineWidth = 0;
        float timer = 0f;
        while (timer < lerpTime)
        {
            lineRenderComponent.LineWidth = Mathf.Lerp(0f, completeWidth, timer / lerpTime);
            timer += Time.deltaTime;
            yield return null;
        }
        lineRenderComponent.LineWidth = completeWidth;
    }
}
