using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPage : MonoBehaviour
{
    public PageLevel pageLevel;
    public PopupMode popupMode;

    public bool IsActive { get { return isActiveAndEnabled; } }

    protected UIManager ui;

    protected void Awake()
    {
        ui = UIManager.Instance;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

public enum PageLevel
{
    Important,
    Normal
}

public enum PopupMode
{
    Simple,
    Hide_Other_Peer,
    Hide_All
}