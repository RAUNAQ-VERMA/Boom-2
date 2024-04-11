using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CharacterLobbyUIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyName;
    [SerializeField] private TextMeshProUGUI lobbyCode;

    void Start()
    {
        Lobby lobby = GameLobbyScript.Instance.GetLobby();
        lobbyName.text = "Lobby Name: "+lobby.Name;
        lobbyCode.text = "Lobby Code: "+lobby.LobbyCode;
    }
}
