using Unity.Netcode;
using UnityEngine.SceneManagement;

public static class Loader 
{
    private static Scene targetScene;
    public enum Scene{
        MainMenuScene,
        GameScene,
        LoadingScene,
        LobbyScene,
        CharacterLobbyScene,
    }
    public static int targetSceneIndex;
    
    public static void Load(Scene scene){
        targetScene =scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
    public static void LoadNetwork(Scene scene){
        NetworkManager.Singleton.SceneManager.LoadScene(scene.ToString(),LoadSceneMode.Single);
    }
    public static void LoaderCallback(){
        SceneManager.LoadScene(targetScene.ToString());
    }
        
    
}

