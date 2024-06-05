using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Button credits;
    [SerializeField] private Button quit;

    void Awake()
    {
        play.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.LobbyScene);
        });
        credits.onClick.AddListener(()=>{
            Show();
        });
        quit.onClick.AddListener(()=>{
            Application.Quit();
        });
    }  

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
