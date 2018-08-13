using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDPage : UIPage
{
    public Button pauseBtn;
    public Button quitBtn;

    private void Start()
    {
        pauseBtn.onClick.AddListener(OnClickPauseBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
        quitBtn.gameObject.SetActive(false);
    }

    private void OnClickPauseBtn()
    {
        bool isPausedAfterClick = GameManager.Instance.Pause();
        if (isPausedAfterClick)
        {
            StopAllCoroutines();
            StartCoroutine(PushQuitBtn());
        }
        else
        {
            quitBtn.gameObject.SetActive(false);
        }
    }
    private IEnumerator PushQuitBtn()
    {
        quitBtn.gameObject.SetActive(true);
        RectTransform trans = quitBtn.GetComponent<RectTransform>();
        CanvasGroup cg = quitBtn.GetComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        cg.interactable = false;
        trans.sizeDelta = new Vector2(0f, trans.sizeDelta.y);
        float timer = 0f;
        while(timer < 0.5f)
        {
            float newX = Mathf.Lerp(0f, 200f, timer / 0.5f);
            trans.sizeDelta = new Vector2(newX, trans.sizeDelta.y);
            timer += Time.deltaTime;
            yield return null;
        }
        trans.sizeDelta = new Vector2(200f, 200f);
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    private void OnClickQuitBtn()
    {
        Debug.Log("[zqdebug] application quit is called");
        Application.Quit();
    }
}
