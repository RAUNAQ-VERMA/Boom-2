using TMPro;
using UnityEngine;

public class GameTimerUIScript : MonoBehaviour
{
    [SerializeField] private TMP_Text timerUI;
    int timer;
    void Update()
    {
        timer = (int)GameStateManagerScript.Instance.GetGamePlayingTimer();
        timerUI.SetText((int)timer/60+":"+(int)timer%60);
    }
}
