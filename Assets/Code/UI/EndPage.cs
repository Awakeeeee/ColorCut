using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndPage : MonoBehaviour
{
    public Button back;
    public Button quit;

    private void Start()
    {
        back.onClick.AddListener(OnClickBack);
        quit.onClick.AddListener(OnClickQuit);
    }

    private void OnClickQuit()
    {
        Debug.Log("[zqdebug] application quit is called on end scene");
        Application.Quit();
    }
    private void OnClickBack()
    {
        SceneManager.LoadScene(0);
    }
}
