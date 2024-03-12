using System;
using UnityEngine;

public class WeaponSpawnerScript : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        GameInput.Instance.OnTestAction += SpawnWeapon;
    }

    private void SpawnWeapon(object sender, EventArgs e)
    {
        WeaponSpawnLogicScript.Instance.SpawnWeapon();
    }
}
