using System;
using UnityEngine;

public class GameOverUIScript : MonoBehaviour
{
    void Start()
    {
        GameStateManagerScript.OnStateChanged += GameStateManager_OnStateChange;
        Hide();
    }

    private void GameStateManager_OnStateChange(object sender, EventArgs e)
    {
        if(GameStateManagerScript.Instance.IsGameOver()){
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
        gameObject.SetActive(true);
    }
}
