using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : NetworkBehaviour
{
    public static GameManagerScript Instance{get;private set;}
    private const string PLAYER_ID_PREFIX = "Player";
    private static Dictionary<string,PlayerScript> players = new Dictionary<string, PlayerScript>();

    [SerializeField] private Transform playerPrefab;
    void Start()
    {
        Instance = this;
       // DamageLogicScript.OnDamage += OnDamageAction;
    }
    public override void OnNetworkSpawn(){
        if(IsServer){
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted+= NetworkManager_OnLoadEventCompleted;
        }
    }

    private void NetworkManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        Debug.Log("Spawning Player");
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds){
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId,true);
        }
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
