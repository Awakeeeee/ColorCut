using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WposDialog : MonoBehaviour
{
    public Text dialogText;
    public CanvasGroup cg;

    public void Show(string dialog)
    {
        dialogText.text = dialog;
        cg.alpha = 1;
        gameObject.SetActive(true);
        Invoke("FadeAway", 1.5f);
    }

    private void FadeAway()
    {
        StartCoroutine(FadeAwayProcess());
    }
    private IEnumerator FadeAwayProcess()
    {
        float timer = 0f;
        while (timer < 1f)
        {
            float al = Mathf.Lerp(1f, 0f, timer / 1f);
            cg.alpha = al;
            timer += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 0f;
        gameObject.SetActive(false);
    }
}
