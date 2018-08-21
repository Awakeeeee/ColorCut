using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VolumetricLines;

public class Finger : MonoBehaviour
{
    public bool gizmosOn;
    public Niba niba;

    private ColorClass passColor;
    private LayerMask targetLayer;

    private Vector3 start;
    private Vector3 end;
    private FingerState fstate;
    //private LineRenderer lr;
    private VolumetricLineBehavior vl;
    private Vector3 fingerPos;

    private bool disableControl;
    public bool DisableControl { get { return disableControl; } set { disableControl = value; } }

    public Color colop1;
    public Color colop2;
    public Color colop3;
    
    private List<Segment> segmentTrack;

    private void Awake()
    {
        //lr = GetComponent<LineRenderer>();
        vl = GetComponent<VolumetricLineBehavior>();
        ResetLineToInitState();
        fstate = FingerState.Idle;
        segmentTrack = new List<Segment>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        Debug.Log("Platform: Unity Editor Mode");
#endif

#if UNITY_STANDALONE_WIN
        Debug.Log("Platform: Win Standalone Mode");
#elif UNITY_STANDALONE_OSX
        Debug.Log("Platform: OSX Standalone Mode");
#endif

#if UNITY_IOS
        Debug.Log("Platform: IOS Mode");
#elif UNITY_ANDROID
        Debug.Log("Platform: Android Mode");
#endif

        //TODO test
        List<ColorClass> l = GameManager.Instance.colors;
        targetLayer = 0;
        passColor = GameManager.Instance.GetColorClass(ColorName.Blue);
        for (int i = 0; i < l.Count; i++)
        {
            targetLayer |= l[i].layerMask;
        }
    }

    private void Update()
    {
        fingerPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

        if (disableControl)
            return;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Control_WIN();
#elif UNITY_ANDROID
        Control_WIN();
#elif UNITY_IOS
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Control_WIN();
        }
#endif

        if (segmentTrack.Count > 0)
        {
            Segment segRef = segmentTrack[0];
            if (niba.transform.position.y < segRef.transform.position.y)
            {
                segRef.Break(niba.transform.position);
                segmentTrack.Remove(segRef);
            }
        }
    }

    private void Control_WIN()
    {
        if (fstate == FingerState.Idle && Input.GetMouseButtonDown(0))
        {          
            if (fingerPos.y <= 0)
            {
                Debug.Log("Invalid finger start point");
                fstate = FingerState.Idle;
                return;
            }
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                CutStart();
        }

        if (fstate == FingerState.Drawing && Input.GetMouseButtonUp(0))
        {
            CutFinish();
        }

        if (fstate == FingerState.Drawing && Input.GetMouseButton(0))
        {
            CutPlanning();
        }
    }

    private void CutStart()
    {
        start = fingerPos;
        //lr.SetPosition(0, start);
        vl.StartPos = start;
        fstate = FingerState.Drawing;
    }

    private void CutPlanning()
    {
        end = fingerPos;
        //lr.SetPosition(1, fingerPos);
        vl.EndPos = end;
    }

    private void CutFinish()
    {
        fstate = FingerState.Idle;
        SliceResult pass = CutCollisionCheck();
 
        StartCoroutine(CutDeliverProcess(pass));
    }

    private SliceResult CutCollisionCheck()
    {
        if (end.y > 0f)
        {
            Debug.LogWarning("Finger end position y > 0 -----> Retry");
            return SliceResult.Retry;
        }

        segmentTrack.Clear();
        Vector2 dir = (end - start).normalized;
        RaycastHit2D[] hitPoints = Physics2D.RaycastAll(start, dir, 20f, targetLayer);

        for (int i = 0; i < hitPoints.Length; i++)
        {
            Segment seg = hitPoints[i].transform.GetComponent<Segment>();
            if (!seg)
            {
                Debug.LogError("Hit something which is not a segment, this is an exception");
                return SliceResult.Fail;
            }

            if (seg.SegColorClass == passColor)
            {
                segmentTrack.Add(seg);
                Debug.Log("pass one : " + passColor.colorName.ToString() + " on " + seg.SegColorClass.colorName.ToString());
            }
            else
            {
                //第一个碰到的障碍
                Debug.Log("hit one : " + passColor.colorName.ToString() + " on " + seg.SegColorClass.colorName.ToString());
                end = hitPoints[i].point;
                GameManager.deadCount++;
                return SliceResult.Fail;
            }
        }

        if (end.y >= GameManager.Instance.level.BottomLinePos)
        {
            Debug.LogWarning("finger ends at the middle area");
            GameManager.deadCount++;
            return SliceResult.Fail;
        }
        return SliceResult.Pass;
    }

    private IEnumerator CutDeliverProcess(SliceResult result)
    {
        niba.OnSliceReady(start, end, result);

        float startWidth = 0.1f;
        float endWidth = 0.005f;
        float flashTime = 0.25f;

        float timer = 0f;
        while (timer < flashTime)
        {
            vl.LineWidth = Mathf.Lerp(startWidth, endWidth, timer / flashTime);
            vl.LineColor = Color.Lerp(colop1, colop2, timer / flashTime);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return null;
        ResetLineToInitState();
    }

    private void ResetLineToInitState()
    {
        vl.LineWidth = 0.01f;
        vl.LightSaberFactor = 0.8f;
        vl.LineColor = colop1;
        vl.StartPos = Vector3.zero;
        vl.EndPos = Vector3.zero;
        //lr.SetPosition(0, Vector3.zero);
        //lr.SetPosition(1, Vector3.zero);
        start = Vector3.zero;
        end = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (!gizmosOn)
            return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(start, 0.05f);
        Gizmos.DrawWireSphere(end, 0.05f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, end);
    }
}

public enum FingerState
{
    Idle,
    Drawing
}
