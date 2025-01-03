using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        SampleScene,
        LoadingScene
    }

    private static Scene targetScene;

    // Hàm load scene và xác định scene tiếp theo
    public static void Load(Loader.Scene targetScene)
    {
        Loader.targetScene = targetScene;

        // Chuyển đến LoadingScene
        SceneManager.LoadScene(Loader.Scene.LoadingScene.ToString());
    }

    // Callback sau khi LoadingScene hoàn tất
    public static void LoaderCallback()
    {
        // Load scene đích sau khi loading
        SceneManager.LoadScene(targetScene.ToString());
    }
}
