using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUpScript : MonoBehaviour
{
    void Awake()
    {
        if(NetworkManager.Singleton!=null){
            Destroy(NetworkManager.Singleton.gameObject);
        }
        if(GameMultiplayerScript.Instance!=null){
            Destroy(GameMultiplayerScript.Instance.gameObject);
        }
    }
}
