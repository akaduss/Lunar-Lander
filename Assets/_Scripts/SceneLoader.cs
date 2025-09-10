using UnityEngine.SceneManagement;

public static class SceneLoader
{

    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

}

public enum Scene
{
    MainMenuScene = 0,
    GameScene = 1,
    GameOverScene = 2
}