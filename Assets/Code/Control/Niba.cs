using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niba : MonoBehaviour
{
    public Finger finger;
    public ReusableParticleEffect deathEffect;
    public AudioClip deathClip;
    public AudioClip sliceClip;

    public Vector3 fingerStart;
    public Vector3 fingerEnd;
    public SliceResult sliceResult;

    private SpriteRenderer renderComponent;
    private TrailRenderer trailComponent;
    public Animator anim;

    [Header("AI")]
    public IntoScene intoScene;
    public MenuIdle menuIdle;
    public GameIdle gameIdle;
    public SliceState sliceState;
    public PassState passState;
    public FailState failState;
    public ActState actState;
    private StateBase currentState;

    private void Start()
    {
        renderComponent = GetComponent<SpriteRenderer>();
        trailComponent = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();

        menuIdle = new MenuIdle(this);
        intoScene = new IntoScene(this);
        gameIdle = new GameIdle(this);
        sliceState = new SliceState(this, 10, 0.5f);
        passState = new PassState(this, 1f);
        failState = new FailState(this, 0.5f);
        actState = new ActState(this);
        TranslateState(null, menuIdle);
    }

    public void OnSliceReady(Vector3 start, Vector3 end, SliceResult result)
    {
        fingerStart = start;
        fingerEnd = end;
        sliceResult = result;

        //TranslateState(gameIdle, sliceState);
        //return;

        if (currentState == gameIdle)
        {
            TranslateState(gameIdle, sliceState);
        }
        else {
            Debug.LogWarning("Slice ready but Niba will not move, because in state: " + currentState.ToString());
        }
    }

    public void TranslateState(StateBase from, StateBase to)
    {
        if (from == to)
        {
            Debug.LogWarning("Translating to the same state");
            return;
        }

        if (from != null)
            from.OnExit();
        if (to != null)
            to.OnEnter();
        currentState = to;
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void Visiblility(bool isVisible)
    {
        renderComponent.enabled = isVisible;
        trailComponent.enabled = isVisible;
    }
}

public enum SliceResult
{
    Pass,
    Fail,
    Retry
}

