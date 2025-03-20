using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(gameScene);
        // won't work unless it is in build profile, whatever that means
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
