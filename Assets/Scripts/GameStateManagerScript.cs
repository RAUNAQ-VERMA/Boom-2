using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameStateManagerScript : NetworkBehaviour
{
    public static GameStateManagerScript Instance{get;private set;}
    public static EventHandler OnStateChanged;
    public static EventHandler OnPlayerReadyChanged;
    private enum State{
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,

    }

    private Dictionary<ulong,bool> playerReadyDictionary;
    private NetworkVariable <State>  state = new NetworkVariable<State>(State.CountdownToStart); 
    
    private bool isPlayerReady;

    private NetworkVariable<float> countdownToStartTimer =new NetworkVariable<float> (3f);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(5f);//change to 120f in final build 
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged+= State_OnValueChanged;
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this,EventArgs.Empty);
    }

    void Awake()
    {
        Instance= this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
       //s DontDestroyOnLoad(gameObject);
        //state initiealization was here
    }

    void Update()
    {
        if(!IsServer){
            return;
        }
        switch(state.Value){
            case State.WaitingToStart:
                break;

            case State.CountdownToStart:
                countdownToStartTimer.Value-=Time.deltaTime;
                if(countdownToStartTimer.Value<0){
                    state.Value = State.GamePlaying;
                }
                break;

            case State.GamePlaying:
                gamePlayingTimer.Value-=Time.deltaTime;
                if(gamePlayingTimer.Value<0){
                    state.Value = State.GameOver;
                }
                break;

            case State.GameOver:
                break; 
        }
        Debug.Log(state.Value.ToString());
    }

    //call on clicking ready on the lobby scene(This was for the waiting for players... scene)
    [ServerRpc(RequireOwnership =false)]
    private void SetPlayerReady_ServerRpc(ServerRpcParams rpcParams= default){
        playerReadyDictionary[rpcParams.Receive.SenderClientId] = true;
        bool allClientsReady = true;
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds){
            if(!playerReadyDictionary.ContainsKey(clientId)||!playerReadyDictionary[clientId]){
                allClientsReady=false;
                break;
            }
        }
        if(allClientsReady){
            state.Value = State.CountdownToStart;
        }
    }

    public bool IsGamePlaying(){
        return state.Value==State.GamePlaying;
    }
    public bool IsCountdownToStartActive(){
        return state.Value==State.CountdownToStart;
    }
    public bool IsGameOver(){
        return state.Value==State.GameOver;
    }
    public float GetCountdownToStartTimer(){
        return countdownToStartTimer.Value;
    }
    public bool IsPlayerReady(){
        return isPlayerReady;
    }
}
