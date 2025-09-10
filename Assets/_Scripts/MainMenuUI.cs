using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.LoadScene(Scene.GameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
