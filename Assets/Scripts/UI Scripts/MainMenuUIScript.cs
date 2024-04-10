using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Button quit;

    void Awake()
    {
        play.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.LobbyScene);
        });
        quit.onClick.AddListener(()=>{
            Application.Quit();
        });
    }    
}
