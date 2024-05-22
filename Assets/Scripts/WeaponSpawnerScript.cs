using System;
using Unity.Netcode;
using UnityEngine;

public class WeaponSpawnerScript : NetworkBehaviour
{
    private float weaponSpawnTimer=0f;
    void Start()
    {
        GameInput.Instance.OnTestAction += SpawnWeapon;
    }
    void Update()
    {   //This is the code for weapon spawntimer test and deploy in final build
        if(IsServer){
            if(weaponSpawnTimer<=0){
                SpawnWeapon();
                weaponSpawnTimer = 30f;
            }
            weaponSpawnTimer-=Time.deltaTime;
        }
    }
    private void SpawnWeapon(object sender, EventArgs e)
    {
        GameMultiplayerScript.Instance.SpawnWeapon();
    }
    private void SpawnWeapon()
    {
        GameMultiplayerScript.Instance.SpawnWeapon();
    }
}
