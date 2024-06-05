using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class LobbyUIScript : MonoBehaviour
{
    [SerializeField] private Button mainMenu;
    [SerializeField] private Button createLobby;
    [SerializeField] private Button joinLobby;
    [SerializeField] private Button joinWithCode;
    [SerializeField] private TMP_InputField lobbyCode;
    [SerializeField] private CreateLobbyScript createLobbyScript;

    [SerializeField] private Transform lobbyListContainer;
    [SerializeField] private Transform lobbyListTemplate;

    void Awake()
    {
        mainMenu.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        createLobby.onClick.AddListener(()=>{
            createLobbyScript.Show();
        });
        joinLobby.onClick.AddListener(()=>{
            GameLobbyScript.Instance.QuickJoinLobby();
        });
        joinWithCode.onClick.AddListener(()=>{
            if(lobbyCode.text.IsNullOrEmpty()){
                return;
            }
            GameLobbyScript.Instance.JoinLobbyWithCode(lobbyCode.text);
        });
        lobbyListTemplate.gameObject.SetActive(false);
    }
    void Start()
    {
        GameLobbyScript.Instance.OnLobbyListChanged+= GameLobbyScript_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void GameLobbyScript_OnLobbyListChanged(object sender, GameLobbyScript.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }
    private void UpdateLobbyList(List<Lobby> lobbyList){
        foreach(Transform child in lobbyListContainer){
            if(child==lobbyListTemplate){
                continue;
            }
            Destroy(child.gameObject);
        }
        foreach(Lobby lobby in lobbyList){
            Transform lobbyTransform = Instantiate(lobbyListTemplate,lobbyListContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyTemplateUIScript>().SetLobby(lobby);
        }
    }
}
