using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewLevelDesignParamsFile", menuName = "Level design param file")]
public class LevelDesignParam : ScriptableObject
{
    //general
    public int level_ID;
    public string level_name;
    public ColorName pass_color;
    //public Line line_prefab;

    //line config
    public List<LineControlParameters> lines;
    public int line_number { get { return lines.Count; } }
}
