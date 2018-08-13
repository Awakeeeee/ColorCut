using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewColorClass", menuName = "Create Color Class")]
public class ColorClass : ScriptableObject
{
    public Color color;
    public LayerMask layerMask;
    public int layerFlag {
        get
        {
            int val = layerMask.value;
            int remain = 0;
            int flag = 0;
            while (remain == 0)
            {
                val /= 2;
                remain = val % 2;
                flag++;
            }
            return flag;
        }
    }
    public ColorName colorName;
}

public enum ColorName
{
    None,
    Red,
    Orange,
    Yellow,
    Green,
    Cyan,
    Blue,
    Pueple
}
