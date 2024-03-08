using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IWeaponParent
{
    public Transform GetWeaponHolderTransform();
    public NetworkObject GetNetworkObject();
    public void ClearWeapon();
    public WeaponScript GetCurrentWeapon();
    public void SetCurrentWeapon(WeaponScript weaponObject);
    public bool HasWeapon();
}
