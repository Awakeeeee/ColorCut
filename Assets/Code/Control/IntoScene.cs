using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoScene : StateBase
{
    private Niba fsm;

    private float arriveTime;
    private Vector3 start;
    private Vector3 destiny;
    private float timer = 0;

    public IntoScene(Niba _fsm)
    {
        fsm = _fsm;
        arriveTime = 0.5f;
        start = new Vector3(GameManager.screenWidth / 2f, GameManager.screenHeight / 2f + 1f, 0f);
        destiny = new Vector3(GameManager.screenWidth / 2f, GameManager.screenHeight / 4f, 0f);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        fsm.transform.position = start;
        fsm.Visiblility(true);
        timer = 0f;

        fsm.anim.SetTrigger("triggerSlice");
    }

    public override void UpdateState()
    {
        if (timer < arriveTime)
        {
            fsm.transform.position = Vector3.Lerp(start, destiny, timer / arriveTime);
            timer += Time.deltaTime;
        }
        else
        {
            fsm.transform.position = destiny;
            fsm.TranslateState(this, fsm.gameIdle);
        }
    }
}
