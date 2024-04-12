using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class CreateLobbyScript : MonoBehaviour
{
    [SerializeField] private Button createPublic;
    [SerializeField] private Button createPrivate;
    [SerializeField] private Button close;
    [SerializeField] private TMP_InputField lobbyName;
    [SerializeField] private TMP_Text error;

    void Start()
    {
        Hide();
    }
    void Awake()
    {
        createPublic.onClick.AddListener(()=>{
            if(lobbyName.text.IsNullOrEmpty()){
                error.text = "Lobby Name cannot be empty";
                return;
            }
            error.text = "";
            GameLobbyScript.Instance.CreateLobby(lobbyName.text,false);
        });
        createPrivate.onClick.AddListener(()=>{
            if(lobbyName.text.IsNullOrEmpty()){
                error.text = "Lobby Name cannot be empty";
                return;
            }
            error.text = "";
            GameLobbyScript.Instance.CreateLobby(lobbyName.text,true);
        });
        close.onClick.AddListener(()=>{
            Hide();
        });
    }
    public void Show(){
        gameObject.SetActive(true);
    }
    public void Hide(){
        gameObject.SetActive(false);
    }
}
