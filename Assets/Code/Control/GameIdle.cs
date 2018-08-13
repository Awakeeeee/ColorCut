using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIdle : StateBase
{
    private Niba fsm;

    private Vector2 boringRange;
    private float boringWait;
    private float boringTimer;

    public GameIdle(Niba _fsm)
    {
        fsm = _fsm;
        boringRange = new Vector2(6f, 15f);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        fsm.finger.DisableControl = false;

        boringWait = Random.Range(boringRange.x, boringRange.y);
        boringTimer = 0f;
        fsm.anim.SetTrigger("triggerIdle");
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if(boringTimer < boringWait)
        {
            boringTimer += Time.deltaTime;
        }
        else
        {
            fsm.anim.SetTrigger("triggerSpeak");
            GameManager.Instance.ShowWposDialog();
            boringWait = Random.Range(boringRange.x, boringRange.y);
            boringTimer = 0f;
        }
    }
}
