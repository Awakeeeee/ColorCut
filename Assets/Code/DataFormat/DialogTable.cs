using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Dialog Table", menuName ="Dialog Table File")]
public class DialogTable : ScriptableObject
{
    [Multiline]
    public List<string> dialogs;

    public string Pick()
    {
        if (dialogs == null || dialogs.Count < 1)
            return string.Empty;
        int ran = Random.Range(0, dialogs.Count - 1);
        return dialogs[ran];
    }
}
