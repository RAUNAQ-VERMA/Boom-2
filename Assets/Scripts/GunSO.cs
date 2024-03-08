using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun",menuName ="Scriptable Objects/Gun")]
public class GunSO : ScriptableObject
{
    [Header("Info")]
    public string type;
    public new string name;
    public int weaponIndex;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public float fireRate, reloadTime, recoil;
    public int magazineSize,currentAmmo;

    [HideInInspector]
    public bool reloading;
}
