using Unity.Netcode;
using UnityEngine;

public class WeaponSetupScript : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;
     void Start()
    {
        if(!IsLocalPlayer){
            for (int i = 0; i<componentsToDisable.Length; i++){
                componentsToDisable[i].enabled =false;
            }
        }
    }
}
