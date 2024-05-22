using System;
using UnityEngine;

public class DamageLogicScript : MonoBehaviour
{
    public static DamageLogicScript Instance{get;private set;}
    ManageDamageScript damageLogic;
    GunSO gunInfo;
    Vector3 bulletTransform;
    public static EventHandler<DamageEventArgs> OnDamage;
    void Start()
    {
        Instance = this;
    }
    // public void ReciveDamage(GunSO gunInfo, Vector3 bulletTransform, string playerId){
    //     PlayerScript player  = GameManagerScript.Instance.GetPlayerFromId(playerId);
    //     damageLogic = player.transform.GetComponent<ManageDamageScript>();
    //     this.gunInfo = gunInfo;
    //     this.bulletTransform = bulletTransform;
    //     damageLogic.ReciveDamage(bulletTransform,gunInfo);
        
    // }
}
