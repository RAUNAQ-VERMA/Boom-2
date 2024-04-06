using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance{get;private set;}
    private const string PLAYER_ID_PREFIX = "Player";
    private static Dictionary<string,PlayerScript> players = new Dictionary<string, PlayerScript>();

    void Start()
    {
        Instance = this;
       // DamageLogicScript.OnDamage += OnDamageAction;
    }

    public void OnDamageAction(GunSO gunInfo, Vector3 bulletDirection, String playerId)
    {
        PlayerScript player = GetPlayerFromId(playerId);
       // player.ReciveDamage(bulletDirection,gunInfo);
    }

    public void RegisterPlayer(string playerId, PlayerScript player){
        string id = PLAYER_ID_PREFIX+ playerId;
        players.Add(id,player);
        player.transform.name = id;
    }
    public void UnRegisterPlayer(string id){
        players.Remove(id);
    }
    public PlayerScript GetPlayerFromId(string id){
        return players[id];
    }
}
