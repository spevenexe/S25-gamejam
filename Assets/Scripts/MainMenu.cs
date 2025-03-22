using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;    
    }

    public void PlayGame()
    {
        LevelLoader.Instance.LoadNext(LevelLoader.TransitionType.FADE);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
