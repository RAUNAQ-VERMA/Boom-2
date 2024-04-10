using System;
using UnityEngine;

public class WeaponSpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update

    private float weaponSpawnTimer=180f;
    void Start()
    {
        GameInput.Instance.OnTestAction += SpawnWeapon;
    }
    void Update()
    {   //This is the code for weapon spawntimer test and deploy in final build
        // weaponSpawnTimer-=Time.deltaTime;
        // if(weaponSpawnTimer%60==0){
        //     SpawnWeapon();
        // }
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
