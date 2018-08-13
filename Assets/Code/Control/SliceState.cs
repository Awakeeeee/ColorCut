using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceState : StateBase
{
    private Niba fsm;

    private float stepLength;
    private float resolutionStep;

    private float t;
    private Vector3 nextDest;

    public SliceState(Niba _fsm, int curveResolution, float speed)
    {
        fsm = _fsm;
        resolutionStep = 1 / (float)curveResolution;
        this.stepLength = speed;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        t = 0;
        nextDest = fsm.transform.position;
        GameManager.Instance.sound.PlayInGameClip(fsm.sliceClip);

        fsm.anim.SetTrigger("triggerSlice");
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector3 framePos = Vector3.MoveTowards(fsm.transform.position, nextDest, stepLength);
        fsm.transform.position = framePos;

        if (Vector3.Distance(fsm.transform.position, nextDest) < 0.05f)
        {
            t += resolutionStep;
            if (t >= 1 + resolutionStep) //END
            {
                switch (fsm.sliceResult)
                {
                    case SliceResult.Retry:
                        fsm.TranslateState(this, fsm.gameIdle);
                        break;
                    case SliceResult.Pass:
                        fsm.TranslateState(this, fsm.passState);
                        break;
                    case SliceResult.Fail:                      
                        fsm.TranslateState(this, fsm.failState);
                        break;
                    default:
                        break;
                }
            }
            nextDest = BezierCurvePointQuad(fsm.transform.position, fsm.fingerStart, fsm.fingerEnd, t);
        }
    }

    /*
     * linear: pf = (1-t)p0 + tp1
     * 
     * quadratic: 
     * a = (1-t)p0 + tp1    b = (1-t)p1 + tp2
     * pf = (1-t)a + tb
     *    = (1-t)^2 * p0 + 2t(1-t)p1 + t^2p2
     */
    private Vector3 BezierCurvePointQuad(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float x = QuadFormula(p0.x, p1.x, p2.x, t);
        float y = QuadFormula(p0.y, p1.y, p2.y, t);
        return new Vector3(x, y, p0.z);
    }
    private float QuadFormula(float p0, float p1, float p2, float t)
    {
        return Mathf.Pow(1 - t, 2) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;
    }
}
