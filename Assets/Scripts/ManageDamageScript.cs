using Unity.Netcode;
using UnityEngine;

public class ManageDamageScript : NetworkBehaviour
{
    [SerializeField] private CharacterController controller;
    private GunSO gunInfo;
    private Vector3 bulletDirection;
    public void ReciveDamage(GunSO gunInfo,Vector3 bulletDirection){
      //apply knockback
      // Vector3 knockback = transform.forward + (bulletDirection * gunInfo.damage);
      // controller.Move(knockback);
      if(!IsOwner){
        return;
      }
      this.gunInfo = gunInfo;
      this.bulletDirection = bulletDirection;
      ReciveDamage_ServerRpc();
    }


    [ServerRpc(RequireOwnership =false)]
    public void ReciveDamage_ServerRpc(){
        ReciveDamage_ClientRpc();
    }
    [ClientRpc]
    public void ReciveDamage_ClientRpc(){
    Vector3 knockback = transform.forward + (bulletDirection * gunInfo.damage);
    if(controller==null){
      Debug.Log("Controller is null : Manage Damage Script");
    }
    controller.Move(knockback);
    
    Debug.Log(transform.position+"||"+knockback+"||"+transform.name);
  //  PlayerScript.LocalInstance.ReciveDamage(bulletDirection,gunInfo);
    }
}
