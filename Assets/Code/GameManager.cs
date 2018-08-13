using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The game system has access to all components
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public static int deadCount;

    [Header("Constant")]
    public const int Total_Level_Num = 7;
    public static string bundlePath { get
        {
            string path = Application.streamingAssetsPath + "/AssetBundles/";
            Debug.Log("[zqdebug] platform bundle path is :" + path);
            return path;
        } }

    [Header("Access to other game components")]
    public UIManager ui;
    public LevelManager level;
    public CameraEffect screen;
    public CameraBehaviour cam;
    public SoundManager sound;
    public Niba niba;
    public Finger finger;
    public WposDialog dialogUI;

    [Header("Game data")]
    public List<ColorClass> colors;
    public DialogTable dialogs;
    public List<LevelDesignParam> levelDocs;
    public List<Vector3> journeyPoints;
    public Transform journeyLinesContainer;
    private JourneyLine[] journeyLines;

    public int topLevelNum;
    public LevelDesignParam currentLevelDoc { get { return levelDocs[topLevelNum]; } }

    public static float screenWidth;
    public static float screenHeight;

    private bool paused;
    private bool backBtnClickedOnce;
    private float backTimer;

    //---Android Interaction
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject memberCurrentActivity;

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
        
        //LOAD GAME
        topLevelNum = PlayerPrefs.GetInt("TopLevelReached");
        deadCount = PlayerPrefs.GetInt("DeadCount");

        if (ui == null)
            ui = UIManager.Instance;
        if (level == null)
            level = LevelManager.Instance;
        if (screen == null)
            screen = CameraEffect.Instance;
        if (cam == null)
            cam = Camera.main.GetComponent<CameraBehaviour>();
        if (levelDocs.Count == 0)
        {
            Debug.LogError("No level data loaded");
            return;
        }

        screenHeight = cam.Height;
        screenWidth = cam.Width;
    }

    private void Start()
    {
#if TEST_FLAG
        Debug.Log("Test vertion");
#endif
        paused = false;
        backBtnClickedOnce = false;
        backTimer = 0f;

        //TEST
        topLevelNum = 0;
        deadCount = 0;

        journeyLines = journeyLinesContainer.gameObject.GetComponentsInChildren<JourneyLine>(true);
        for (int i = 0; i < journeyPoints.Count; i++)
        {
            journeyPoints[i] = new Vector3(screenWidth / 2f, journeyPoints[i].y, journeyPoints[i].z);
        }

#if UNITY_ANDROID
        //这个类是unity提供的
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //UnityPlayer类中有一个publci static Activity currentActivity的成员,通过这样的语法获得他
        memberCurrentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

    private void Android_Unity_Show_Toast(string toastString)
    {
        Debug.Log("[zqdebug] call toast method in unity, not through aar");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
        AndroidJavaObject context = memberCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject t = toastClass.CallStatic<AndroidJavaObject>("makeText", context, javaString, toastClass.GetStatic<int>("LENGTH_LONG"));
        t.Call("show");
    }
    private void Android_Show_Toast(string toastString)
    {
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
        memberCurrentActivity.Call("showToast", javaString);
    }
    private void Android_DoubleClick_Back()
    {
        if (backBtnClickedOnce)
        {
            backTimer += Time.deltaTime;
            Debug.Log("[zqdebug] back btn is waiting for the second click, timer: " + backTimer.ToString());
            if (backTimer > 2.0f)
            {
                backTimer = 0f;
                backBtnClickedOnce = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //android back btn
        {
            if (backBtnClickedOnce)
            {
                Debug.Log("[zqdebug] application quit is called on double click back btn");
                backBtnClickedOnce = false;
                Application.Quit();
            }
            else
            {
                //Android_Show_Toast("Double click to quit game :)");
                Android_Unity_Show_Toast("Double click to quit game :)");
                backBtnClickedOnce = true;
            }
        }
    }

    private void Update()
    {
#if UNITY_ANDROID
        Android_DoubleClick_Back();
#endif
    }

    public bool Pause()
    {
        if (paused)
        {
            finger.DisableControl = false;
        }
        else
        {
            finger.DisableControl = true;   
        }
        paused = !paused;
        return paused;
    }

    public JourneyLine GetJourneyLine()
    {
        journeyLinesContainer.gameObject.SetActive(true);
        foreach (var l in journeyLines)
        {
            if (!l.isActiveAndEnabled)
                return l;
        }
        return null;
    }

    public void ShowWposDialog()
    {
        dialogUI.transform.position = niba.transform.position + Vector3.up * 0.8f;
        dialogUI.Show(dialogs.Pick());
    }

    public ColorClass GetColorClass(ColorName cn)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            if (colors[i].colorName == cn)
                return colors[i];
        }
        Debug.LogError("reqested color is not in color database");
        return null;
    }

    private void OnApplicationQuit()
    {
        //SAVE GAME
        PlayerPrefs.SetInt("TopLevelReached", topLevelNum);
        PlayerPrefs.SetInt("DeadCount", deadCount);
    }
}
