using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    // Start is called before the first frame update
    void Start()
    {
        GameStateManagerScript.OnStateChanged += GameStateManager_OnStateChange;
    }

    private void GameStateManager_OnStateChange(object sender, EventArgs e)
    {
        if(GameStateManagerScript.Instance.IsCountdownToStartActive()){
            Show();
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

    // Update is called once per frame
    void Update()
    {
        countdownText.text =Mathf.Ceil( GameStateManagerScript.Instance.GetCountdownToStartTimer()).ToString();
    }
}
