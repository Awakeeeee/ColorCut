using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIdle : StateBase
{
    private Niba fsm;
    private bool inProcess;

    public MenuIdle(Niba _fsm)
    {
        fsm = _fsm;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        fsm.Visiblility(false);
        fsm.finger.DisableControl = true;
        inProcess = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!inProcess && Input.GetMouseButtonDown(0))
        {
            inProcess = true;
            fsm.Visiblility(false);
            MainMenuPage main = GameManager.Instance.ui.GetPage(UIID.MainMenu) as MainMenuPage;
            main.FadeOut();
            GameManager.Instance.cam.State = CameraBehaviourState.To_InGame;
            GameManager.Instance.level.OnNewLevelLoad();
        }
        else if (inProcess)
        {
            if (GameManager.Instance.cam.State == CameraBehaviourState.Static)
            {
                GameManager.Instance.ui.ShowPage(UIID.GameHUD);
                fsm.TranslateState(this, fsm.intoScene);
            }
        }
    }
}
