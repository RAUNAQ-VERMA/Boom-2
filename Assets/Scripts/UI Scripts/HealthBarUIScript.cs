using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIScript : MonoBehaviour
{
    public static HealthBarUIScript Instance {get;private set;}
    [SerializeField] private static Image healthbar;
    public static EventHandler<String> gameOver;
    private static int health = 100;
    void Awake()
    {
        Instance = this;
    }
    public void Damage(int damage){
        health -= damage;
        healthbar.fillAmount = health/100f;
        if(health<=0){
            PlayerScript.LocalInstance.IsLoser = true;
            GameStateManagerScript.Instance.SetGameOver();
        }
    }
}
