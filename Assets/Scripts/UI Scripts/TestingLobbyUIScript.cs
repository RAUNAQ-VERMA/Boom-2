using UnityEngine;
using UnityEngine.UI;

public class TestingLobbyUIScript : MonoBehaviour
{
    [SerializeField] private Button createGame;
    [SerializeField] private Button joinGame;

    void Awake()
    {
        createGame.onClick.AddListener(()=>{
            GameMultiplayerScript.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterLobbyScene);
        });
        joinGame.onClick.AddListener(()=>{
            GameMultiplayerScript.Instance.StartClient();
            //Here Load is not needed the client will match the server scene.
        });
    }
}
