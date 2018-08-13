using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActState : StateBase
{
    private Niba fsm;

    private bool inBackProcess;
    private Vector3 destiny;

    private float delay;
    JourneyLine journeyLine;

    public ActState(Niba _fsm)
    {
        fsm = _fsm;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        fsm.finger.DisableControl = true;
        inBackProcess = false;
        delay = 0f;
        
        fsm.transform.position = GameManager.Instance.journeyPoints[GameManager.Instance.topLevelNum];
        GameManager.Instance.level.OnLevelUnload(SliceResult.Pass);

        GameManager.Instance.cam.State = CameraBehaviourState.To_Distant;
        destiny = GameManager.Instance.journeyPoints[GameManager.Instance.topLevelNum];

        journeyLine = GameManager.Instance.GetJourneyLine();
        journeyLine.lineRenderComponent.StartPos = fsm.transform.position;
        journeyLine.lineRenderComponent.EndPos = fsm.transform.position;
        journeyLine.gameObject.SetActive(true);

        fsm.anim.SetTrigger("triggerIdle");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (inBackProcess) //while zooming to new level
        {
            if (GameManager.Instance.cam.State == CameraBehaviourState.Static)
            {
                GameManager.Instance.level.objContainer.gameObject.SetActive(true);
                fsm.TranslateState(this, fsm.intoScene);
                return;
            }
        }
        else if (GameManager.Instance.cam.State == CameraBehaviourState.Static) //zoom to distant is complete
        {
            if (delay < 0.2f)
            {
                delay += Time.deltaTime;
                return;
            }

            fsm.transform.position = Vector3.Lerp(fsm.transform.position, destiny, Time.deltaTime);
            journeyLine.lineRenderComponent.EndPos = fsm.transform.position;
            if (Vector3.Distance(fsm.transform.position, destiny) < 0.2f) //Niba act is complete
            {
                if (!inBackProcess && Input.GetMouseButtonDown(0)) //press to go next level
                {
                    GameManager.Instance.cam.State = CameraBehaviourState.To_InGame;
                    GameManager.Instance.level.OnNewLevelLoad();

                    fsm.transform.position = new Vector3(GameManager.screenWidth/2f, GameManager.screenWidth/4f, 0f);
                    GameManager.Instance.journeyLinesContainer.gameObject.SetActive(false);

                    inBackProcess = true;
                    fsm.Visiblility(false);
                    GameManager.Instance.level.objContainer.gameObject.SetActive(false);
                }
            }
        }
    }
}
