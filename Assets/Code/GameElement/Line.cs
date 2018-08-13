using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineFixedParameters fparams;
    public LineControlParameters cparams;
    
    [SerializeField]
    private List<float> segStartPoints;
    [SerializeField]
    private List<float> segLengthList;
    [SerializeField]
    private List<Segment> segments;

    private float colorTimer;
    private float lengthTimer;

    private void Awake()
    {
        segStartPoints = new List<float>();
        segLengthList = new List<float>();
        segments = new List<Segment>();

        fparams.line_length = GameManager.screenWidth;
    }

    private void Start()
    {
    }

    public void ConfigSelf()
    {
        this.transform.position = cparams.start_point;
        for (int i = 0; i < cparams.seg_number; i++)
        {
            Segment newSeg = Instantiate(fparams.seg_prefab, this.transform);
            segments.Add(newSeg);
            segStartPoints.Add(0);
            segLengthList.Add(0);
        }
        ResetLine();
    }

    private void Update()
    {
        colorTimer += Time.deltaTime; ;
        lengthTimer += Time.deltaTime;
        if (colorTimer > cparams.color_update_itv)
        {
            UpdateSegmentsColor();
            colorTimer = 0;
        }
        if (lengthTimer > cparams.length_update_itv)
        {
            UpdateSegmentsLength();
            lengthTimer = 0;
        }
    }

    public void ResetLine()
    {
        colorTimer = 0f;
        lengthTimer = 0f;
        foreach (var seg in segments)
            seg.ResetSelf();
        UpdateSegmentsColor();
        UpdateSegmentsLength();
    }

    public void UpdateSegmentsColor()
    {
        List<ColorClass> sequence = ShuffleSegmentColor();
        for (int i = 0; i < cparams.seg_number; i++)
        {
            segments[i].UpdateColor(sequence[i]);
        }
    }
    public void UpdateSegmentsLength()
    {
        ShuffleSegmentLength();
        for (int i = 0; i < cparams.seg_number; i++)
        {
            segments[i].UpdateLength(segLengthList[i], segStartPoints[i]);
        }
    }

    private void ShuffleSegmentLength()
    {
        segStartPoints[0] = 0;
        for (int i = 1; i < cparams.seg_number; i++)
        {
            float lastStart = segStartPoints[i - 1];
            float thisStart = Random.Range(lastStart + fparams.seg_min_length, fparams.line_length - fparams.seg_min_length * (cparams.seg_number- i));
            segStartPoints[i] = thisStart;
            segLengthList[i - 1] = thisStart - lastStart;
        }
        segLengthList[cparams.seg_number - 1] = fparams.line_length - segStartPoints[cparams.seg_number - 1];
    }
    private List<ColorClass> ShuffleSegmentColor()
    {
        //shuffle color class to generate random sequence
        List<ColorClass> shuffleList = GameManager.Instance.colors;
        for (int i = 0; i < shuffleList.Count; i++)
        {
            int ran = Random.Range(i, shuffleList.Count);
            ColorClass temp = shuffleList[i];
            shuffleList[i] = shuffleList[ran];
            shuffleList[ran] = temp;
        }
        //make sure pass color is in the random sequence
        for (int i = 0; i < shuffleList.Count; i++)
        {
            if (shuffleList[i] == LevelManager.Instance.PassColor)
            {
                if (i < cparams.seg_number)
                {
                    return shuffleList;
                }
                else
                {
                    int pass = Random.Range(0, cparams.seg_number - 1);
                    ColorClass temp = shuffleList[i];
                    shuffleList[i] = shuffleList[pass];
                    shuffleList[pass] = temp;
                    return shuffleList;
                }
            }
        }
        Debug.LogError("Cannot find the color to pass in the color database");
        return null;
    }
}

[System.Serializable]
public class LineFixedParameters
{
    public Segment seg_prefab;
    public float seg_min_length;
    public float line_length;
}

[System.Serializable]
public class LineControlParameters
{
    public Vector3 start_point;
    [Range(0, 7)]
    public int seg_number;
    public float color_update_itv;
    public float length_update_itv;
    //public float thickness;
}
