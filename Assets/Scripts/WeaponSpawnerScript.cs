using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeaponSpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        GameInput.Instance.OnAttackAction += SpawnWeapon;
        
    }

    private void SpawnWeapon(object sender, EventArgs e)
    {
        WeaponSpawnLogicScript.Instance.SpawnWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
