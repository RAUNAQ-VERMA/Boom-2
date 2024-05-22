using System;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIScript : MonoBehaviour
{
    [SerializeField] private Button mainMenu;
    [SerializeField] private TextMeshProUGUI winnerText;
    void Start()
    {
        GameStateManagerScript.OnStateChanged += GameStateManager_OnStateChange;
        Hide();
    }
    void Awake()
    {
        mainMenu.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void GameStateManager_OnStateChange(object sender, EventArgs e)
    {
        if(GameStateManagerScript.Instance.IsGameOver()){
            if(PlayerScript.LocalInstance.IsLoser){
                winnerText.text = "You Lost";
            }
            else{
                winnerText.text = "You Won";
            }
            Show();
            // if(NetworkManager.Singleton!= null){
            // NetworkManager.Singleton.Shutdown();
            // }
        }
        else{
            Hide();
        }
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
    }
}
