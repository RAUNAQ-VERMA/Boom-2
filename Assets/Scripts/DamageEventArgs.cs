using System;
using UnityEngine;

public class DamageEventArgs : EventArgs
{
    public string playerId;
    public Vector3 bulletTransform;
    public GunSO gunInfo;
}
