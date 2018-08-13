using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    private static CameraEffect _instance;
    public static CameraEffect Instance { get { return _instance; } }

    private Dictionary<Effect, PostRenderEffector> effects;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(_instance.gameObject);
        }

        //DontDestroyOnLoad(gameObject);

        effects = new Dictionary<Effect, PostRenderEffector>
        {
            { Effect.Blur, GetComponent<Blur>()},
            { Effect.Mask, GetComponent<Mask>()},
            //...
        };
    }

    //TODO 没有好办法同时应用2个以上效果
    public IEnumerator PixelatedBlur(int start, int end, float time)
    {
        Blur effector = effects[Effect.Blur] as Blur;
        float timer = 0f;
        effector.texScaleDown = start;

        effector.enabled = true;

        while (timer < time)
        {
            int val = (int)Mathf.Lerp(start, end, timer / time);
            effector.texScaleDown = val;
            timer += Time.deltaTime;
            yield return null;
        }
        effector.texScaleDown = end;
        effector.enabled = false;
    }

    public IEnumerator GaussainBlur(int startL, int endL, int startS, int endS, float time)
    {
        Blur effector = effects[Effect.Blur] as Blur;
        effector.enabled = true;

        float timer = 0f;
        effector.blurLoopTime = startL;
        effector.blurSpread = startS;
        while (timer < time)
        {
            int s = (int)Mathf.Lerp(startS, endS, timer / time);
            effector.blurSpread = s;
            timer += Time.deltaTime;
            yield return null;
        }

        effector.blurSpread = endS;
        effector.blurLoopTime = endL;
        effector.enabled = false;
    }

    public IEnumerator ScreenCurtainDown()
    {
        Mask effector = effects[Effect.Mask] as Mask;
        effector.maskColor = Color.black;
        float start = 0;
        float end = 1;
        float time = 2f;

        effector.lerpValue = start;
        float timer = 0f;

        effector.enabled = true;

        while (timer < time)
        {
            float val = Mathf.Lerp(start, end, timer / time);
            effector.lerpValue = val;
            timer += Time.deltaTime;
            yield return null;
        }
        effector.lerpValue = end;
    }

    public IEnumerator ScreenCurtainUp()
    {
        Mask effector = effects[Effect.Mask] as Mask;
        effector.maskColor = Color.black;
        float start = 1;
        float end = 0;
        float time = 1f;

        effector.lerpValue = start;
        float timer = 0f;

        effector.enabled = true;

        while (timer < time)
        {
            float val = Mathf.Lerp(start, end, timer / time);
            effector.lerpValue = val;
            timer += Time.deltaTime;
            yield return null;
        }
        effector.lerpValue = end;
        effector.enabled = false;
    }

    public void DisableAllEffectors()
    {
        foreach (PostRenderEffector p in effects.Values)
            p.enabled = false;
    }
}

public enum Effect
{
    Blur,
    Mask,
    Bloom,
    ToonEdge,
    HSV_Enhance
}
