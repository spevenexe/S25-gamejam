using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeGroup;
    public Animator transition;

    public static LevelLoader Instance;

    public enum TransitionType
    {
        FADE,
        CUT,
    }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // void Start()
    // {
    //     DontDestroyOnLoad(gameObject);
    //     DontDestroyOnLoad(fadeGroup.gameObject);
    // }

    public void LoadNext(TransitionType fadeType,float waitTime = 2f)
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1,fadeType,waitTime));
    }

    private IEnumerator LoadLevel(int buildIndex, TransitionType fadeType,float waitTime)
    {
        switch(fadeType)
        {
            case TransitionType.FADE:
                transition.SetTrigger("Start");
                break;
            case TransitionType.CUT:
                transition.SetTrigger("Cut");
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadSceneAsync(buildIndex % SceneManager.sceneCountInBuildSettings);
    }

    public void LoadNext(int buildIndex,TransitionType fadeType,float waitTime = 2f)
    {
        StartCoroutine(LoadLevel(buildIndex,fadeType,waitTime));
    }

    public void LoadNext(Scene scene,TransitionType fadeType,float waitTime = 2f)
    {
        int buildIndex = scene.buildIndex;
        StartCoroutine(LoadLevel(buildIndex,fadeType,waitTime));
    }
}
