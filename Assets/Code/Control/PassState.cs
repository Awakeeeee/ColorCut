using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassState : StateBase
{
    private Niba fsm;

    private float waitTime;
    private float timer;

    public PassState(Niba _fsm, float waitTime)
    {
        fsm = _fsm;
        this.waitTime = waitTime;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        timer = 0f;

        fsm.anim.SetTrigger("triggerBlink");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (timer <= waitTime)
        {
            timer += Time.deltaTime;
            return;
        }

        fsm.TranslateState(this, fsm.actState);
    }
}
