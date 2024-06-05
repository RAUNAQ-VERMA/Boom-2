using System;
using UnityEngine;

public class CrosshairUIScript : MonoBehaviour
{
    void Awake()
    {
        GameStateManagerScript.OnStateChanged += GameManagerScript_OnStateChanged;
    }

    private void GameManagerScript_OnStateChanged(object sender, EventArgs e)
    {
        if(GameStateManagerScript.Instance.IsGameOver()){
            Hide();
        }
        else{
            Show();
        }
    }

    public void Show(){
        gameObject.SetActive(true);
    }
    public void Hide(){
        gameObject.SetActive(false);
    }
}
