using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPage : UIPage
{
    public Button continue_btn;
    public Button menu_btn;

    private void Start()
    {
        continue_btn.onClick.AddListener(OnClickContinueBtn);
        menu_btn.onClick.AddListener(OnClickMenuBtn);
    }

    private void OnClickContinueBtn()
    {
        //GameManager.Instance.LOAD_STORY_TO_GAME();
    }
    private void OnClickMenuBtn()
    {
        //GameManager.Instance.LOAD_STORY_TO_MAIN();
    }
}
