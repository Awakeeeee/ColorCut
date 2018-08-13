using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailState : StateBase
{
    private Niba fsm;
    private float resetWait;
    private float timer;

    public FailState(Niba _fsm, float resetWait)
    {
        fsm = _fsm;
        this.resetWait = resetWait;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        fsm.deathEffect.transform.position = fsm.transform.position;
        fsm.deathEffect.gameObject.SetActive(true);
        GameManager.Instance.sound.PlayInGameClip(fsm.deathClip);
        fsm.Visiblility(false);

        timer = 0f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (timer < resetWait)
        {
            timer += Time.deltaTime;
            return;
        }

        fsm.transform.position = new Vector3(GameManager.screenWidth / 2f, GameManager.screenHeight / 4f, 0f);
        fsm.Visiblility(true);
        fsm.TranslateState(this, fsm.gameIdle);
        GameManager.Instance.level.ResetLevel();
    }
}
