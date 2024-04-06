using System;
using Unity.Netcode;
using UnityEngine;

public class DamageLogicScript : NetworkBehaviour
{
    public static DamageLogicScript Instance{get;private set;}
    public static EventHandler<int> OnDamage;
    
    void Start()
    {
        Instance = this;
    }
    private GunSO gunInfo;
    private Vector3 bulletDirection;
    private String playerId;

    public void ReciveDamage(GunSO gunInfo,Vector3 bulletDirection,String playerId){
    //apply knockback
    this.gunInfo = gunInfo;
    this.bulletDirection = bulletDirection;
    this.playerId = playerId;
    ReciveDamage_ServerRpc();
    }


    [ServerRpc(RequireOwnership =false)]
    public void ReciveDamage_ServerRpc(){
        ReciveDamage_ClientRpc();
    }

    [ClientRpc]
    public void ReciveDamage_ClientRpc(){
   //   transform.position = knockback;
      //  OnDamage?.Invoke(this,playerId);
        GameManagerScript.Instance.OnDamageAction(gunInfo,bulletDirection,playerId);
    //  player.ReciveDamage(bulletDirection, gunInfo);
    }
}
