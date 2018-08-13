using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public ViewParameter focus;
    public ViewParameter inGame;
    public ViewParameter distant;
    public ViewParameter finalView;
    public float translateTime;

    private Camera self;
    [SerializeField]
    private float timer;
    private CameraBehaviourState currentState;
    public CameraBehaviourState State { get { return currentState; } set { currentState = value; } }
    private ViewParameter currentParam;

    private float width;
    public float Width { get { return width; } }
    private float height;
    public float Height { get { return height; } }

    private void Awake()
    {
        self = GetComponent<Camera>();
        width = GetScreenWidth();
        height = GetScreenHeight();
    }

    private void Start()
    {
        float scale = focus.size / inGame.size;
        transform.position = new Vector3(width / 4f, 0f, transform.position.z);
        focus.pos = new Vector3(scale * width / 2, focus.pos.y, focus.pos.z);
        inGame.pos = new Vector3(width / 2, inGame.pos.y, inGame.pos.z);
        distant.pos = new Vector3(width / 2, inGame.pos.y, inGame.pos.z);

        currentState = CameraBehaviourState.Static;
        SetCurrentView(focus);
        timer = 0f;
    }

    private void Update()
    {
        switch (currentState)
        {
            case CameraBehaviourState.Static:
                break;
            case CameraBehaviourState.To_Focus:
                TranslateView(focus);
                break;
            case CameraBehaviourState.To_InGame:
                TranslateView(inGame);
                break;
            case CameraBehaviourState.To_Distant:
                TranslateView(distant);
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentState = CameraBehaviourState.To_InGame;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            currentState = CameraBehaviourState.To_Focus;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentState = CameraBehaviourState.To_Distant;
        }
    }

    public void TranslateView(ViewParameter to)
    {
        timer += Time.deltaTime;
        self.orthographicSize = Mathf.Lerp(currentParam.size, to.size, timer / translateTime);
        self.transform.position = Vector3.Lerp(currentParam.pos, to.pos, timer / translateTime);
        if (timer > translateTime)
        {
            SetCurrentView(to);
            timer = 0f;
            currentState = CameraBehaviourState.Static;
        }
    }

    public void SetCurrentView(ViewParameter param)
    {
        self.orthographicSize = param.size;
        self.transform.position = param.pos;
        currentParam = param;
    }

    public float GetScreenHeight()
    {
        return self.orthographicSize * 2;
    }
    public float GetScreenWidth()
    {
        return GetScreenHeight() * self.aspect;
    }
}

[System.Serializable]
public struct ViewParameter
{
    public float size;
    public Vector3 pos;
}

public enum CameraBehaviourState
{
    Static,
    To_Focus,
    To_InGame,
    To_Distant
}
