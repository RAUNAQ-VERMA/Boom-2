using UnityEngine;
using UnityEngine.UI;

public class TestingCharacterLobbyUiScript : MonoBehaviour
{
    [SerializeField] private Button ready;

    void Awake()
    {
        ready.onClick.AddListener(()=>{
            PlayerReadyScript.Instance.SetPlayerReady();
        });
    }
}
