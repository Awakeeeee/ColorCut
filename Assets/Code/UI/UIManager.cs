using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    public Dictionary<UIID, string> ui_config_table;

    public Dictionary<UIID, UIPage> ui_dict_all;
    public Dictionary<UIID, UIPage> ui_dict_show;

    private AssetBundle uiBundle;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(_instance.gameObject);
        }

        //DontDestroyOnLoad(gameObject);

        ui_config_table = new Dictionary<UIID, string>
        {
            { UIID.Background, "uires/BackgroundPage"},
            { UIID.MainMenu, "uires/MainMenuPage"},
            { UIID.GameHUD, "uires/GameHUDPage"},
            { UIID.Story, "uires/StoryPage"}
        };
        ui_dict_all = new Dictionary<UIID, UIPage>();
        ui_dict_show = new Dictionary<UIID, UIPage>();
    }

    private void Start()
    {
        uiBundle = AssetBundle.LoadFromFile(GameManager.bundlePath + "ui.var1");

        LoadPage(UIID.Background);
        LoadPage(UIID.MainMenu);
        LoadPage(UIID.GameHUD);
        LoadPage(UIID.Story);
        ShowPage(UIID.MainMenu);

        uiBundle.Unload(false);
    }

    public void HidePage(UIID uid)
    {
        if (!ui_dict_show.ContainsKey(uid))
        {
            Debug.LogError("Hide ui fails: " + uid + " is not exist in game.");
            return;
        }
        ui_dict_show[uid].Hide();
        ui_dict_show.Remove(uid);
    }

    public UIPage ShowPage(UIID uid)
    {
        UIPage page = GetPage(uid);

        if (page == null)
            return null;

        if (page.IsActive)
        {
            Debug.LogWarning("Show ui not needed: " + uid + " is already showing.");
            return null;
        }

        switch (page.popupMode)
        {
            case PopupMode.Hide_All:
                foreach (var p in ui_dict_all.Values)
                {
                    p.Hide();
                    ui_dict_show.Clear();
                }
                break;
            case PopupMode.Simple:
                break;
            case PopupMode.Hide_Other_Peer:
                foreach (var p in ui_dict_show.Values)
                {
                    if (p.pageLevel == page.pageLevel)
                        p.Hide();
                }
                break;
            default:
                break;
        }
        page.Show();
        ui_dict_show.Add(uid, page);
        return page;
    }

    public UIPage GetPage(UIID uid)
    {
        if (!ui_dict_all.ContainsKey(uid))
        {
            Debug.LogError("Get ui fails: " + uid + " is not exist in game.");
            return null;
        }
        return ui_dict_all[uid];
    }

    public void LoadPage(UIID uid)
    {
        if (ui_dict_all.ContainsKey(uid))
        {
            Debug.LogWarning("UI duplicate: " + uid + " already exists.");
            return;
        }

        if (!ui_config_table.ContainsKey(uid))
        {
            Debug.LogError("Load UI fails: " + uid + " is not found in ui config table.");
            return;
        }

        string path = ui_config_table[uid];
        //UIPage page = Resources.Load<UIPage>(path);
        GameObject assetObj = uiBundle.LoadAsset(uid.ToString()+ ".prefab") as GameObject;
        UIPage page_obj = assetObj.GetComponent<UIPage>();
        if (page_obj == null)
        {
            Debug.LogError("Load UI asset fails: " + uid + " cannot be loaded.");
            return;
        }
        UIPage page = Instantiate(page_obj, this.transform);
        page.Hide();
        ui_dict_all.Add(uid, page);
    }
}

public enum UIID
{
    Background,
    MainMenu,
    GameHUD,
    Story
}
