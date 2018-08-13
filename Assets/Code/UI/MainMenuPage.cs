using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuPage : UIPage
{
    public Button start_btn;
    public CanvasGroup cg;

    private void Start()
    {
        start_btn.onClick.AddListener(OnClickStartBtn);
    }

    private void OnEnable()
    {
        cg.interactable = true;
        cg.blocksRaycasts = true;
        cg.alpha = 1f;
        StopAllCoroutines();
    }

    private void OnClickStartBtn()
    {
        //GameManager.Instance.LOAD_MAIN_TO_GAME();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutProcess());
    }

    IEnumerator FadeOutProcess()
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;
        float timer = 0f;

        while(timer < 0.5f)
        {
            float al = Mathf.Lerp(1f, 0f, timer / 0.5f);
            cg.alpha = al;
            timer += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 0f;
    }
}
