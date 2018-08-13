using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    public Transform objContainer;
    public LevelDesignParam levelParam;

    private ColorClass passColor;
    public ColorClass PassColor { get { return passColor; } set { passColor = value; } }

    private Line[] levelLines;
    public float BottomLinePos { get { return levelLines[levelLines.Length - 1].transform.position.y; } }

    Line lineAsset;
    AssetBundle prefabBundle;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        prefabBundle = AssetBundle.LoadFromFile(GameManager.bundlePath + "prefabbundle.var1");
        GameObject obj = prefabBundle.LoadAsset("line.prefab") as GameObject;
        lineAsset = obj.GetComponent<Line>();
    }

    public void OnNewLevelLoad()
    {
        levelParam = GameManager.Instance.currentLevelDoc;
        GenerateLevel();
    }
    public void OnLevelUnload(SliceResult result)
    {
        foreach (Line l in levelLines)
            Destroy(l.gameObject);
        objContainer.gameObject.SetActive(false);

        if (result == SliceResult.Pass)
        {
            GameManager.Instance.topLevelNum++;
            if (GameManager.Instance.topLevelNum > GameManager.Total_Level_Num - 1)
            {
                GameManager.Instance.topLevelNum--;
                prefabBundle.Unload(false);
                UnityEngine.SceneManagement.SceneManager.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }
    public void ResetLevel()
    {
        foreach (Line l in levelLines)
            l.ResetLine();
    }
    private void GenerateLevel()
    {
        passColor = GameManager.Instance.GetColorClass(levelParam.pass_color);
        objContainer.gameObject.SetActive(true);

        for (int i = 0; i < levelParam.line_number; i++)
        {
            LineControlParameters data = levelParam.lines[i];
            //Line newLine = Instantiate(levelParam.line_prefab, objContainer) as Line;
            Line newLine = Instantiate(lineAsset, objContainer) as Line;

            if (newLine)
            {
                //NOTE: the cparams field in Line class is a deep copy of the data config, not a reference to it
                //because we use scriptableObject but not modify it
                newLine.cparams = new LineControlParameters();
                newLine.cparams.start_point = data.start_point;
                newLine.cparams.seg_number = data.seg_number;
                newLine.cparams.color_update_itv = data.color_update_itv;
                newLine.cparams.length_update_itv = data.length_update_itv;

                newLine.ConfigSelf();
            }
            else
            {
                Debug.LogError("Read line prefab error");
                return;
            }
        }
        levelLines = objContainer.GetComponentsInChildren<Line>();
    }
}
